using System;
using System.Collections.Generic;
using System.Linq;

namespace BerpGherkinParser
{
    public interface ISourceLine
    {
        int Indent { get; }
        bool IsEmpty();
        bool StartsWith(string text);
        bool StartsWith(string text, char separator);
        string GetRestTrimmed(int length);
        IEnumerable<string> Split(int prefixToRemove);
        string GetLineText();
        IEnumerable<string> GetTableCells();
        string GetLineText(int multiLineArgumentIndent);
        void Detach();
    }
}