using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Berp
{
    public class SequenceRule : DerivedRule
    {
        private RuleElement[] originalRuleElements = null;
        public SequenceRule(string name, params RuleElement[] ruleElements) : base(name, ruleElements)
        {
        }

        public override string GetRuleDescription(bool embedNonProductionRules)
        {
            var result = new StringBuilder();
            foreach (var ruleElement in originalRuleElements ?? RuleElements)
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(ruleElement.ToString(embedNonProductionRules));
            }
            return result.ToString();
        }

        public override void Resolve(RuleSet ruleSet)
        {
            base.Resolve(ruleSet);
            var resolvedRules = new List<RuleElement>();
            foreach (var ruleElement in RuleElements)
            {
                if (ruleElement.Multilicator == Multilicator.OneOrMore)
                {
                    resolvedRules.Add(new RuleElement(ruleElement.RuleName, Multilicator.One) { ResolvedRule = ruleElement.ResolvedRule });
                    resolvedRules.Add(new RuleElement(ruleElement.RuleName, Multilicator.Any) { ResolvedRule = ruleElement.ResolvedRule });
                }
                else
                {
                    resolvedRules.Add(ruleElement);
                }
            }
            originalRuleElements = RuleElements;
            RuleElements = resolvedRules.ToArray();
        }
    }
}