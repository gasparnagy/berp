using System;
using System.Linq;

namespace Berp.BerpGrammar
{
    public class Token
    {
        public Token(TokenType tokenType, int lineNumber, int linePosition)
        {
            TokenType = tokenType;
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        public bool IsEOF { get { return TokenType == TokenType.EOF; } }
        public TokenType TokenType { get; private set; }
        public string Text { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }
        public Location Location { get {return new Location(LineNumber, LinePosition);} }

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