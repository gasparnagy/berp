using System;
using System.Linq;

namespace Berp
{
    public abstract class Rule
    {
        private bool tempRule = false;
        private bool allowProductionRules = true;
        public abstract string Name { get; }

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
            if (AllowProductionRules)
                return string.Format("{0}! := {1}", Name, GetRuleDescription(embedNonProductionRules));
            return string.Format("{0} := {1}", Name, GetRuleDescription(embedNonProductionRules));
        }

        public abstract void Resolve(RuleSet ruleSet);
    }
}