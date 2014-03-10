using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow.Parser.Gherkin;
using TechTalk.SpecFlow.Parser.SyntaxElements;

namespace BerpGherkinParser
{
    public class DomBuilder
    {
        public IEnumerable<object> BuildFromToken(Token token)
        {
            switch (token.MatchedType)
            {
                case TokenType.TagLine:
                    foreach (var item in token.Items)
                    {
                        yield return new Tag(item);
                    }
                    break;
                case TokenType.TableRow:
                    yield return new GherkinTableRow(token.Items.Select(cv => new GherkinTableCell(cv)).ToArray());
                    break;
                default:
                    yield return token;
                    break;
            }
        }

        public object BuildFromNode(ASTNode astNode)
        {
            switch (astNode.Node)
            {
                case RuleType.Description:
                    return string.Join(Environment.NewLine, astNode.GetAllSubNodes().Cast<Token>().Select(t => t.Text));
                case RuleType.Multiline_Arg:
                {
                    int indent = astNode.GetSubNodesOf(RuleType._MultiLineArgument).Cast<Token>().First().Indent; //TODO: use indent
                    return string.Join(Environment.NewLine, astNode.GetAllSubNodes().Cast<Token>().Where(t => t.MatchedType != TokenType.MultiLineArgument).Select(t => t.Text)); //TODO: indent
                }
                case RuleType.Table_Arg:
                case RuleType.Examples_Table:
                {
                    var rows = astNode.GetSubNodesOf(RuleType._TableRow).Cast<GherkinTableRow>().ToArray();
                    var header = rows.First();
                    return new GherkinTable(header, rows.Skip(1).ToArray());
                }
                case RuleType.Step:
                {
                    var stepToken = astNode.GetSubNodesOf(RuleType._Step).Cast<Token>().First();
                    var step = CreateStep(stepToken.MatchedKeyword, StepKeyword.Given, stepToken.Text, null, ScenarioBlock.Given); //TODO: G/W/T
                    step.MultiLineTextArgument = astNode.GetSubNodesOf(RuleType.Multiline_Arg).Cast<string>().FirstOrDefault();
                    step.TableArg = astNode.GetSubNodesOf(RuleType.Table_Arg).Cast<GherkinTable>().FirstOrDefault();
                    return step;
                }
                case RuleType.Background:
                {
                    var backgroundToken = astNode.GetSubNodesOf(RuleType._Background).Cast<Token>().First();
                    var description = astNode.GetSubNodesOf(RuleType.Description).Cast<string>().FirstOrDefault();
                    var steps = astNode.GetSubNodesOf(RuleType.Step).Cast<ScenarioStep>().ToArray();
                    return new Background(backgroundToken.MatchedKeyword, backgroundToken.Text, description, new ScenarioSteps(steps));
                }
                case RuleType.Scenario:
                {
                    //Tags will be added at Scenario_Base
                    var scenarioToken = astNode.GetSubNodesOf(RuleType._Scenario).Cast<Token>().First();
                    var description = astNode.GetSubNodesOf(RuleType.Description).Cast<string>().FirstOrDefault();
                    var steps = astNode.GetSubNodesOf(RuleType.Step).Cast<ScenarioStep>().ToArray();
                    return new Scenario(scenarioToken.MatchedKeyword, scenarioToken.Text, description, null, new ScenarioSteps(steps));
                }
                case RuleType.ScenarioOutline:
                {
                    //Tags will be added at Scenario_Base
                    var scenarioToken = astNode.GetSubNodesOf(RuleType._ScenarioOutline).Cast<Token>().First();
                    var description = astNode.GetSubNodesOf(RuleType.Description).Cast<string>().FirstOrDefault();
                    var steps = astNode.GetSubNodesOf(RuleType.Step).Cast<ScenarioStep>().ToArray();
                    var exampleSets = astNode.GetSubNodesOf(RuleType.Examples).Cast<ExampleSet>().ToArray();
                    return new ScenarioOutline(scenarioToken.MatchedKeyword, scenarioToken.Text, description, null, new ScenarioSteps(steps), new Examples(exampleSets));
                }
                case RuleType.Scenario_Base:
                {
                    var tags = astNode.GetAllSubNodes().OfType<Tag>().ToArray();
                    var scenario = astNode.GetAllSubNodes().OfType<Scenario>().First();
                    scenario.Tags = new Tags(tags);
                    return scenario;
                }
                case RuleType.Examples:
                {
                    var examplesToken = astNode.GetSubNodesOf(RuleType._Examples).Cast<Token>().First();
                    var description = astNode.GetSubNodesOf(RuleType.Description).Cast<string>().FirstOrDefault();
                    var tags = astNode.GetAllSubNodes().OfType<Tag>().ToArray();
                    var table = astNode.GetSubNodesOf(RuleType.Examples_Table).Cast<GherkinTable>().First();
                    return new ExampleSet(examplesToken.MatchedKeyword, examplesToken.Text, description, new Tags(tags), table);
                }
                case RuleType.Feature_File:
                {
                    var featureDef = astNode.GetSubNodesOf(RuleType.Feature_Def).Cast<ASTNode>().First();
                    var featureToken = featureDef.GetSubNodesOf(RuleType._Feature).Cast<Token>().First();
                    var description = featureDef.GetSubNodesOf(RuleType.Description).Cast<string>().FirstOrDefault();
                    var tags = featureDef.GetAllSubNodes().OfType<Tag>().ToArray();
                    var background = astNode.GetSubNodesOf(RuleType.Background).Cast<Background>().FirstOrDefault();
                    var scenarios = astNode.GetSubNodesOf(RuleType.Scenario_Base).Cast<Scenario>().ToArray();
                    return new Feature(featureToken.MatchedKeyword, featureToken.Text, new Tags(tags), description, background, scenarios, null); //TODO: comments;
                }
            }

            return astNode;
        }

        public ScenarioStep CreateStep(string keyword, StepKeyword stepKeyword, string text, FilePosition position, ScenarioBlock scenarioBlock)
        {
            ScenarioStep step;
            switch (stepKeyword)
            {
                case StepKeyword.Given:
                    step = new Given();
                    break;
                case StepKeyword.When:
                    step = new When();
                    break;
                case StepKeyword.Then:
                    step = new Then();
                    break;
                case StepKeyword.And:
                    step = new And();
                    break;
                case StepKeyword.But:
                    step = new But();
                    break;
                default:
                    throw new NotSupportedException();
            }

            step.Keyword = keyword;
            step.Text = text;
            step.FilePosition = position;
            step.ScenarioBlock = scenarioBlock;
            step.StepKeyword = stepKeyword;
            return step;
        }
    }
}
