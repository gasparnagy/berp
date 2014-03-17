using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berp.Specs.Support
{
    public static class TestHelpers
    {
        static public string NormalizeText(string text)
        {
            return text.Trim().Replace(" ", "").Replace("\t", "");
        }

        public static bool CollectionEquals<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return a.Except(b).Empty() && b.Except(a).Empty();
        }

        public static bool Empty<T>(this IEnumerable<T> a)
        {
            return !a.Any();
        }

        public static string GetErrorMessage(this Exception exception)
        {
            if (exception.InnerException == null)
                return exception.Message;
            return string.Format("{0} -> {1}", exception.Message, GetErrorMessage(exception.InnerException));
        }
    }
}
