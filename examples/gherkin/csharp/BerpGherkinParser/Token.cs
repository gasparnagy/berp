using System;
using System.Linq;

namespace BerpGherkinParser
{
    public class Token
    {
        public bool IsEOF { get { return Line == null; } }
        public ISourceLine Line { get; set; }
        public TokenType MatchedType { get; set; }
        public string MatchedKeyword { get; set; }
        public string Text { get; set; }
        public string[] Items { get; set; }
        public int Indent { get; set; }

        public Token(ISourceLine line)
        {
            Line = line;
        }

        public void Detach()
        {
            Line.Detach();
        }
    }
}