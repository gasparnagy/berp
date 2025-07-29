using System;
using System.IO;
using Berp.Specs.Support;
using AwesomeAssertions;
using Reqnroll;
using Xunit.Abstractions;

namespace Berp.Specs.StepDefinitions;

[Binding]
public class GenerationStepDefinitions(ITestOutputHelper testOutputHelper)
{
    private string grammarDefinition;
    private string outputFile;
    private Exception generationError;

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
        var ruleSet = parser.Parse(new BerpGrammar.TokenScanner(new StringReader(grammarDefinition)));
        var states = StateCalculator.CalculateStates(ruleSet);

        try
        {
            var generator = new Generator(ruleSet.Settings);
            outputFile = TestFolders.GetTempFilePath("output.txt");
            generator.Generate(Path.Combine("GeneratorTemplates", templateName), ruleSet, states, outputFile);
            testOutputHelper.WriteLine($"Result saved to: {outputFile}");
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