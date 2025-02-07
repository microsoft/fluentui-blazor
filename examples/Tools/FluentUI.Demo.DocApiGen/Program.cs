// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

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

        // Help
        if (string.IsNullOrEmpty(xmlFile) || string.IsNullOrEmpty(dllFile))
        {
            Console.WriteLine("Usage: DocApiGen --xml <xml file> --dll <dll file>");
            return;
        }

        // Load the assembly
        var assembly = Assembly.LoadFrom(dllFile);

        // Read the XML file
        var apiClass = ApiClass.FromTypeName()

        foreach (var type in assembly.GetTypes())
        {
            if (type.Name == "FluentOption" || type.Name == "FluentButton")
            {

                Console.WriteLine();
            }
        }
    }

    private static string ConvertSeeCref(string input)
    {
        const string pattern = @"<see(also)* cref=""[\w]:Microsoft\.FluentUI\.AspNetCore\.Components\.([\w.]+)"" />";
        const string replacement = "`$2`";

        return Regex.Replace(input, pattern, replacement);
    }

    private static string ConvertSeeHref(string input)
    {
        const string pattern = @"<see href=""([^""]+)"">([^<]+)</see>";
        const string replacement = "[$2]($1)";

        return Regex.Replace(input, pattern, replacement);
    }

    private static string RemoveCrLf(string input)
    {
        return input.Replace("\r", "").Replace("\n", "");
    }
}
