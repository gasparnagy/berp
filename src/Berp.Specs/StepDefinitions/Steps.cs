using Berp.Specs.Support;
using System;
using System.IO;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace Berp.Specs.StepDefinitions
{
    [Binding]
    public class Steps
    {
        private string sourceContent;
        private Exception parsingError = null;
        private object ast;

        [Given(@"the input source from '(.*)'")]
        public void GivenTheInputSourceFrom(string fileName)
        {
            var fullPath = TestFolders.GetInputFilePath(fileName);
            sourceContent = File.ReadAllText(fullPath);
        }

        [Given(@"the input source")]
        public void GivenTheInputSource(string content)
        {
            sourceContent = content;
        }


        [When(@"the input source is parsed with the BerpGrammarParserForTest parser")]
        public void WhenTheInputSourceIsParsedWithTheBerpGrammarParserForTestParser()
        {
            var parser = new BerpGrammar.Parser();
            try
            {
                ast = parser.Parse(new BerpGrammar.TokenScanner(new StringReader(sourceContent)));
                Console.WriteLine(ast);
            }
            catch (Exception ex)
            {
                parsingError = ex;
            }
        }

        [Then(@"the parsing should be successful")]
        public void ThenTheParsingShouldBeSuccessful()
        {
            parsingError.Should().BeNull("A parsing error occured.");
        }

        [Then(@"the created AST should be")]
        public void ThenTheCreatedASTShouldBe(string expectedAstText)
        {
            ast.Should().NotBeNull();
            string astText = ast.ToString();
            Normalize(astText).Should().Be(Normalize(expectedAstText));
        }

        private object Normalize(string text)
        {
            return text.Trim().Replace(" ", "").Replace("\t", "");
        }
    }
}
