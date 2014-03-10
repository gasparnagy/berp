using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp
{
    class Branch
    {
        class BranchProductionComparer : IEqualityComparer<Branch>
        {
            public bool Equals(Branch x, Branch y)
            {
                return x.TokenType.Equals(y.TokenType)
                    && x.OptimizedProductions.SequenceEqual(y.OptimizedProductions)
                    && CallStackItem.PositionAgnosticEquals(x.CallStackItem, y.CallStackItem)
                    ;
            }

            public int GetHashCode(Branch obj)
            {
                return obj.TokenType.GetHashCode();
            }
        }
        public static readonly IEqualityComparer<Branch> ProductionComparer = new BranchProductionComparer();


        public TokenType TokenType { get; private set; }
        public CallStackItem CallStackItem { get; private set; }

        public List<ProductionRule> Productions { get; private set; }
        public List<ProductionRule> OptimizedProductions { get; set; }

        public LookAheadHint LookAheadHint { get; set; }

        public Branch(TokenType tokenType, CallStackItem callStackItem, List<ProductionRule> productions)
        {
            TokenType = tokenType;
            CallStackItem = callStackItem;
            Productions = productions;
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", TokenType, CallStackItem);
        }

        public string GetProductionsText()
        {
            return string.Format("[{0}]", string.Join(",", OptimizedProductions ?? Productions));
        }

        protected bool Equals(Branch other)
        {
            return TokenType.Equals(other.TokenType)
                && CallStackItem.Equals(other.CallStackItem)
                && OptimizedProductions.SequenceEqual(other.OptimizedProductions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Branch) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TokenType.GetHashCode()*397);
            }
        }
    }
}