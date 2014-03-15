using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Berp.BerpGrammar
{
    public class ASTBuilder
    {
        private class ASTNode : List<object>
        {
            private RuleType ruleType;

            public ASTNode(RuleType ruleType)
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
                    result.AppendLine(subItem is ASTNode ? ((ASTNode)subItem).InternalToString(subIndent) : (subIndent + subItem.ToString()));
                result.Append(indent + "]");
                return result.ToString();
            }
        }

        private readonly Stack<ASTNode> stack = new Stack<ASTNode>();
        private ASTNode CurrentNode { get { return stack.Peek(); } }

        public ASTBuilder()
        {
            stack.Push(new ASTNode(RuleType.None));
        }

        public void Build(Token token)
        {
            CurrentNode.Add(token);
        }

        public void Push(RuleType ruleType)
        {
            stack.Push(new ASTNode(ruleType));
        }

        public void Pop(RuleType ruleType)
        {
            var node = stack.Pop();
            CurrentNode.Add(node);
        }


        public object RootNode { get { return CurrentNode.First(); } }
    }
}