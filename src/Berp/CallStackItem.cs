using System;
using System.Linq;
using System.Text;

namespace Berp
{
    class CallStackItem
    {
        private readonly CallStackItem parent;
        private readonly Rule rule;
        private readonly int position;

        public CallStackItem Parent
        {
            get { return parent; }
        }

        public CallStackItem(CallStackItem parent, Rule rule, int position = 0)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            this.parent = parent;
            this.rule = rule;
            this.position = position;
        }

        public Rule Rule
        {
            get { return rule; }
        }

        public int Position
        {
            get { return position; }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            if (parent != null)
                result.AppendFormat("{0}>", parent);
            result.AppendFormat("{0}:{1}", rule.Name, position);
            return result.ToString();
        }

        protected bool Equals(CallStackItem other)
        {
            return Equals(parent, other.parent) && Equals(rule, other.rule) && position == other.position;
        }

        public static bool PositionAgnosticEquals(CallStackItem x, CallStackItem y)
        {
            return ((x.parent == null && y.parent == null) ||
                    (x.parent != null && y.parent != null && PositionAgnosticEquals(x.parent, y.parent))) &&
                    Equals(x.rule, y.rule);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CallStackItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (parent != null ? parent.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (rule != null ? rule.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ position;
                return hashCode;
            }
        }
    }
}