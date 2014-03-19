using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
