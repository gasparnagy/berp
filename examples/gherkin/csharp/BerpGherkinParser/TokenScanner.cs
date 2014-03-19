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
            //HACK: while commens are not implemented
            while (line != null && (line.IsEmpty() || line.StartsWith("#")))
                line = reader.ReadLine();

            return new Token(line);
        }
    }
}