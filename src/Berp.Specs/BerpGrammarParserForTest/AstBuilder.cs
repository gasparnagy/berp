using System.Collections.Generic;
using System.Linq;
using System.Text;
using Berp.BerpGrammar;

namespace Berp.Specs.BerpGrammarParserForTest;

public class AstBuilderForTest : IAstBuilder<RuleSet>
{
    private class AstNode(RuleType ruleType) : List<object>
    {
        public override string ToString()
        {
            return InternalToString("");
        }

        private string InternalToString(string indent)
        {
            var subIndent = indent + "\t";
            var result = new StringBuilder();
            result.AppendLine(indent + "[" + ruleType);
            foreach(var subItem in this)
                result.AppendLine(subItem is AstNode astNode ? astNode.InternalToString(subIndent) : subIndent + subItem);
            result.Append(indent + "]");
            return result.ToString();
        }
    }

    private readonly Stack<AstNode> stack = new();
    private AstNode CurrentNode => stack.Peek();

    public AstBuilderForTest()
    {
        stack.Push(new AstNode(RuleType.None));
    }

    public void Build(Token token)
    {
        CurrentNode.Add(token);
    }

    public void StartRule(RuleType ruleType)
    {
        stack.Push(new AstNode(ruleType));
    }

    public void EndRule(RuleType ruleType)
    {
        var node = stack.Pop();
        CurrentNode.Add(node);
    }

    class RuleSetForTest(object node) : RuleSet((ParserGeneratorSettings)null)
    {
        public override string ToString()
        {
            return node.ToString();
        }
    }

    public RuleSet GetResult()
    {
        return new RuleSetForTest(CurrentNode.First());
    }

    public void Reset()
    {
        //nop
    }
}