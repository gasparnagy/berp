using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berp.BerpGrammar
{
    public class Parser : Parser<RuleSet>
    {
        public Parser()
        {
        }

        public Parser(IAstBuilder<RuleSet> astBuilder) : base(astBuilder)
        {
        }
    }
}
