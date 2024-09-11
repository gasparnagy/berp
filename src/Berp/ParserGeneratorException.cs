using System;

namespace Berp
{
    public class ParserGeneratorException : Exception
    {
        public ParserGeneratorException()
        {
        }

        public ParserGeneratorException(string message) : base(message)
        {
        }

        public ParserGeneratorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
