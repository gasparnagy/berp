using System;
using System.Linq;

namespace Berp
{
    public abstract class DerivedRule : Rule
    {
        private readonly string name;
        public RuleElement[] RuleElements { get; protected set; }

        protected DerivedRule(string name, params RuleElement[] ruleElements)
        {
            this.name = name;
            this.RuleElements = ruleElements;
        }

        public override string Name
        {
            get { return name; }
        }

        public override void Resolve(RuleSet ruleSet)
        {
            if (LookAheadHint != null && 
                LookAheadHint.ExpectedTokens.Length == 0)
            {
                LookAheadHint = new LookAheadHint(ruleSet.OfType<TokenRule>().Select(tr => tr.TokenType).ToArray(), new TokenType[0]);
            }
            foreach (var ruleElement in RuleElements)
            {
                ruleElement.Resolve(ruleSet);
            }
        }
    }
}