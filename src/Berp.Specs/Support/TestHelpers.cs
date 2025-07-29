using System;

namespace Berp.Specs.Support;

public static class TestHelpers
{
    public static string NormalizeText(string text)
    {
        return text.Trim().Replace(" ", "").Replace("\t", "");
    }

    public static string GetErrorMessage(this Exception exception)
    {
        if (exception.InnerException == null)
            return exception.Message;
        return $"{exception.Message} -> {GetErrorMessage(exception.InnerException)}";
    }
}