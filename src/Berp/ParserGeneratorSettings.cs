using System;
using System.Collections.Generic;

namespace Berp
{
    public class ParserGeneratorSettings : Dictionary<string, object>
    {
        public ParserGeneratorSettings()
        {
        }

        public ParserGeneratorSettings(Dictionary<string, object> settings) : base(settings)
        {
        }

        public T GetSetting<T>(string name, T defaultValue = default(T))
        {
            object paramValue;
            if (this.TryGetValue(name, out paramValue))
            {
                if (paramValue == null)
                    return defaultValue;

                if (!(paramValue is T))
                    paramValue = Convert.ChangeType(paramValue, typeof(T));

                return (T)paramValue;
            }
            return defaultValue;
        }
    }
}
