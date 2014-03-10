using System;
using System.Linq;

namespace Berp
{
    public class TokenType : IComparable<TokenType>
    {
        public static readonly TokenType EOF = new TokenType("EOF");
        public static readonly TokenType Other = new TokenType("Other");

        public string Name { get; private set; }

        public TokenType(string name)
        {
            Name = name;
        }

        public int CompareTo(TokenType other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Name;
        }

        protected bool Equals(TokenType other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TokenType) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}