using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Berp
{
    public class StateCalculator
    {
        private const int AFTER_RULE_POSITION = -1;

        private static List<Branch> GetBranches(CallStackItem caller)
        {
            return GetBranchesInternal(caller, new List<ProductionRule>());
        }

        private static List<Branch> GetBranchesInternal(CallStackItem caller, List<ProductionRule> productions)
        {
            productions = productions ?? new List<ProductionRule>();

            bool ruleCanFinish;
            var branches = GetBranchesInSubRules(caller, out ruleCanFinish, productions);

            if (ruleCanFinish && caller.Parent != null)
            {
                productions.Add(new ProductionRule(ProductionRuleType.End, caller.Rule));
                var nextInParent = Advance(caller.Parent, productions);
                if (nextInParent != null)
                    branches.AddRange(GetBranchesInternal(nextInParent, productions));
            }

            return branches;
        }

        private static List<Branch> GetBranchesInSubRules(CallStackItem caller, out bool ruleCanFinish, List<ProductionRule> productions, LookAheadHint lookAheadHint = null)
        {
            var result = new List<Branch>();
            var lookAhead = caller.Rule.LookAheadHint ?? lookAheadHint;

            if (caller.Position == AFTER_RULE_POSITION)
            {
                ruleCanFinish = true;
                return result;
            }

            if (caller.Rule is TokenRule)
            {
                ruleCanFinish = false;
                productions.Add(new ProductionRule(ProductionRuleType.Process, caller.Rule));
                result.Add(CreateBranch(caller, productions, lookAhead));
                return result;
            }

            var ruleElements = ((DerivedRule)caller.Rule).RuleElements;

            if (caller.Rule is SequenceRule)
            {
                int subRuleIndex = caller.Position;
                for (; subRuleIndex < ruleElements.Length; subRuleIndex++)
                {
                    var subRuleElement = ruleElements[subRuleIndex];

                    if (RuleCountInCallStack(subRuleElement.ResolvedRule, caller) > 3)
                    {
                        ruleCanFinish = false;
                        return result;
                    }

                    var subRuleCaller = new CallStackItem(caller.Parent, caller.Rule, subRuleIndex);
                    var subRuleCallStackItem = new CallStackItem(subRuleCaller, subRuleElement.ResolvedRule);
                    bool subRuleCanFinish;

                    var subRuleProductions = new List<ProductionRule>(productions)
                    {
                        new ProductionRule(ProductionRuleType.Start, subRuleElement.ResolvedRule)
                    };
                    var subRuleBranches = GetBranchesInSubRules(subRuleCallStackItem, out subRuleCanFinish, subRuleProductions, lookAhead);

                    result.AddRange(subRuleBranches);

                    bool continueWithNext = subRuleCanFinish || 
                                            subRuleElement.Multilicator == Multilicator.Any ||
                                            subRuleElement.Multilicator == Multilicator.OneOrZero;

                    if (!continueWithNext)
                        break;
                }
                ruleCanFinish = subRuleIndex == ruleElements.Length;
                return result;
            }

            Debug.Assert(caller.Rule is AlternateRule);
            if (caller.Rule is AlternateRule)
            {
                Debug.Assert(caller.Position == 0);

                ruleCanFinish = false;
                for (int subRuleIndex = 0; subRuleIndex < ruleElements.Length; subRuleIndex++)
                {
                    var subRuleElement = ruleElements[subRuleIndex];
                    var subRuleCaller = new CallStackItem(caller.Parent, caller.Rule, subRuleIndex);
                    var subRuleCallStackItem = new CallStackItem(subRuleCaller, subRuleElement.ResolvedRule);
                    bool subRuleCanFinish;

                    var subRuleProductions = new List<ProductionRule>(productions)
                    {
                        new ProductionRule(ProductionRuleType.Start, subRuleElement.ResolvedRule)
                    };
                    var subRuleBranches = GetBranchesInSubRules(subRuleCallStackItem, out subRuleCanFinish, subRuleProductions, lookAhead);

                    result.AddRange(subRuleBranches);
                    ruleCanFinish |= subRuleCanFinish;
                }
                return result;
            }

            throw new NotSupportedException();
        }

        private static int RuleCountInCallStack(Rule rule, CallStackItem callStackItem)
        {
            int result = callStackItem.Rule == rule ? 1 : 0;
            if (callStackItem.Parent == null)
                return result;
            return RuleCountInCallStack(rule, callStackItem.Parent) + result;
        }

        private static Branch CreateBranch(CallStackItem caller, List<ProductionRule> productions, LookAheadHint lookAheadHint)
        {
            var branch = new Branch(((TokenRule)caller.Rule).TokenType, caller, productions)
            {
                LookAheadHint = lookAheadHint
            };
            OptimizeProductions(branch);
            return branch;
        }

        public static Dictionary<int, State> CalculateStates(RuleSet ruleSet)
        {
            var startCallStackItem = new CallStackItem(null, ruleSet.StartRule);
            var branches = GetBranches(startCallStackItem);

            int stateCount = 0;
            var states = new Dictionary<int, State>();
            var startState = new State(stateCount++, branches) { Comment = "Start" };
            states.Add(startState.Id, startState);

            GetNextStates(states, startState, ref stateCount);

            foreach (var state in states.Values.Where(s => !s.IsEndState))
            {
                if (!state.Branches.Any(b => b.TokenType.Equals(TokenType.Other)))
                {
                    foreach (var ignoredToken in ruleSet.IgnoredTokens)
                    {
                        if (!state.Branches.Any(b => b.TokenType.Equals(ignoredToken)))
                            state.AddTransition(new StateTransition(ignoredToken, state.Id, new List<ProductionRule> { new ProductionRule(ProductionRuleType.Process, null) }, null));
                    }
                }
            }

            return states;
        }

        private static void GetNextStates(Dictionary<int, State> states, State state, ref int stateCount)
        {
            var branchGroups = state.Branches.GroupBy(b => b.TokenType);
            foreach (var branchGroup in branchGroups)
            {
                if (branchGroup.Count(b => b.LookAheadHint == null) > 1)
                {
                    throw new ParserGeneratorException("Ambiguous rules. Provide more look-ahead hints. Branches: {0}{1}", Environment.NewLine, string.Join(Environment.NewLine, branchGroup.Where(b => b.LookAheadHint == null)));
                }

                foreach (var branch in branchGroup.OrderBy(b => b.LookAheadHint == null))
                {
                    var nextCallStackItem = new CallStackItem(branch.CallStackItem.Parent, branch.CallStackItem.Rule, AFTER_RULE_POSITION);
                    var nextBranches = GetBranches(nextCallStackItem);
                    var lookAheadHint = branchGroup.Count() == 1 ? null : branch.LookAheadHint;

                    var existingState = states.Values.FirstOrDefault(s => s.Equals(nextBranches));
                    if (existingState != null)
                    {
                        state.AddTransition(new StateTransition(branch.TokenType, existingState.Id, branch.OptimizedProductions, lookAheadHint));
                    }
                    else
                    {
                        var newState = new State(stateCount++, nextBranches)
                            { Comment = branch.CallStackItem.ToString() };
                        states.Add(newState.Id, newState);
                        state.AddTransition(new StateTransition(branch.TokenType, newState.Id, branch.OptimizedProductions, lookAheadHint));
                        GetNextStates(states, newState, ref stateCount);
                    }
                }
            }
        }

        private static CallStackItem Advance(CallStackItem item, List<ProductionRule> productions)
        {
            var sequenceRule = item.Rule as SequenceRule;
            if (sequenceRule != null)
            {
                var ruleElement = sequenceRule.RuleElements[item.Position];

                if (ruleElement.Multilicator == Multilicator.Any/* OneOrMore is trasformed to One + Any already || ruleElement.Multilicator == Multilicator.OneOrMore*/)
                    return item;

                if (item.Position != AFTER_RULE_POSITION && item.Position + 1 < sequenceRule.RuleElements.Length)
                {
                    return new CallStackItem(item.Parent, item.Rule, item.Position + 1);
                }
            }

            // advance the level above
            if (item.Parent == null)
                return null;

            productions.Add(new ProductionRule(ProductionRuleType.End, item.Rule));
            return Advance(item.Parent, productions);
        }

        private static void OptimizeProductions(Branch branch)
        {
            branch.OptimizedProductions = branch.Productions.Where(p => !IsIgnoredProductionRule(p)).Select(OptimizeProduction).ToList();
        }

        private static ProductionRule OptimizeProduction(ProductionRule productionRule)
        {
            if (productionRule.Rule is TokenRule)
            {
                return new ProductionRule(ProductionRuleType.Process, null);
            }
            return productionRule;
        }

        private static bool IsIgnoredProductionRule(ProductionRule productionRule)
        {
            return 
                (productionRule.Rule is TokenRule && productionRule.Type != ProductionRuleType.Process) ||
                (!productionRule.Rule.AllowProductionRules)
                ;
        }
    }
}