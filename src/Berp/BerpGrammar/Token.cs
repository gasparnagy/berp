using System;
using System.Linq;

namespace Berp.BerpGrammar
{
    public class Token
    {
        public Token(TokenType tokenType)
        {
            this.TokenType = tokenType;
        }

        public bool IsEOF { get { return TokenType == TokenType.EOF; } }
        public TokenType TokenType { get; private set; }
        public string Text { get; set; }

        public void Detach()
        {
            //nop;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TokenType, Text);
        }
    }
}