using Berp.Specs.BerpGrammarParserForTest;
using Berp.Specs.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Reqnroll;
using AwesomeAssertions;
using Berp.BerpGrammar;
using Xunit.Abstractions;

namespace Berp.Specs.StepDefinitions;

[Binding]
public class StepDefinitions(ITestOutputHelper testOutputHelper)
{
    private string sourceContent;
    private bool stopAtFirstError = false;
    private CompositeParserException parsingError = null;
    private RuleSet ruleSet;

    [Given("the input source from {string}")]
    public void GivenTheInputSourceFrom(string fileName)
    {
        var fullPath = TestFolders.GetInputFilePath(fileName);
        sourceContent = File.ReadAllText(fullPath);
    }

    [Given("the input source")]
    public void GivenTheInputSource(string content)
    {
        sourceContent = content;
    }

    [Given("the parser is set to stop at first error")]
    public void GivenTheParserIsSetToStopAtFirstError()
    {
        stopAtFirstError = true;
    }

    private void ParseWithErrorHandling(Action<Parser> doParsing, IAstBuilder<RuleSet> astBuilder = null)
    {
        var parser = astBuilder == null ? new Parser() : new Parser(astBuilder);
        parser.StopAtFirstError = stopAtFirstError;
        try
        {
            doParsing(parser);
        }
        catch (CompositeParserException parserEx)
        {
            parsingError = parserEx;
            Console.WriteLine(parsingError.GetErrorMessage());
        }
        catch (ParserException ex)
        {
            parsingError = new CompositeParserException([ex]);
            Console.WriteLine(parsingError.GetErrorMessage());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("unhandled parsing error", ex);
        }
    }

    [When("the input source is parsed with the BerpGrammarParserForTest parser")]
    public void WhenTheInputSourceIsParsedWithTheBerpGrammarParserForTestParser()
    {
        ParseWithErrorHandling(parser =>
        {
            ruleSet = parser.Parse(new TokenScanner(new StringReader(sourceContent)),
                new TokenMatcher());
            testOutputHelper.WriteLine(ruleSet.ToString());
        }, new AstBuilderForTest());
    }

    [When("the input source is compiled with the BerpGrammarParserForTest parser")]
    public void WhenTheInputSourceIsCompiledWithTheBerpGrammarParserForTestParser()
    {
        ParseWithErrorHandling(parser =>
        {
            ruleSet = parser.Parse(new TokenScanner(new StringReader(sourceContent)));
            testOutputHelper.WriteLine(ruleSet.ToString());

            var states = StateCalculator.CalculateStates(ruleSet);
            testOutputHelper.WriteLine("{0} states calculated for the parser.", states.Count);
            foreach (var state in states.Values)
            {
                PrintStateTransitions(state);
                PrintStateBranches(state.Branches, state.Id);
            }
        });
    }

    private void PrintStateTransitions(State state)
    {
        testOutputHelper.WriteLine("{0}: {1}", state.Id, state.Comment);
        foreach (var transition in state.Transitions)
        {
            testOutputHelper.WriteLine("    {0} -> {1}", transition.TokenType, transition.TargetState);
        }
    }

    private void PrintStateBranches(List<Branch> branches, int state)
    {
        testOutputHelper.WriteLine("{0}:", state);
        testOutputHelper.WriteLine("");
        foreach (var branch in branches.OrderBy(b => b.TokenType))
        {
            testOutputHelper.WriteLine("    {0}", branch);
            testOutputHelper.WriteLine("    \t{0}", branch.GetProductionsText());
        }
    }

    [Then("the parsing should be successful")]
    public void ThenTheParsingShouldBeSuccessful()
    {
        parsingError.Should().BeNull("A parsing error occured.");
    }

    [Then("the parsing should fail")]
    public void ThenTheParsingShouldFail()
    {
        parsingError.Should().NotBeNull("No parsing error received.");
    }

    [Then("the created AST should be")]
    public void ThenTheCreatedAstShouldBe(string expectedAstText)
    {
        ruleSet.Should().NotBeNull();
        string astText = ruleSet.ToString();
        TestHelpers.NormalizeText(astText).Should().Be(TestHelpers.NormalizeText(expectedAstText));
    }

    [Then("there should be {int} parsing errors")]
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
        if (error is UnexpectedEOFException eofEx)
        {
            error = new UnexpectedTokenException(new Token(BerpGrammar.TokenType.EOF, 0, 0), eofEx.ExpectedTokenTypes, eofEx.StateComment);
        }
        return error as UnexpectedTokenException;
    }

    [Then("the error should contain the expected tokens")]
    public void ThenTheErrorShouldContain(Table expectedTokenTypesTable)
    {
        var error = GetFirstError();

        var expectedTokenTypes = expectedTokenTypesTable.Rows.Select(r => r[0]).ToArray();
        error.ExpectedTokenTypes.Should().BeEquivalentTo(expectedTokenTypes, "there should be an error with the expected tokens (got: {0})", parsingError.GetErrorMessage());
    }

    [Then("the error should contain the received token {word}")]
    public void ThenTheErrorShouldContainTheReceivedToken(string expectedToken)
    {
        var error = GetFirstError();

        error.ReceivedToken.ToString().Should().StartWith(expectedToken, "there should be an error with the received token (got: {0})", parsingError.GetErrorMessage());
    }

    [Then("the error should contain the line number {int}")]
    public void ThenTheErrorShouldContainTheLineNumber(int expectedLineNumber)
    {
        var error = GetFirstError();

        error.Location.Line.Should().Be(expectedLineNumber, "there should be an error with the line number (got: {0})", parsingError.GetErrorMessage());
    }

    [Then("the error should contain the line position number {int}")]
    public void ThenTheErrorShouldContainTheLinePositionNumber(int expectedLinePositionNumber)
    {
        var error = GetFirstError();

        error.Location.Column.Should().Be(expectedLinePositionNumber, "there should be an error with the line position number (got: {0})", parsingError.GetErrorMessage());
    }

    [Then("the error should be an unexpected end of file error")]
    public void ThenTheErrorShouldBeAnUnexpectedEndOfFileError()
    {
        ThenTheErrorShouldContainTheReceivedToken("#EOF");
    }
}