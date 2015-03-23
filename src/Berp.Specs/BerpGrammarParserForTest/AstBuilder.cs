using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Berp.BerpGrammar;

namespace Berp.Specs.BerpGrammarParserForTest
{
    public class AstBuilderForTest : IAstBuilder
    {
        private class AstNode : List<object>
        {
            private RuleType ruleType;

            public AstNode(RuleType ruleType)
            {
                this.ruleType = ruleType;
            }

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
                    result.AppendLine(subItem is AstNode ? ((AstNode)subItem).InternalToString(subIndent) : (subIndent + subItem.ToString()));
                result.Append(indent + "]");
                return result.ToString();
            }
        }

        private readonly Stack<AstNode> stack = new Stack<AstNode>();
        private AstNode CurrentNode { get { return stack.Peek(); } }

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

        class RuleSetForTest : RuleSet
        {
            private readonly object node;

            public RuleSetForTest(object node)
                : base((Dictionary<string, object>)null)
            {
                this.node = node;
            }

            public override string ToString()
            {
                return node.ToString();
            }
        }

        public RuleSet GetResult()
        {
            return new RuleSetForTest(CurrentNode.First());
        } 
    }
}