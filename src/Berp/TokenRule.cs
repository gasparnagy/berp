using System;
using System.Linq;

namespace Berp
{
    class TokenRule : Rule
    {
        private readonly TokenType tokenType;

        public TokenType TokenType
        {
            get { return tokenType; }
        }

        public TokenRule(TokenType tokenType)
        {
            this.tokenType = tokenType;
        }

        public override string Name
        {
            get { return "#" + tokenType; }
        }

        public override string GetRuleDescription(bool embedNonProductionRules)
        {
            return Name;
        }

        public override string ToString(bool embedNonProductionRules)
        {
            return Name;
        }

        public override void Resolve(RuleSet ruleSet)
        {
            //nop;
        }
    }
}