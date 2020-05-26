using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp.BerpGrammar
{
    internal class DomBuilder
    {
        private bool firstRule = true;
        readonly List<Rule> tempRules = new List<Rule>(); 
        private int counter = 0;
        public object BuildFromNode(AstNode astNode)
        {
            switch (astNode.Node)
            {
                case RuleType.RuleDefinitionElement:
                {
                    var core = astNode.GetAllSubNodes().First();
                    var multiplierToken = astNode.GetAllSubNodes().Skip(1).OfType<Token>().FirstOrDefault();
                    var multiplier = Multilicator.One;
                    if (multiplierToken != null)
                        multiplier = multiplierToken.TokenType == TokenType.AnyMultiplier
                            ? Multilicator.Any
                            : multiplierToken.TokenType == TokenType.OneOrMoreMultiplier
                                ? Multilicator.OneOrMore
                                : multiplierToken.TokenType == TokenType.OneOrZeroMultiplier
                                    ? Multilicator.OneOrZero
                                    : Multilicator.One;
                    var ruleElement = new RuleElement(core is Rule ? ((Rule) core).Name : ((Token)core).Text, multiplier);
                    return ruleElement;
                }

                case RuleType.Parameter:
                {
                    var paramName = astNode.GetAllSubNodes().Cast<Token>().First().Text;
                    var values = astNode.GetAllSubNodes().Skip(1).Cast<Token>().Select(t => (object)t.Text).ToArray();
                    return new KeyValuePair<string, object>(paramName, values.Length == 1 ? values[0] : values);
                }

                case RuleType.Settings:
                {
                    var parameters = astNode.GetAllSubNodes().Cast<KeyValuePair<string, object>>().ToArray();
                    return parameters.ToDictionary(p => p.Key, p => p.Value);
                }

                case RuleType.AlternateElement:
                {
                    var alternatives = astNode.GetAllSubNodes().ToArray();
                    return AddTempRule(new AlternateRule(string.Format("__alt{0}", counter++),
                        alternatives.Cast<Token>().Select(t => t.Text).ToArray()));
                }

                case RuleType.GroupElement:
                {
                    var alternatives = astNode.GetAllSubNodes().Cast<RuleElement>().ToArray();
                    return AddTempRule(new SequenceRule(string.Format("__grp{0}", counter++), alternatives));
                }

                case RuleType.RuleDefinition:
                {
                    var ruleElements = astNode.GetAllSubNodes().OfType<RuleElement>().ToArray();
                    var ruleName = ((Token) astNode.GetAllSubNodes().First()).Text;
                    var isProduction = astNode.GetAllSubNodes().Skip(1).OfType<Token>().Any(t => t.TokenType == TokenType.Production);
                    var lookAhead = astNode.GetAllSubNodes().OfType<LookAheadHint>().FirstOrDefault();
                    var rule = firstRule ? new StartRule(ruleName, ruleElements) : new SequenceRule(ruleName, ruleElements);
                    if (!isProduction)
                        rule.IgnoreProduction();

                    rule.LookAheadHint = lookAhead;
                    firstRule = false;
                    return rule;
                }
                case RuleType.LookAheadTokenList1:
                case RuleType.LookAheadTokenList2:
                {
                    var tokens = astNode.GetAllSubNodes().Cast<Token>().ToArray();
                    return tokens;
                }
                case RuleType.LookAhead:
                {
                    var items = astNode.GetAllSubNodes().Cast<Token[]>().ToArray();
                    var expected = items.Length == 0 ? new Berp.TokenType[0] :
                        items.Last().Select(t => new Berp.TokenType(t.Text.Substring(1))).ToArray();
                    var skip = items.Length > 1 ? items[0].Select(t => new Berp.TokenType(t.Text.Substring(1))).ToArray() : new Berp.TokenType[0];
                    return new LookAheadHint(expected, skip);
                }

                case RuleType.Grammar:
                {
                    var rules = astNode.GetAllSubNodes().OfType<Rule>().ToArray();
                    var settings = new ParserGeneratorSettings(astNode.GetAllSubNodes().OfType<Dictionary<string, object>>().FirstOrDefault());
                    var ruleSet = new RuleSet(settings);
                    ruleSet.AddRange(rules);
                    ruleSet.AddRange(tempRules);
                    ruleSet.Resolve();
                    return ruleSet;
                }
            }

            throw new Exception("unknown node: " + astNode.Node);
        }

        private object AddTempRule(Rule rule)
        {
            rule.Temporary();
            tempRules.Add(rule);
            return rule;
        }

        public IEnumerable<object> BuildFromToken(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.EOL:
                case TokenType.EOF:
                case TokenType.Comma:
                case TokenType.RParen:
                case TokenType.LParen:
                case TokenType.RBracket:
                case TokenType.LBracket:
                case TokenType.Arrow:
                case TokenType.AlternateOp:
                    break;
                default:
                    yield return token;
                    break;
            }
        }
    }
}