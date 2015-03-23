using Berp.Specs.BerpGrammarParserForTest;
using Berp.Specs.Support;
using System;
using System.Linq;
using System.IO;
using TechTalk.SpecFlow;
using FluentAssertions;
using Berp.BerpGrammar;

namespace Berp.Specs.StepDefinitions
{
    [Binding]
    public class Steps
    {
        private string sourceContent;
        private bool stopAtFirstError = false;
        private CompositeParserException parsingError = null;
        private RuleSet ast;

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

        [Given(@"the parser is set to stop at first error")]
        public void GivenTheParserIsSetToStopAtFirstError()
        {
            stopAtFirstError = true;
        }

        [When(@"the input source is parsed with the BerpGrammarParserForTest parser")]
        public void WhenTheInputSourceIsParsedWithTheBerpGrammarParserForTestParser()
        {
            var parser = new BerpGrammar.Parser();
            parser.StopAtFirstError = stopAtFirstError;
            try
            {
                ast = parser.Parse(new BerpGrammar.TokenScanner(new StringReader(sourceContent)), new TokenMatcher(), new AstBuilderForTest());
                Console.WriteLine(ast);
            }
            catch (CompositeParserException parserEx)
            {
                parsingError = parserEx;
                Console.WriteLine(parsingError.GetErrorMessage());
            }
            catch (ParserException ex)
            {
                parsingError = new CompositeParserException(new []{ ex });
                Console.WriteLine(parsingError.GetErrorMessage());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("unhandled parsing error", ex);
            }
        }

        [Then(@"the parsing should be successful")]
        public void ThenTheParsingShouldBeSuccessful()
        {
            parsingError.Should().BeNull("A parsing error occured.");
        }

        [Then(@"the parsing should fail")]
        public void ThenTheParsingShouldFail()
        {
            parsingError.Should().NotBeNull("No parsing error received.");
        }

        [Then(@"the created AST should be")]
        public void ThenTheCreatedASTShouldBe(string expectedAstText)
        {
            ast.Should().NotBeNull();
            string astText = ast.ToString();
            TestHelpers.NormalizeText(astText).Should().Be(TestHelpers.NormalizeText(expectedAstText));
        }

        [Then(@"there should be (.*) parsing errors")]
        public void ThenThereShouldBeParsingErrors(int expectedErrorCount)
        {
            stopAtFirstError.Should().BeFalse();
            parsingError.Errors.Should().HaveCount(expectedErrorCount);
        }

        private UnexpectedTokenException GetFirstError()
        {
            stopAtFirstError.Should().BeTrue();
            parsingError.Errors.Should().HaveCount(1);
            var error = parsingError.Errors.First();
            if (error is UnexpectedEOFException)
            {
                var eofEx = (UnexpectedEOFException) error;
                error = new UnexpectedTokenException(new Token(BerpGrammar.TokenType.EOF, 0, 0), eofEx.ExpectedTokenTypes, eofEx.StateComment);
            }
            return error as UnexpectedTokenException;
        }

        [Then(@"the error should contain the expected tokens")]
        public void ThenTheErrorShouldContain(Table expectedTokenTypesTable)
        {
            var error = GetFirstError();

            var expectedTokenTypes = expectedTokenTypesTable.Rows.Select(r => r[0]).ToArray();
            error.ExpectedTokenTypes.Should().BeEquivalentTo(expectedTokenTypes, "there should be an error with the expected tokens (got: {0})", parsingError.GetErrorMessage());
        }

        [Then(@"the error should contain the received token (.*)")]
        public void ThenTheErrorShouldContainTheReceivedToken(string expectedToken)
        {
            var error = GetFirstError();

            error.ReceivedToken.ToString().Should().StartWith(expectedToken, "there should be an error with the received token (got: {0})", parsingError.GetErrorMessage());
        }

        [Then(@"the error should contain the line number (.*)")]
        public void ThenTheErrorShouldContainTheLineNumber(int expectedLineNumber)
        {
            var error = GetFirstError();

            error.Location.Line.Should().Be(expectedLineNumber, "there should be an error with the line number (got: {0})", parsingError.GetErrorMessage());
        }

        [Then(@"the error should contain the line position number (.*)")]
        public void ThenTheErrorShouldContainTheLinePositionNumber(int expectedLinePositionNumber)
        {
            var error = GetFirstError();

            error.Location.Column.Should().Be(expectedLinePositionNumber, "there should be an error with the line position number (got: {0})", parsingError.GetErrorMessage());
        }

        [Then(@"the error should be an unexpected end of file error")]
        public void ThenTheErrorShouldBeAnUnexpectedEndOfFileError()
        {
            ThenTheErrorShouldContainTheReceivedToken("#EOF");
        }
    }
}
