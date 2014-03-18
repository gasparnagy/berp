using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Berp.BerpGrammar
{
    public class TokenScanner
    {
        private readonly TextReader textReader;
        private readonly IEnumerator<Token> tokenEnumerator;

        public TokenScanner(TextReader textReader)
        {
            this.textReader = textReader;
            tokenEnumerator = GetTokenEnumerator();
        }

        private IEnumerator<Token> GetTokenEnumerator()
        {
            var tokenRe = new Regex(@"^(?<token>\:=|\-\>|\,|\[|\]|\(|\)|\*|\+|\?|\!|#\w+|\w[\w\.]*|\d+|\s|.)*$");
            string line;
            int lineNumber = 0;
            int linePosition = 0;
            string tokenText = null;
            Func<TokenType, Token> tokenFactory = tokenType => new Token(tokenType, lineNumber, linePosition) { Text = tokenText };
            while ((line = textReader.ReadLine()) != null)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Trim().StartsWith("//"))
                    continue; //TODO: comment

                var parts = tokenRe.Match(line).Groups["token"].Captures.OfType<Capture>();
                foreach (var part in parts.Where(p => !string.IsNullOrWhiteSpace(p.Value)))
                {
                    tokenText = part.Value.Trim();
                    linePosition = part.Index + 1;
                    switch (tokenText)
                    {
                        case ",":
                            yield return tokenFactory(TokenType.Comma);
                            break;
                        case ":=":
                            yield return tokenFactory(TokenType.Definition);
                            break;
                        case "(":
                            yield return tokenFactory(TokenType.LParen);
                            break;
                        case ")":
                            yield return tokenFactory(TokenType.RParen);
                            break;
                        case "|":
                            yield return tokenFactory(TokenType.AlternateOp);
                            break;
                        case "*":
                            yield return tokenFactory(TokenType.AnyMultiplier);
                            break;
                        case "+":
                            yield return tokenFactory(TokenType.OneOrMoreMultiplier);
                            break;
                        case "?":
                            yield return tokenFactory(TokenType.OneOrZeroMultiplier);
                            break;
                        case "!":
                            yield return tokenFactory(TokenType.Production);
                            break;
                        case "->":
                            yield return tokenFactory(TokenType.Arrow);
                            break;
                        case "[":
                            yield return tokenFactory(TokenType.LBracket);
                            break;
                        case "]":
                            yield return tokenFactory(TokenType.RBracket);
                            break;
                        default:
                            if (tokenText.StartsWith("#"))
                                yield return tokenFactory(TokenType.Token);
                            else if (char.IsDigit(tokenText[0]))
                                yield return tokenFactory(TokenType.Number);
                            else if (char.IsLetter(tokenText[0]))
                                yield return tokenFactory(TokenType.Rule);
                            else
                            {
                                throw new Exception("Invalid token: " + tokenText);
                            }
                            break;
                    }
                }

                tokenText = null;
                yield return tokenFactory(TokenType.EOL);
            }

            linePosition = 1;
            tokenText = null;
            yield return tokenFactory(TokenType.EOF);
        }

        public Token Read()
        {
            if (!tokenEnumerator.MoveNext())
                throw new InvalidOperationException("Reader closed");

            return tokenEnumerator.Current;
        }
    }
}