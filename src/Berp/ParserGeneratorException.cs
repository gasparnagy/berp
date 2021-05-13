using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Berp
{
    [Serializable]
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

        protected ParserGeneratorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
