using System;
using System.Linq;

namespace Berp
{
    public class RuleElement
    {
        private readonly string ruleName;
        private readonly Multilicator multilicator;
        private Rule resolvedRule;

        public string RuleName
        {
            get { return ruleName; }
        }

        public Rule ResolvedRule
        {
            get { return resolvedRule; }
            internal set { resolvedRule = value; }
        }

        public Multilicator Multilicator
        {
            get { return multilicator; }
        }

        public RuleElement(TokenType tokenType, Multilicator multilicator = Multilicator.One) : this("#" + tokenType, multilicator)
        {
        }

        public RuleElement(string ruleName, Multilicator multilicator = Multilicator.One)
        {
            this.ruleName = ruleName;
            this.multilicator = multilicator;
        }

        public void Resolve(RuleSet ruleSet)
        {
            resolvedRule = ruleSet.Resolve(ruleName);
            if (resolvedRule == null)
            {
                throw new Exception("Unable to resolve rule: " + ruleName);
            }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool embedNonProductionRules)
        {
            string postFix = "";
            switch (multilicator)
            {
                case Multilicator.OneOrZero:
                    postFix = "?";
                    break;
                case Multilicator.Any:
                    postFix = "*";
                    break;
                case Multilicator.OneOrMore:
                    postFix = "+";
                    break;
                case Multilicator.One:
                    postFix = "";
                    break;
            }

            if (embedNonProductionRules && resolvedRule != null && resolvedRule.TempRule)
            {
                if ((multilicator == Multilicator.One && resolvedRule is SequenceRule && ((SequenceRule)resolvedRule).RuleElements.Length == 1) || resolvedRule is AlternateRule)
                    return string.Format("{0}{1}", resolvedRule.GetRuleDescription(true), postFix);

                return string.Format("({0}){1}", resolvedRule.GetRuleDescription(true), postFix);
            }

            return ruleName + postFix;
        }
    }
}