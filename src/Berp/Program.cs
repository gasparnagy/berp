using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Berp.BerpGrammar;
using CommandLine;
using CommandLine.Text;

namespace Berp
{
    public class Options
    {
        [Option('t', "template", Required = true, HelpText = "Template (.razor) file for generation.")]
        public string Template { get; set; }

        [Option('g', "grammar", Required = true, HelpText = "Grammar (.berp) file for generation.")]
        public string Grammar { get; set; }
        [Option('o', "output", Required = true, HelpText = "Generated parser class file.")]
        public string OutputFile { get; set; }
        [Option('d', "details", HelpText = "Print details during execution.")]
        public bool DiagnosticsMode { get; set; }
        [Option("settings", Required = true, HelpText = "Extends/overrides settings in grammar file, use 'key1=value1,key2=value2' format")]
        public string SettingsOverride { get; set; }

        public HelpText GetHeader()
        {
            var help = new HelpText
            {
                Heading = HeadingInfo.Default,
                Copyright = CopyrightInfo.Default,
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine(string.Empty);

            help.AddPreOptionsLine("Licensed under the Apache License, Version 2.0 (the \"License\")");
            help.AddPreOptionsLine("    http://www.apache.org/licenses/LICENSE-2.0");
            return help;
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            var commandLineParseResult = CommandLine.Parser.Default.ParseArguments<Options>(args);

            int result = 2;

            commandLineParseResult.WithParsed<Options>(options =>
            {
                try
                {
                    GenerateParserInternal(options);
                    result = 0;
                }
                catch (Exception thrownEx)
                {
                    Console.WriteLine();
                    Console.WriteLine("Generation failed.");
                    var ex = thrownEx;
                    while (ex != null)
                    {
                        Console.WriteLine(ex.Message);
                        ex = ex.InnerException;
                    }

                    if (options.DiagnosticsMode)
                    {
                        Console.WriteLine(thrownEx);
                    }

                    result = 1;
                }
            });

            return result;
        }

        private static void GenerateParserInternal(Options options)
        {
            Console.WriteLine(options.GetHeader());
            Console.WriteLine();
            Console.WriteLine("Generating parser for grammar '{0}' using template '{1}'.", Path.GetFileName(options.Grammar), Path.GetFileName(options.Template));

            Console.WriteLine("Loading grammar...");
            var grammarDefinition = File.ReadAllText(options.Grammar);
            var parser = new BerpGrammar.Parser();
            var ruleSet = parser.Parse(new TokenScanner(new StringReader(grammarDefinition)));

            if (!string.IsNullOrWhiteSpace(options.SettingsOverride))
            {
                var overrides = options.SettingsOverride.Split(',');
                foreach (var settingSpec in overrides)
                {
                    var parts = settingSpec.Split('=');
                    ruleSet.Settings[parts[0].Trim()] = parts[1].Trim();
                }
            }

            int tokenCount = ruleSet.Count(r => r is TokenRule);
            Console.WriteLine("The grammar was loaded with {0} tokens and {1} rules.", tokenCount, ruleSet.Count() - tokenCount);

            if (options.DiagnosticsMode)
            {
                Console.WriteLine(ruleSet.ToString(true));
                Console.WriteLine("---------------");
            }

            Console.WriteLine("Calculating parser states...");
            var states = StateCalculator.CalculateStates(ruleSet);

            Console.WriteLine("{0} states calculated for the parser.", states.Count);

            if (options.DiagnosticsMode)
            {
                foreach (var state in states.Values)
                {
                    PrintStateTransitions(state);
                    PrintStateBranches(state.Branches, state.Id);
                }
            }

            Console.WriteLine("Generating parser class...");
            var generator = new Generator(ruleSet.Settings);
            generator.Generate(options.Template, ruleSet, states, options.OutputFile);
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
