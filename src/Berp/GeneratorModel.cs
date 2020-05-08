using System;
using System.Collections.Generic;
using System.Dynamic;
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
        public bool SimpleTokenMatcher { get; set; }
        public int MaxCollectedError { get; set; }
        public RuleSet RuleSet { get; set; }
        public dynamic Settings { get; }

        public State EndState
        {
            get { return States.Values.FirstOrDefault(s => s.IsEndState); }
        }

        public GeneratorModel(Dictionary<int, State> states, ParserGeneratorSettings settings)
        {
            States = states;

            var dynamicSettings = new ExpandoObject();
            foreach (var setting in settings)
            {
                ((IDictionary<string, object>)dynamicSettings)[setting.Key] = setting.Value;
            }
            Settings = dynamicSettings;
        }
    }
}