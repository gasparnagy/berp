using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Berp.BerpGrammar
{
    public class TokenScanner : TokenMatcher
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
            while ((line = textReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Trim().StartsWith("//"))
                    continue; //TODO: comment

                var parts = tokenRe.Match(line).Groups["token"].Captures.OfType<Capture>().Select(c => c.Value);
                foreach (var part in parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()))
                {
                    switch (part)
                    {
                        case ",":
                            yield return new Token(TokenType.Comma) { Text = part };
                            break;
                        case ":=":
                            yield return new Token(TokenType.Definition) { Text = part };
                            break;
                        case "(":
                            yield return new Token(TokenType.LParen) { Text = part };
                            break;
                        case ")":
                            yield return new Token(TokenType.RParen) { Text = part };
                            break;
                        case "|":
                            yield return new Token(TokenType.AlternateOp) { Text = part };
                            break;
                        case "*":
                            yield return new Token(TokenType.AnyMultiplier) { Text = part };
                            break;
                        case "+":
                            yield return new Token(TokenType.OneOrMoreMultiplier) { Text = part };
                            break;
                        case "?":
                            yield return new Token(TokenType.OneOrZeroMultiplier) { Text = part };
                            break;
                        case "!":
                            yield return new Token(TokenType.Production) { Text = part };
                            break;
                        case "->":
                            yield return new Token(TokenType.Arrow) { Text = part };
                            break;
                        case "[":
                            yield return new Token(TokenType.LBracket) { Text = part };
                            break;
                        case "]":
                            yield return new Token(TokenType.RBracket) { Text = part };
                            break;
                        default:
                            if (part.StartsWith("#"))
                                yield return new Token(TokenType.Token) { Text = part/*.Substring(1)*/ };
                            else if (char.IsDigit(part[0]))
                                yield return new Token(TokenType.Number) { Text = part };
                            else if (char.IsLetter(part[0]))
                                yield return new Token(TokenType.Rule) { Text = part };
                            else
                            {
                                throw new Exception("Invalid token: " + part);
                            }
                            break;
                    }
                }

                yield return new Token(TokenType.EOL);
            }
        }

        public Token Read()
        {
            if (!tokenEnumerator.MoveNext())
                return new Token(TokenType.EOF);

//            Console.WriteLine("{0}: {1}", tokenEnumerator.Current.TokenType, tokenEnumerator.Current.Text);
            return tokenEnumerator.Current;
        }
    }
}