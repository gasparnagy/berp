using System;
using System.Linq;

namespace Berp.BerpGrammar
{
    public class Token
    {
        public Token(TokenType tokenType, int lineNumber, int linePosition)
        {
            this.TokenType = tokenType;
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

        public bool IsEOF { get { return TokenType == TokenType.EOF; } }
        public TokenType TokenType { get; private set; }
        public string Text { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }

        public void Detach()
        {
            //nop;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Text))
                return string.Format("#{0}", TokenType);
            return string.Format("#{0}:'{1}'", TokenType, Text);
        }
    }
}