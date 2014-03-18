using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp.BerpGrammar
{
    public class AstBuilder
    {
        public class AstNode
        {
            public RuleType Node { get; set; }
            public List<KeyValuePair<RuleType, List<object>>> SubNodes { get; set; }

            public IEnumerable<object> GetSubNodesOf(RuleType ruleType)
            {
                return SubNodes.Where(sn => sn.Key == ruleType).SelectMany(sn => sn.Value);
            }

            public IEnumerable<object> GetAllSubNodes()
            {
                return SubNodes.SelectMany(sn => sn.Value);
            }

            public void AddSubNode(RuleType nodeName, object subNode)
            {
                if (SubNodes.Count > 0)
                {
                    var lastSubNode = SubNodes.LastOrDefault();
                    if (lastSubNode.Key == nodeName)
                    {
                        lastSubNode.Value.Add(subNode);
                        return;
                    }
                }
                SubNodes.Add(new KeyValuePair<RuleType, List<object>>(nodeName, new List<object> { subNode }));
            }

            public AstNode()
            {
                SubNodes = new List<KeyValuePair<RuleType, List<object>>>();
            }

            public override string ToString()
            {
                return string.Format("<{0}>{1}</{0}>", Node, string.Join(", ", SubNodes.Select(sn =>
                    string.Format("[{0}:{1}]", sn.Key, string.Join(", ", sn.Value.Select(v => v.ToString()))))));
            }
        }


        private DomBuilder domBuilder = new DomBuilder();
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

        public object GetResult() { return CurrentNode.SubNodes[0].Value[0]; }
    }
}