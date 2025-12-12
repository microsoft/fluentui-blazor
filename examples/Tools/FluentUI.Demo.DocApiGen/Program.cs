// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
                                              " --format <csharp|json|mcp>");
            Console.WriteLine();
            Console.WriteLine("Formats:");
            Console.WriteLine("  csharp  - Generate C# code with summary data dictionary");
            Console.WriteLine("  json    - Generate JSON with summary data");
            Console.WriteLine("  mcp     - Generate complete MCP documentation JSON for McpServer");
            return;
        }

        // Assembly and documentation file
        var assembly = Assembly.LoadFrom(dllFile);
        var docXml = new FileInfo(xmlFile);

        Console.WriteLine("Generating documentation...");

        if (format.Equals("mcp", StringComparison.OrdinalIgnoreCase))
        {
            // Generate MCP-compatible JSON documentation
            var mcpGenerator = new McpDocumentationGenerator(assembly, docXml);

            if (!string.IsNullOrEmpty(outputFile))
            {
                mcpGenerator.SaveToFile(outputFile);
                Console.WriteLine($"MCP documentation saved to {outputFile}");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(mcpGenerator.GenerateJson());
            }
        }
        else
        {
            // Generate traditional API documentation
            var apiGenerator = new ApiClassGenerator(assembly, docXml);

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
        }

        Console.WriteLine($"Completed in {_watcher.ElapsedMilliseconds} ms");
    }
}
