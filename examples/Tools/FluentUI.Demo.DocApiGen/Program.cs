// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FluentUI.Demo.DocApiGen;

/// <summary />
public class Program
{
    /// <summary />
    public static void Main(string[] args)
    {
        Console.WriteLine($"-------------------------------------------------------------------");
        Console.WriteLine($" DocApiGen v{Assembly.GetEntryAssembly()?.GetName().Version}          ");
        Console.WriteLine($" A simple command line tool to generate the documentation classes. ");
        Console.WriteLine($"-------------------------------------------------------------------");
        Console.WriteLine();

        // Build a configuration object from command line
        var config = new ConfigurationBuilder().AddCommandLine(args).Build();
        var xmlFile = config["xml"];
        var dllFile = config["dll"];
        var outputFile = config["output"];

        // Help
        if (string.IsNullOrEmpty(xmlFile) || string.IsNullOrEmpty(dllFile))
        {
            Console.WriteLine("Usage: DocApiGen --xml <xml file> --dll <dll file> --output <generated file>");
            return;
        }

        // Assembly and documentation file
        var assembly = Assembly.LoadFrom(dllFile);
        var docXml = new FileInfo(xmlFile);

        var apiGenerator = new ApiClassGenerator(assembly, docXml);

        if (!string.IsNullOrEmpty(outputFile))
        {
            apiGenerator.SaveToFile(outputFile);
            Console.WriteLine($"Documentation saved to {outputFile}");
        }
    }
}
