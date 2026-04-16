// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Formatters;
using FluentUI.Demo.DocApiGen.Generators;
using FluentUI.Demo.DocApiGen.Models;
using Microsoft.Extensions.Configuration;

namespace FluentUI.Demo.DocApiGen;

/// <summary>
/// Main entry point for the DocApiGen tool.
/// </summary>
public class Program
{
    private static readonly System.Diagnostics.Stopwatch _watcher = new();
    private static readonly char[] InputSeparators = [';', '|'];

    /// <summary>
    /// Main method.
    /// </summary>
    public static void Main(string[] args)
    {
        _watcher.Start();

        Console.WriteLine($"-------------------------------------------------------------------");
        Console.WriteLine($" DocApiGen v{Assembly.GetEntryAssembly()?.GetName().Version}       ");
        Console.WriteLine($" A tool to generate API documentation from assemblies.            ");
        Console.WriteLine($"-------------------------------------------------------------------");
        Console.WriteLine();

        // Build a configuration object from command line
        var config = new ConfigurationBuilder().AddCommandLine(args).Build();
        var xmlFiles = config["xml"];
        var dllFiles = config["dll"];
        var outputFile = config["output"];
        var format = config["format"] ?? "json";
        var modeArg = config["mode"] ?? "summary";

        // Help
        if (string.IsNullOrEmpty(xmlFiles) || string.IsNullOrEmpty(dllFiles))
        {
            ShowHelp();
            return;
        }

        try
        {
            // Parse generation mode
            var mode = ParseMode(modeArg);

            // Validate format compatibility
            ValidateFormatCompatibility(mode, format);

            // Load assemblies and XML documentation
            var inputs = CreateDocumentationInputs(dllFiles, xmlFiles);

            Console.WriteLine("Generating documentation...");
            Console.WriteLine($"  Assemblies: {string.Join(", ", inputs.Select(input => input.Assembly.GetName().Name))}");
            Console.WriteLine($"  Mode: {mode}");
            Console.WriteLine($"  Format: {format}");
            Console.WriteLine();

            // Create generator and formatter
            var generator = DocumentationGeneratorFactory.Create(mode, inputs);
            var formatter = OutputFormatterFactory.Create(format);

            // Generate and output
            if (!string.IsNullOrEmpty(outputFile))
            {
                generator.SaveToFile(outputFile, formatter);
                Console.WriteLine($"✓ Documentation saved to: {outputFile}");
            }
            else
            {
                var output = generator.Generate(formatter);
                Console.WriteLine(output);
            }

            _watcher.Stop();
            Console.WriteLine();
            Console.WriteLine($"✓ Completed in {_watcher.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"✗ Error: {ex.Message}");

            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                Console.ResetColor();
            }

            //Environment.Exit(1);
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  DocApiGen --xml <xml_file[;xml_file2]> --dll <dll_file[;dll_file2]> [options]");
        Console.WriteLine();
        Console.WriteLine("Required Arguments:");
        Console.WriteLine("  --xml <file>      Path to the XML documentation file, or multiple paths separated by ';' or '|'");
        Console.WriteLine("  --dll <file>      Path to the assembly DLL file, or multiple paths separated by ';' or '|'");
        Console.WriteLine();
        Console.WriteLine("Optional Arguments:");
        Console.WriteLine("  --output <file>   Path to the output file (default: stdout)");
        Console.WriteLine("  --format <name>   Output format (default: json)");
        Console.WriteLine("  --mode <name>     Generation mode (default: summary)");
        Console.WriteLine();
        Console.WriteLine("Formats:");
        Console.WriteLine("  json     - Generate JSON documentation");
        Console.WriteLine("  csharp   - Generate C# code with documentation dictionary");
        Console.WriteLine();
        Console.WriteLine("Modes:");
        Console.WriteLine("  summary  - Generate documentation with only [Parameter] properties");
        Console.WriteLine("             Supports: json, csharp");
        Console.WriteLine("  all      - Generate complete documentation (properties, methods, events)");
        Console.WriteLine("             Supports: json only");
        Console.WriteLine("  mcp      - Generate MCP server documentation (tools, resources, prompts)");
        Console.WriteLine("             Supports: json only");
        Console.WriteLine("  icons    - Generate Fluent UI icon documentation");
        Console.WriteLine("             Supports: json only");
        Console.WriteLine("  emojis   - Generate Fluent UI emoji documentation");
        Console.WriteLine("             Supports: json only");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  # Generate Summary mode JSON");
        Console.WriteLine("  DocApiGen --xml MyApp.xml --dll MyApp.dll --output api-summary.json");
        Console.WriteLine();
        Console.WriteLine("  # Generate Summary mode C#");
        Console.WriteLine("  DocApiGen --xml MyApp.xml --dll MyApp.dll --output CodeComments.cs --format csharp");
        Console.WriteLine();
        Console.WriteLine("  # Generate All mode JSON");
        Console.WriteLine("  DocApiGen --xml MyApp.xml --dll MyApp.dll --output api-all.json --mode all");
        Console.WriteLine();
        Console.WriteLine("  # Generate Summary mode JSON from multiple assemblies");
        Console.WriteLine("  DocApiGen --xml Core.xml;Charts.xml --dll Core.dll;Charts.dll --output api-summary.json");
        Console.WriteLine();
        Console.WriteLine("  # Generate MCP documentation JSON");
        Console.WriteLine("  DocApiGen --xml McpServer.xml --dll McpServer.dll --output mcp-docs.json --mode mcp");
        Console.WriteLine();
        Console.WriteLine("  # Generate Icons documentation JSON");
        Console.WriteLine("  DocApiGen --xml MyApp.xml --dll MyApp.dll --output icons.json --mode icons");
        Console.WriteLine();
        Console.WriteLine("  # Generate Emojis documentation JSON");
        Console.WriteLine("  DocApiGen --xml MyApp.xml --dll MyApp.dll --output emojis.json --mode emojis");
    }

    private static GenerationMode ParseMode(string modeArg)
    {
        return modeArg.ToLowerInvariant() switch
        {
            "summary" => GenerationMode.Summary,
            "all" => GenerationMode.All,
            "mcp" => GenerationMode.Mcp,
            "icons" => GenerationMode.Icons,
            "emojis" => GenerationMode.Emojis,
            _ => throw new ArgumentException($"Invalid mode '{modeArg}'. Valid modes are: summary, all, mcp, icons, emojis")
        };
    }

    private static void ValidateFormatCompatibility(GenerationMode mode, string format)
    {
        var formatLower = format.ToLowerInvariant();

        // All mode only supports JSON
        if (mode == GenerationMode.All && formatLower != "json")
        {
            throw new NotSupportedException(
                $"Mode 'all' only supports JSON format. Requested format: {format}");
        }

        // MCP mode only supports JSON
        if (mode == GenerationMode.Mcp && formatLower != "json")
        {
            throw new NotSupportedException(
                $"Mode 'mcp' only supports JSON format. Requested format: {format}");
        }

        // Icons and Emojis modes only support JSON
        if ((mode == GenerationMode.Icons || mode == GenerationMode.Emojis) && formatLower != "json")
        {
            throw new NotSupportedException(
                $"Mode '{mode}' only supports JSON format. Requested format: {format}");
        }
    }

    private static IReadOnlyList<DocumentationInput> CreateDocumentationInputs(string dllFiles, string xmlFiles)
    {
        var dllPaths = SplitInputValues(dllFiles)
            .Select(MakeAbsolutePath)
            .ToArray();

        var xmlPaths = SplitInputValues(xmlFiles)
            .Select(MakeAbsolutePath)
            .ToArray();

        if (dllPaths.Length != xmlPaths.Length)
        {
            throw new ArgumentException($"The number of DLL files ({dllPaths.Length}) must match the number of XML files ({xmlPaths.Length}).");
        }

        var loadContext = new DocumentationAssemblyLoadContext(dllPaths);

        return dllPaths
            .Select((dllPath, index) =>
            {
                Console.WriteLine($"Attempting to load assembly: {dllPath}");

                try
                {
                    var assembly = loadContext.LoadFromAssemblyPath(dllPath);

                    Console.WriteLine($"  Loaded input assembly: {assembly.GetName().Name}");
                    Console.WriteLine($"    Requested path: {dllPath}");
                    Console.WriteLine($"    FullName: {assembly.FullName}");
                    Console.WriteLine($"    Location: {assembly.Location}");

                    return new DocumentationInput(assembly, new FileInfo(xmlPaths[index]));
                }
                catch (FileLoadException ex)
                {
                    Console.WriteLine($"  Failed to load assembly: {dllPath}");
                    Console.WriteLine($"  {ex.Message}");
                    throw;
                }
            })
            .ToArray();
    }

    private static IEnumerable<string> SplitInputValues(string value)
    {
        return value
            .Split(InputSeparators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Where(static path => !string.IsNullOrWhiteSpace(path));
    }
    private static string MakeAbsolutePath(string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.Combine(Directory.GetCurrentDirectory(), path);
    }
}
