using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Berp.Specs.Support;

public static class TestFolders
{
    public static readonly string UniqueId = DateTime.Now.ToString("s", CultureInfo.InvariantCulture).Replace(":", "");

    public static string InputFolder => 
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public static string OutputFolder
    {
        //a simple solution that puts everything to the output folder directly would look like this:
        //get { return Directory.GetCurrentDirectory(); }
        get
        {
            var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), UniqueId);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            return outputFolder;
        }
    }

    public static string TempFolder => Path.GetTempPath();

    // very simple helper methods that can improve the test code readability

    public static string GetInputFilePath(string fileName)
    {
        return Path.GetFullPath(Path.Combine(InputFolder, fileName));
    }

    // ReSharper disable once UnusedMember.Global
    public static string GetOutputFilePath(string fileName)
    {
        return Path.GetFullPath(Path.Combine(OutputFolder, fileName));
    }

    public static string GetTempFilePath(string fileName)
    {
        return Path.GetFullPath(Path.Combine(TempFolder, fileName));
    }
}