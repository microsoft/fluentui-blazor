// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FluentUI.Demo.DocApiGen;

/// <summary />
public class Program
{
    private static readonly System.Diagnostics.Stopwatch _watcher = new ();

    /// <summary />
    public static void Main(string[] args)
    {
        _watcher.Start();

        Console.WriteLine($"-------------------------------------------------------------------");
        Console.WriteLine($" DocApiGen v{Assembly.GetEntryAssembly()?.GetName().Version}       ");
        Console.WriteLine($" A simple command line tool to generate the documentation classes. ");
        Console.WriteLine($"-------------------------------------------------------------------");
        Console.WriteLine();

        // Build a configuration object from command line
        var config = new ConfigurationBuilder().AddCommandLine(args).Build();
        var xmlFile = config["xml"];
        var dllFile = config["dll"];
        var outputFile = config["output"];
        var format = config["format"] ?? "json";

        // Help
        if (string.IsNullOrEmpty(xmlFile) || string.IsNullOrEmpty(dllFile))
        {
            Console.WriteLine("Usage: DocApiGen --xml <xml_file>" +
                                              " --dll <dll_file>" +
                                              " --output <generated_file>" +
                                              " --format <csharp|json>");
            return;
        }

        // Assembly and documentation file
        var assembly = Assembly.LoadFrom(dllFile);
        var docXml = new FileInfo(xmlFile);
        var apiGenerator = new ApiClassGenerator(assembly, docXml);

        Console.WriteLine("Generating documentation...");
        if (!string.IsNullOrEmpty(outputFile))
        {
            apiGenerator.SaveToFile(outputFile, format);
            Console.WriteLine($"Documentation saved to {outputFile}");
        }
        else
        {
            Console.WriteLine();
            if (format == "json")
            {
                Console.WriteLine(apiGenerator.GenerateJson());
            }
            else
            {
                Console.WriteLine(apiGenerator.GenerateCSharp());
            }
        }

        Console.WriteLine($"Completed in {_watcher.ElapsedMilliseconds} ms");
    }
}
