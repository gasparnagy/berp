using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp
{
    public class StartRule : SequenceRule
    {
        public StartRule(string name, params RuleElement[] ruleElements) : base(name, ruleElements)
        {
        }

        public override void Resolve(RuleSet ruleSet)
        {
            base.Resolve(ruleSet);

            var resolvedRules = new List<RuleElement>(RuleElements);
            var eofElement = new RuleElement(TokenType.EOF);
            eofElement.Resolve(ruleSet);
            resolvedRules.Add(eofElement);
            RuleElements = resolvedRules.ToArray();
        }

//        public override string ToString(bool embedNonProductionRules)
//        {
//            return "=" + base.ToString(embedNonProductionRules);
//        }
    }
}