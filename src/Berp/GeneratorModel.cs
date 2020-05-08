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

        public class NullingExpandoObject : DynamicObject
        {
            private readonly Dictionary<string, object> values;

            public NullingExpandoObject(Dictionary<string, object> values)
            {
                this.values = values;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                // We don't care about the return value...
                values.TryGetValue(binder.Name, out result);
                return true;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                values[binder.Name] = value;
                return true;
            }
        }

        public GeneratorModel(Dictionary<int, State> states, ParserGeneratorSettings settings)
        {
            States = states;
            Settings = new NullingExpandoObject(settings);
        }
    }
}