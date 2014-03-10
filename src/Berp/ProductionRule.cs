using System;
using System.Linq;

namespace Berp
{
    public enum ProductionRuleType
    {
        Start,
        End,
        Process
    }

    public class ProductionRule
    {
        public ProductionRuleType Type { get; private set; }
        internal Rule Rule { get; private set; }
        public string RuleName { get { return Rule == null ? "#TOKEN" : Rule.Name; } }
        public bool IsTokenProduction { get { return Rule == null; } }

        internal ProductionRule(ProductionRuleType type, Rule rule)
        {
            Type = type;
            Rule = rule;
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Type.ToString().Substring(0, 1), Rule == null ? "#TOKEN" : Rule.Name);
        }

        protected bool Equals(ProductionRule other)
        {
            return Type == other.Type && Equals(Rule, other.Rule);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProductionRule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Type*397) ^ (Rule != null ? Rule.GetHashCode() : 0);
            }
        }
    }
}