using System;
using System.Linq;

namespace BerpGherkinParser
{
    public class TokenMatcher
    {
        private readonly GherkinKeywords gherkinKeywords;

        public TokenMatcher()
        {
            gherkinKeywords = new GherkinKeywords();
        }

        public bool Match_Empty(Token token)
        {
            if (token.Line.IsEmpty())
            {
                token.MatchedType = TokenType.Empty;
                return true;
            }
            return false;
        }

        public bool Match_Background(Token token)
        {
            return MatchTitleLine(token, TokenType.Background);
        }

        private bool MatchTitleLine(Token token, TokenType tokenType)
        {
            var keywords = gherkinKeywords.GetTitleKeywords(tokenType);
            foreach (var keyword in keywords)
            {
                if (token.Line.StartsWith(keyword, ':'))
                {
                    token.MatchedType = tokenType;
                    token.MatchedKeyword = keyword;
                    token.Text = token.Line.GetRestTrimmed(keyword.Length + 1);
                    return true;
                }
            }
            return false;
        }

        public bool Match_TagLine(Token token)
        {
            if (token.Line.StartsWith("@"))
            {
                token.MatchedType = TokenType.TagLine;
                token.Items = token.Line.Split(1).ToArray();

                return true;
            }
            return false;
        }

        public bool Match_Scenario(Token token)
        {
            return MatchTitleLine(token, TokenType.Scenario);
        }

        public bool Match_ScenarioOutline(Token token)
        {
            return MatchTitleLine(token, TokenType.ScenarioOutline);
        }

        public bool Match_EOF(Token token)
        {
            if (token.IsEOF)
            {
                token.MatchedType = TokenType.EOF;
                return true;
            }
            return false;
        }

        public bool Match_Other(Token token)
        {
            token.MatchedType = TokenType.Other;
            token.Text = token.Line.GetLineText();
            return true;
        }

        public bool Match_Examples(Token token)
        {
            return MatchTitleLine(token, TokenType.Examples);
        }

        public bool Match_Feature(Token token)
        {
            return MatchTitleLine(token, TokenType.Feature);
        }

        public bool Match_MultiLineArgument(Token token)
        {
            if (token.Line.StartsWith("\"\"\"")) //TODO: equals
            {
                token.MatchedType = TokenType.MultiLineArgument;
                token.Indent = token.Line.Indent;
                return true;
            }
            return false;
        }

        public bool Match_Step(Token token)
        {
            var keywords = gherkinKeywords.StepKeywords;
            foreach (var keyword in keywords)
            {
                if (token.Line.StartsWith(keyword))
                {
                    token.MatchedType = TokenType.Step;
                    token.MatchedKeyword = keyword;
                    token.Text = token.Line.GetRestTrimmed(keyword.Length);
                    return true;
                }
            }
            return false;
        }

        public bool Match_TableRow(Token token)
        {
            if (token.Line.StartsWith("|"))
            {
                token.MatchedType = TokenType.TableRow;
                token.Items = token.Line.GetTableCells().ToArray();
                return true;
            }
            return false;
        }

        public bool Match_Comment(Token token)
        {
            if (token.Line.StartsWith("#"))
            {
                token.MatchedType = TokenType.TableRow;
                token.Text = token.Line.GetLineText();
                return true;
            }
            return false;
        }
    }
}