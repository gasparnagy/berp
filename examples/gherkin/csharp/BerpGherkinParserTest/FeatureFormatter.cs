using System;
using System.Globalization;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow.Parser;
using TechTalk.SpecFlow.Parser.SyntaxElements;

namespace BerpGherkinParserTest
{
    public class FeatureFormatter
    {
        const string indent = "\t";

        public string GetFeatureText(Feature feature, CultureInfo defaultLanguage = null)
        {
            defaultLanguage = defaultLanguage ?? new CultureInfo("en-US");
            var dialectServices = new GherkinDialectServices(defaultLanguage);
            var dialect = dialectServices.GetGherkinDialect(feature);

            var result = new StringBuilder();

            AppendHeader(defaultLanguage, dialect, result);

            AppendTags(result, feature.Tags);
            AppendFeatureLine(feature, result);
            AppendLine(result);

            if (feature.Background != null)
            {
                AppendBackgroundLine(feature.Background, result);
                AppendSteps(feature.Background.Steps, result);
                AppendLine(result);
            }

            foreach (var scenario in feature.Scenarios)
            {
                var outline = scenario as ScenarioOutline;
                bool isOutline = outline != null;

                AppendTags(result, scenario.Tags);
                AppendScenarioLine(result, scenario, dialect, isOutline);

                AppendSteps(scenario.Steps, result);

                AppendLine(result);

                if (isOutline)
                {
                    foreach (var exampleSet in outline.Examples.ExampleSets)
                    {
                        AppendTags(result, exampleSet.Tags);
                        AppendExampleSetLine(result, dialect, exampleSet);
                        AppendTable(result, exampleSet.Table, indent);
                        AppendLine(result);
                    }
                }

                AppendLine(result);
            }

            return result.ToString();
        }

        protected virtual void AppendExampleSetLine(StringBuilder result, GherkinDialect dialect, ExampleSet exampleSet)
        {
            result.AppendFormat("{0}: {1}", dialect.GetBlockKeywords(GherkinBlockKeyword.Examples).First(), exampleSet.Title);
            AppendLine(result);
        }

        protected virtual void AppendScenarioLine(StringBuilder result, Scenario scenario, GherkinDialect dialect, bool isOutline)
        {
            AppendNodeLine(result, scenario.FilePosition, "{0}: {1}",
                dialect.GetBlockKeywords(isOutline ? GherkinBlockKeyword.ScenarioOutline : GherkinBlockKeyword.Scenario).First(),
                scenario.Title);
            if (!string.IsNullOrEmpty(scenario.Description))
                AppendMulitLine(result, scenario.Description);
        }

        protected virtual void AppendBackgroundLine(Background background, StringBuilder result)
        {
            AppendNodeLine(result, background.FilePosition, "{0}: {1}", background.Keyword, background.Title);
            if (!string.IsNullOrEmpty(background.Description))
                AppendMulitLine(result, background.Description);
        }

        protected virtual void AppendFeatureLine(Feature feature, StringBuilder result)
        {
            AppendNodeLine(result, feature.FilePosition, "{0}: {1}", feature.Keyword, feature.Title);
            if (!string.IsNullOrEmpty(feature.Description))
                AppendMulitLine(result, feature.Description);
        }

        protected virtual void AppendHeader(CultureInfo defaultLanguage, GherkinDialect dialect, StringBuilder result)
        {
            if (!DialectEquals(defaultLanguage, dialect))
            {
                result.AppendFormat("#language:{0}", dialect.CultureInfo);
                AppendLine(result);
            }
        }

        #region low-level write functions
        protected virtual void AppendNodeLine(StringBuilder result, FilePosition filePosition, string textFormat, params object[] args)
        {
            result.AppendFormat(textFormat, args);
            AppendLine(result);
        }

        protected virtual void AppendSinleLine(StringBuilder result, string text)
        {
            result.Append(text);
            AppendLine(result);
        }

        protected virtual void AppendMulitLine(StringBuilder result, string text)
        {
            var lines = text.Replace(Environment.NewLine, "\n").Split('\n');
            foreach (var line in lines)
            {
                AppendSinleLine(result, line);
            }
        }

        protected virtual void AppendLine(StringBuilder result)
        {
            result.AppendLine();
        }
        #endregion

        private static bool DialectEquals(CultureInfo defaultLanguage, GherkinDialect dialect)
        {
            if (!defaultLanguage.IsNeutralCulture)
                defaultLanguage = defaultLanguage.Parent;

            var dialectCulture = dialect.CultureInfo;
            if (!dialectCulture.IsNeutralCulture)
                dialectCulture = dialectCulture.Parent;

            return defaultLanguage.Equals(dialectCulture);
        }

        private void AppendSteps(ScenarioSteps steps, StringBuilder result)
        {
            foreach (var step in steps)
            {
                AppendStepLine(result, step);
                if (step.TableArg != null)
                {
                    AppendTable(result, step.TableArg, indent + indent);
                }
                if (step.MultiLineTextArgument != null)
                {
                    AppendMultiLineText(result, step.MultiLineTextArgument);
                }
            }
        }

        protected virtual void AppendStepLine(StringBuilder result, ScenarioStep step)
        {
            AppendNodeLine(result, step.FilePosition, "{2}{0}{1}", step.Keyword, step.Text, indent);
        }

        private void AppendTags(StringBuilder result, Tags tags)
        {
            if (tags == null || tags.Count == 0)
                return;

            foreach (var tag in tags)
            {
                result.Append("@");
                result.Append(tag.Name);
                result.Append(" ");
            }
            AppendLine(result);
        }

        private void AppendMultiLineText(StringBuilder result, string multiLineTextArgument)
        {
            const string pyStringQuote = "\"\"\"";
            AppendSinleLine(result, pyStringQuote);
            AppendMulitLine(result, multiLineTextArgument);
            AppendSinleLine(result, pyStringQuote);
        }

        private void AppendTable(StringBuilder result, GherkinTable table, string tableIndent)
        {
            int[] widths = new int[table.Header.Cells.Count()];
            CalculateWidth(widths, table.Header);
            foreach (var row in table.Body)
                CalculateWidth(widths, row);

            AppendTableRow(result, table.Header, tableIndent, widths);
            foreach (var row in table.Body)
            {
                AppendTableRow(result, row, tableIndent, widths);
            }
        }

        private void CalculateWidth(int[] widths, GherkinTableRow row)
        {
            int i = 0;
            foreach (var cell in row.Cells)
            {
                widths[i] = Math.Max(widths[i], cell.Value.Length);
                i++;
            }
        }

        protected virtual void AppendTableRow(StringBuilder result, GherkinTableRow row, string tableIndent, int[] widths)
        {
            result.Append(tableIndent);

            int i = 0;
            foreach (var cell in row.Cells)
            {
                result.Append("| ");
                result.Append(cell.Value);
                result.Append(new string(' ', widths[i] - cell.Value.Length));
                result.Append(" ");
                i++;
            }
            result.Append("|");
            AppendLine(result);
        }
    }
}
