using BerpGherkinParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser;
using TechTalk.SpecFlow.Parser.SyntaxElements;

namespace BerpGherkinParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            AstTest();
#else
            PerformanceTest();
#endif
        }

        private static void AstTest()
        {
            try
            {
                BerpGherkinParserTest(new[] { @"..\..\..\..\feature_files\RetryTests.feature" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void PerformanceTest()
        {
            var files = Directory.GetFiles(@"..\..\..\..\feature_files");
            BerpGherkinParserTest(files);
            DefaultGherkinParserTest(files);
        }

        private static void BerpGherkinParserTest(string[] files)
        {
            GherkinParserTest(files, "Berp", () => new Parser(), BerpGherkinParserTest);
        }

        private static void DefaultGherkinParserTest(string[] files)
        {
            GherkinParserTest(files, "Gherkin2 (SpecFlow 1.9)", () => new SpecFlowLangParser(new CultureInfo("en-US")), DefaultGherkinParserTest);
        }

        private static void GherkinParserTest<TParser>(string[] files, string name, Func<TParser> parserFactory, Action<TParser, string> runner)
        {
            Console.WriteLine("------- {0} Gherkin Parser -------", name);
            var memBefore = GC.GetTotalMemory(true);
            double msec;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var parser = parserFactory();
            Console.WriteLine("new init: {0}", sw.ElapsedMilliseconds);
            sw.Stop();
            sw.Reset();
            sw.Start();
            foreach (var file in files)
                runner(parser, file);
            msec = sw.ElapsedMilliseconds;
            Console.WriteLine("first round:    {0:00.00}msec, {1:0.###}msec/file", msec, msec / files.Length);

            sw.Stop();
            sw.Reset();
            sw.Start();
            const int count = 5;
            for (int i = 0; i < count; i++)
            {
                foreach (var file in files)
                    runner(parser, file);
            }
            msec = (double)sw.ElapsedMilliseconds / count;
            Console.WriteLine("further rounds: {0:00.00}msec, {1:0.###}msec/file", msec, msec / files.Length);
            var memAfter = GC.GetTotalMemory(false);
            var memAfterCleanup = GC.GetTotalMemory(true);
            Console.WriteLine("Memory usage:   {0:0.##} Mb", (double)(memAfter - memBefore) / (1024 * 1024));
            Console.WriteLine("Memory 'leak':  {0:0.###} Mb", (double)(memAfterCleanup - memBefore) / (1024 * 1024));
        }


        private static void BerpGherkinParserTest(Parser parser, string file)
        {
            Feature feature;
            using (StreamReader reader = new StreamReader(file))
            {
                var scanner = new TokenScanner(new FastSourceReader(reader));
                //var scanner = new TokenScanner(new DefaultSourceReader(reader));
                feature = (Feature)parser.Parse(scanner);
            }

#if DEBUG
                //Console.WriteLine(File.ReadAllText(file));
                //Console.WriteLine("--------------");

                var formatter = new FeatureFormatter();
                Console.WriteLine(formatter.GetFeatureText(feature));
#endif
        }

        private static void DefaultGherkinParserTest(SpecFlowLangParser parser, string file)
        {
            Feature feature;
            using (StreamReader reader = new StreamReader(file))
            {
                feature = parser.Parse(reader, file);
            }
        }

    }
}
