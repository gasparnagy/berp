using System;
using System.Collections.Generic;
using System.IO;
using RazorEngine;
using Encoding = System.Text.Encoding;

namespace Berp
{
    public class Generator
    {
        private readonly string ns;
        private readonly string className;
        private readonly string targetNamespace;
        private readonly string targetClassName;
        private readonly bool simpleTokenMatcher;
        private readonly int maxCollectedError;
        private readonly ParserGeneratorSettings settings;

        public Generator(ParserGeneratorSettings settings)
            :this(
                settings.GetSetting("Namespace", "ParserGen"),
                settings.GetSetting("ClassName", "Parser"),
                settings.GetSetting("TargetNamespace", (string)null),
                settings.GetSetting("TargetClassName", "Ast"),
                settings.GetSetting("SimpleTokenMatcher", false),
                settings.GetSetting("MaxCollectedError", 10),
                settings)
        {
        }

        public Generator(string ns, string className, string targetNamespace, string targetClassName, bool simpleTokenMatcher, int maxCollectedError, ParserGeneratorSettings settings)
        {
            this.ns = ns;
            this.className = className;
            this.targetNamespace = targetNamespace ?? ns;
            this.targetClassName = targetClassName;
            this.simpleTokenMatcher = simpleTokenMatcher;
            this.maxCollectedError = maxCollectedError;
            this.settings = settings;
        }

        public void Generate(string templatePath, RuleSet ruleSet, Dictionary<int, State> states, string outputPath)
        {
            string template = File.ReadAllText(templatePath);

            var model = new GeneratorModel(states, settings)
            {
                Namespace = ns,
                ParserClassName = className,
                TargetNamespace = targetNamespace,
                TargetClassName = targetClassName,
                SimpleTokenMatcher = simpleTokenMatcher,
                MaxCollectedError = maxCollectedError,
                RuleSet = ruleSet
            };

            string result = Razor.Parse(template, model);
            if (File.Exists(outputPath) && File.ReadAllText(outputPath).Equals(result))
            {
                Console.WriteLine("Parser class up-to-date.");
                return;
            }
            File.WriteAllText(outputPath, result, Encoding.UTF8);
            Console.WriteLine("Parser class generated to '{0}'.", outputPath);
        }
    }
}
