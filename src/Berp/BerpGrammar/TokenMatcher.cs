using System;
using System.Linq;

namespace Berp.BerpGrammar
{
    public partial class TokenMatcher : ITokenMatcher
    {
        public override bool Match_Number(Token token)
        {
            throw new NotSupportedException("Number token");
        }

        public override bool Match_Other(Token token)
        {
            throw new NotSupportedException("Other token");
        }

        public override bool Match_EOF(Token token)
        {
            return token.IsEOF;
        }

    }
}