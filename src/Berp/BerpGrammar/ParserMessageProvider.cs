using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berp.BerpGrammar
{
    partial class ParserError
    {
        public int? LineNumber { get { return ReceivedToken.LineNumber; } }
        public int? LinePosition { get { return ReceivedToken.LinePosition; } }
    }

    public static class ParserMessageProvider
    {
        static public string GetDefaultExceptionMessage(ParserError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return "Parser error";

            return "Parser errors: " + Environment.NewLine + string.Join(Environment.NewLine, errors.Select(e => GetParserErrorMessage(e)));
        }

        static public string GetParserErrorMessage(ParserError error)
        {
            if (error.ReceivedToken.TokenType == TokenType.EOF)
                return string.Format("Error at line {1}: unexpected end of file, expected: {0}", string.Join(", ", error.ExpectedTokenTypes), error.LineNumber);

            return string.Format("Error at line {2} position {3}: expected: {0}, got {1}", string.Join(", ", error.ExpectedTokenTypes), error.ReceivedToken, error.LineNumber, error.LinePosition);
        }
    }
}
