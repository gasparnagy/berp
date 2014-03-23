using System;
using System.Linq;

namespace BerpGherkinParser
{
    public class TokenScanner
    {
        private readonly ISourceReader reader;

        public TokenScanner(ISourceReader reader)
        {
            this.reader = reader;
        }

        public Token Read()
        {
            var line = reader.ReadLine();
            return new Token(line);
        }
    }
}