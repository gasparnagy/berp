using System;
using System.Linq;

namespace BerpGherkinParser
{
    public class GherkinKeywords
    {
        private readonly string[] featureKeywords = { "Feature" };
        private readonly string[] backgroundKeywords = { "Background" };
        private readonly string[] scenarioKeywords = { "Scenario" };
        private readonly string[] scenarioOutlineKeywords = { "Scenario Outline", "Scenario Template" };
        private readonly string[] examplesKeywords = { "Examples", "Scenarios" };
        private readonly string[] stepKeywords = { "Given ", "When ", "Then ", "And ", "But ", "* " };
        private readonly string[] givenStepKeywords = { "Given " };
        private readonly string[] whenStepKeywords = { "When " };
        private readonly string[] thenStepKeywords = { "Then " };
        private readonly string[] andStepKeywords = { "And ", "But ", "* " };
        private readonly string[] butStepKeywords = { "But " };

        public string[] FeatureKeywords
        {
            get { return featureKeywords; }
        }

        public string[] BackgroundKeywords
        {
            get { return backgroundKeywords; }
        }

        public string[] ScenarioKeywords
        {
            get { return scenarioKeywords; }
        }

        public string[] ScenarioOutlineKeywords
        {
            get { return scenarioOutlineKeywords; }
        }

        public string[] ExamplesKeywords
        {
            get { return examplesKeywords; }
        }

        public string[] StepKeywords
        {
            get { return stepKeywords; }
        }

        public string[] GivenStepKeywords
        {
            get { return givenStepKeywords; }
        }

        public string[] WhenStepKeywords
        {
            get { return whenStepKeywords; }
        }

        public string[] ThenStepKeywords
        {
            get { return thenStepKeywords; }
        }

        public string[] AndStepKeywords
        {
            get { return andStepKeywords; }
        }

        public string[] ButStepKeywords
        {
            get { return butStepKeywords; }
        }

        public string[] GetTitleKeywords(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Feature:
                    return FeatureKeywords;
                case TokenType.Background:
                    return BackgroundKeywords;
                case TokenType.Scenario:
                    return ScenarioKeywords;
                case TokenType.ScenarioOutline:
                    return ScenarioOutlineKeywords;
                case TokenType.Examples:
                    return ExamplesKeywords;
            }
            throw new NotSupportedException();
        }
    }

    public class TokenScanner
    {
        private readonly ISourceReader reader;
        private readonly GherkinKeywords gherkinKeywords;

        public TokenScanner(ISourceReader reader)
        {
            this.reader = reader;
            gherkinKeywords = new GherkinKeywords();
        }

        public Token Read()
        {
            var line = reader.ReadLine();
            //HACK: while commens are not implemented
            while (line != null && (line.IsEmpty() || line.StartsWith("#")))
                line = reader.ReadLine();

            return new Token(line);
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