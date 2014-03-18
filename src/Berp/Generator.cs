using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine;
using Encoding = System.Text.Encoding;

namespace Berp
{
    class Generator
    {
        private readonly string ns;
        private readonly string className;

        public Generator(string ns, string className = "Parser")
        {
            this.ns = ns;
            this.className = className;
        }

        public void Generate(string templatePath, RuleSet ruleSet, Dictionary<int, State> states, string outputPath)
        {
            string template = File.ReadAllText(templatePath);

            var model = new GeneratorModel(states)
            {
                Namespace = ns,
                ParserClassName = className,
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
