using System;
using System.IO;
using Berp.BerpGrammar;
using Berp.Specs.Support;
using FluentAssertions;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace Berp.Specs.StepDefinitions
{
    [Binding]
    public class GenerationStepDefinitions
    {
        private string grammarDefinition;
        private string outputFile;
        private Exception generationError;
        private readonly ITestOutputHelper _testOutputHelper;

        public GenerationStepDefinitions(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Given("there is a complex grammar")]
        public void GivenThereIsAComplexGrammar()
        {
            var fullPath = TestFolders.GetInputFilePath(@"BerpGrammarParserForTest\BerpGrammar.berp");
            grammarDefinition = File.ReadAllText(fullPath);
        }

        [When("the parser generation is performed using {string}")]
        public void WhenTheParserGenerationIsPerformedUsing(string templateName)
        {
            var parser = new BerpGrammar.Parser();
            var ruleSet = parser.Parse(new TokenScanner(new StringReader(grammarDefinition)));
            var states = StateCalculator.CalculateStates(ruleSet);

            try
            {
                var generator = new Generator(ruleSet.Settings);
                outputFile = TestFolders.GetTempFilePath("output.txt");
                generator.Generate(Path.Combine("GeneratorTemplates", templateName), ruleSet, states, outputFile);
                _testOutputHelper.WriteLine($"Result saved to: {outputFile}");
            }
            catch (Exception ex)
            {
                generationError = ex;
            }
        }

        [Then("then the generation should be successful")]
        public void ThenThenTheGenerationShouldBeSuccessful()
        {
            generationError.Should().BeNull();
            File.Exists(outputFile).Should().BeTrue();
        }
    }
}
