using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BerpGherkinParser
{
    public class DefaultSourceReader : ISourceReader
    {
        private int lineNo = 0;
        private readonly TextReader innerReader;

        public DefaultSourceReader(TextReader innerReader)
        {
            this.innerReader = innerReader;
        }

        public ISourceLine ReadLine()
        {
            var lineText = innerReader.ReadLine();
            if (lineText == null)
                return null;

            return new SourceLine(lineText, lineNo++);
        }

        class SourceLine : ISourceLine
        {
            private readonly string trimmedLineText;
            private readonly string lineText;
            private readonly int lineNo;

            public SourceLine(string lineText, int lineNo)
            {
                this.lineText = lineText;
                this.lineNo = lineNo;
                trimmedLineText = lineText.TrimStart();
            }

            public int Indent
            {
                get
                {
                    return lineText.Length - trimmedLineText.Length;
                }
            }

            public int LineNumber { get { return lineNo; } }

            public bool IsEmpty()
            {
                return trimmedLineText.Length == 0;
            }

            public bool StartsWith(string text)
            {
                return trimmedLineText.StartsWith(text);
            }
            public bool StartsWith(string text, char separator)
            {
                int textLength = text.Length;
                return trimmedLineText.Length > textLength && trimmedLineText.StartsWith(text) && trimmedLineText[textLength] == separator;
            }

            public string GetRestTrimmed(int length)
            {
                return trimmedLineText.Substring(length).Trim();
            }

            public IEnumerable<string> Split(int prefixToRemove)
            {
                return trimmedLineText.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(s => s.Substring(prefixToRemove));
            }

            public string GetLineText()
            {
                return lineText;
            }

            public IEnumerable<string> GetTableCells()
            {
                var parts = lineText.Split('|');
                return parts.Skip(1).Take(parts.Length - 2).Select(cv => cv.Trim());
            }

            public string GetLineText(int multiLineArgumentIndent)
            {
                if (lineText.Length < multiLineArgumentIndent)
                    return String.Empty;
                return lineText.Substring(multiLineArgumentIndent); //TODO: check if skipped part is whitespace
            }

            public void Detach()
            {
                //nop;
            }

            public override string ToString()
            {
                return String.Format("{0}:{1}", lineNo, lineText);
            }
        }
    }
}