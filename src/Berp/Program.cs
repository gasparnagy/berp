using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Berp.BerpGrammar;
using CommandLine;

namespace Berp
{
    public class Options
    {
        [Option('t', "template", Required = true, HelpText = "Template (.razor) file for generation.")]
        public string Template { get; set; }

        [Option('g', "grammar", Required = true, HelpText = "Grammar (.gnpg) file for generation.")]
        public string Grammar { get; set; }
        [Option('o', "output", Required = true, HelpText = "Generated parser class file.")]
        public string OutputFile { get; set; }
        [Option('d', null, HelpText = "Print details during execution.")]
        public bool DiagnosticsMode { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("GNPG Parser Generator 1.0");
            usage.AppendLine("gnpg.exe -g grammar.gnpg -t template.razor -o parseroutpout [-d]");
            return usage.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var grammarDefinition = File.ReadAllText(options.Grammar);
                var parser = new BerpGrammar.Parser();
                RuleSet ruleSet = (RuleSet)parser.Parse(new TokenScanner(new StringReader(grammarDefinition)));

                if (options.DiagnosticsMode)
                {
                    Console.WriteLine(ruleSet.ToString(true));
                    Console.WriteLine("---------------");
                }

                var states = StateCalculator.CalculateStates(ruleSet);

                Console.WriteLine("Claculated {0} states for the parser.", states.Count);

                if (options.DiagnosticsMode)
                {
                    foreach (var state in states.Values)
                    {
                        PrintStateTransitions(state);
                        PrintStateBranches(state.Branches, state.Id);
                    }
                }

                var generator = new Generator(ruleSet.GetSetting("Namespace", "ParserGen"), ruleSet.GetSetting("ClassName", "Parser"));
                generator.Generate(options.Template, ruleSet, states, options.OutputFile);
            }
        }
        private static void PrintStateTransitions(State state)
        {
            Console.WriteLine("{0}: {1}", state.Id, state.Comment);
            foreach (var transition in state.Transitions)
            {
                Console.WriteLine("    {0} -> {1}", transition.TokenType, transition.TargetState);
            }
        }

        private static void PrintStateBranches(List<Branch> branches, int state)
        {
//            Console.WriteLine("{0}:", state);
            Console.WriteLine();
            foreach (var branch in branches.OrderBy(b => b.TokenType))
            {
                Console.WriteLine("    {0}", branch);
                Console.WriteLine("    \t{0}", branch.GetProductionsText());
            }
        }
    }
}
