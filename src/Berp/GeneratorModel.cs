using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp
{
    public class GeneratorModel
    {
        public Dictionary<int, State> States { get; private set; }
        public string Namespace { get; set; }
        public string ParserClassName { get; set; }
        public string TargetNamespace { get; set; }
        public string TargetClassName { get; set; }
        public RuleSet RuleSet { get; set; }

        public State EndState
        {
            get { return States.Values.FirstOrDefault(s => s.IsEndState); }
        }

        public GeneratorModel(Dictionary<int, State> states)
        {
            States = states;
        }
    }
}