using System;
using System.Linq;
using System.Text;

namespace Berp
{
    public abstract class Rule
    {
        private bool tempRule = false;
        private bool allowProductionRules = true;
        public abstract string Name { get; }
        public LookAheadHint LookAheadHint { get; set; }

        public bool AllowProductionRules
        {
            get { return allowProductionRules; }
        }

        public bool TempRule
        {
            get { return tempRule; }
        }

        public Rule IgnoreProduction()
        {
            allowProductionRules = false;
            return this;
        }

        public Rule Temporary()
        {
            IgnoreProduction();
            tempRule = true;
            return this;
        }

        public abstract string GetRuleDescription(bool embedNonProductionRules);

        public override string ToString()
        {
            return ToString(false);
        }

        public virtual string ToString(bool embedNonProductionRules)
        {
            var result = new StringBuilder(Name);
            if (AllowProductionRules)
                result.Append("!");
            if (LookAheadHint != null)
                result.AppendFormat(" [{0}->{1}]", string.Join("|", LookAheadHint.Skip.Select(t => "#" + t.Name)), string.Join("|", LookAheadHint.ExpectedTokens.Select(t => "#" + t.Name)));

            result.Append(" := ");
            result.Append(GetRuleDescription(embedNonProductionRules));
            return result.ToString();
        }

        public abstract void Resolve(RuleSet ruleSet);
    }
}