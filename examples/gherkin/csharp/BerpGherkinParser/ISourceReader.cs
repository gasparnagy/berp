using System;
using System.Linq;

namespace BerpGherkinParser
{
    public interface ISourceReader
    {
        ISourceLine ReadLine();
    }
}