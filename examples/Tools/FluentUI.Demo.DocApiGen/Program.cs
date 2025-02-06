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

        // Help
        if (string.IsNullOrEmpty(xmlFile) || string.IsNullOrEmpty(dllFile))
        {
            Console.WriteLine("Usage: DocApiGen --xml <xml file> --dll <dll file>");
            return;
        }

        // Load the assembly
        var assembly = Assembly.LoadFrom(dllFile);

        // Read the XML file
        var docReader = new LoxSmoke.DocXml.DocXmlReader(xmlFile);

        foreach (var type in assembly.GetTypes())
        {
            if (type.Name == "FluentButton")
            {
                //var apiClass = ApiClass.FromTypeName(assembly, type.Name, allProperties: true);

                var z = docReader.GetTypeComments(type);

                foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (member is PropertyInfo property)
                    {
                        var comments = docReader.GetMemberComments(property);

                        Console.WriteLine($"{property.Name}:    {comments.Summary}");

                        
                    }
                }
            }
        }
    }
}
