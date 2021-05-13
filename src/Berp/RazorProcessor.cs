using System;
using System.Linq;
using System.Text.RegularExpressions;
using RazorEngineCore;

namespace Berp
{
    public class BerpTemplateBase<TModel> : RazorEngineTemplateBase<TModel>
    {
        public string Raw(object value)
        {
            return value?.ToString();
        }
    }

    static class RazorProcessor
    {
        public static string RazorParse(string templateText, GeneratorModel model)
        {
            templateText = PreProcess(templateText);
            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                var template = razorEngine.Compile<BerpTemplateBase<GeneratorModel>>(templateText, builder =>
                {
                    builder.AddAssemblyReferenceByName(
                        "System.Collections, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                    builder.AddUsing("System.Collections.Generic");
                });

                string result = template.Run(instance =>
                {
                    instance.Model = model;
                });

                return result;
            }
            catch (RazorEngineCompilationException compilationException)
            {
                throw new ParserGeneratorException(
                    $"{compilationException.Message}{Environment.NewLine}All errors:{Environment.NewLine}{string.Join(Environment.NewLine, compilationException.Errors.Select(e => "  " + e.ToString()))}{Environment.NewLine}Source:{Environment.NewLine}{compilationException.GeneratedCode}",
                    compilationException);
            }
        }

        private static string PreProcess(string templateText)
        {
            templateText = FixHelperDeclaration(templateText);
            templateText = FixHelperUsage(templateText);
            return templateText;
        }

        private static string FixHelperDeclaration(string templateText)
        {
            var helperPrefix = "@helper";
            var usages = Regex.Matches(templateText, helperPrefix);
            foreach (var match in usages.Reverse())
            {
                var closeIndex = FindCloseParen(templateText, match.Index, '{', '}');
                if (closeIndex < 0)
                    continue;
                templateText = templateText.Insert(closeIndex + 1, "}");
                templateText = templateText.Remove(match.Index, helperPrefix.Length);
                templateText = templateText.Insert(match.Index, "@functions { void");
            }

            return templateText;
        }

        private static string FixHelperUsage(string templateText)
        {
            var functions = Regex.Matches(templateText, @"void (?<name>\w+)\(")
                .Select(m => m.Groups["name"].Value)
                .Distinct()
                .ToArray();

            var functionsOption = string.Join("|", functions);

            var usages = Regex.Matches(templateText, $@"@({functionsOption})\(");
            foreach (var match in usages.Reverse())
            {
                var closeIndex = FindCloseParen(templateText, match.Index);
                if (closeIndex < 0)
                    continue;
                templateText = templateText.Insert(closeIndex + 1, ";}");
                templateText = templateText.Insert(match.Index + 1, "{");
            }

            return templateText;
        }

        private static int FindCloseParen(string text, in int startIndex, char openChar = '(', char closeChar = ')')
        {
            var firstOpenParen = text.IndexOf(openChar, startIndex);
            if (firstOpenParen < 0 || firstOpenParen == text.Length - 1)
                return -1;

            int openCount = 1;
            for (int i = firstOpenParen + 1; i < text.Length; i++)
            {
                var ch = text[i];
                openCount += ch == openChar ? 1 : ch == closeChar ? -1 : 0;
                if (openCount == 0)
                    return i;
            }

            return -1;
        }
    }
}
