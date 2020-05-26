using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp.BerpGrammar
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
            return String.Format("<{0}>{1}</{0}>", Node, String.Join(", ", SubNodes.Select(sn =>
                String.Format("[{0}:{1}]", sn.Key, String.Join(", ", sn.Value.Select(v => v.ToString()))))));
        }
    }
}