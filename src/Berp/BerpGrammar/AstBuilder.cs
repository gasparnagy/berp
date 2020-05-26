using System;
using System.Collections.Generic;

namespace Berp.BerpGrammar
{
    public class AstBuilder<T> : IAstBuilder<T>
    {
        private readonly DomBuilder domBuilder = new DomBuilder();
        private readonly Stack<AstNode> stack = new Stack<AstNode>();
        public AstNode CurrentNode { get { return stack.Peek(); } }

        public AstBuilder()
        {
            stack.Push(new AstNode { Node = RuleType.None });
        }

        public void Build(Token token)
        {
            var subNodes = domBuilder.BuildFromToken(token);
            foreach (var subNode in subNodes)
            {
                CurrentNode.AddSubNode((RuleType)token.TokenType, subNode);
            }
        }

        public void StartRule(RuleType node)
        {
            stack.Push(new AstNode { Node = node });
        }

        public void EndRule(RuleType node)
        {
            Pop();
        }

        public void Pop()
        {
            var astNode = stack.Pop();
            var subNode = domBuilder.BuildFromNode(astNode);
            CurrentNode.AddSubNode(astNode.Node, subNode);
        }

        public T GetResult() { return (T)CurrentNode.SubNodes[0].Value[0]; }

        public void Reset()
        {
            //nop
        }
    }
}