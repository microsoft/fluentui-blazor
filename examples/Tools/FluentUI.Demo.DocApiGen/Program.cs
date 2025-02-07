// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Extensions;
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

        // Help
        if (string.IsNullOrEmpty(xmlFile) || string.IsNullOrEmpty(dllFile))
        {
            Console.WriteLine("Usage: DocApiGen --xml <xml file> --dll <dll file>");
            return;
        }

        // Assembly and documentation file
        var assembly = Assembly.LoadFrom(dllFile);
        var docXml = new FileInfo(xmlFile);

        var apiGenerator = new ApiClassGenerator(assembly, docXml);

        foreach (var type in assembly.GetTypes().Where(i => i.IsValidType()))
        {
            var apiClass = apiGenerator.FromTypeName(type);
            var apiClassMembers = apiClass.ToDictionary();

            if (apiClassMembers.Any())
            {
                Console.WriteLine($"{apiClass.Name}");
                foreach (var member in apiClass.ToDictionary())
                {
                    Console.WriteLine($"    {member.Key} => {member.Value}");
                }
            }
        }
    }
}
