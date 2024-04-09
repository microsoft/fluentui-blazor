using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator;

/// <summary>
/// Configuration for the generator.
/// </summary>
internal class Configuration
{
    private const string DefaultNamespace = "Microsoft.FluentUI.AspNetCore.Components";
    private const string DefaultLibrary = "icon";
    private const string DefaultSizes = ""; // All sizes

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration"/> class.
    /// </summary>
    /// <param name="args"></param>
    internal Configuration(string[] args)
    {
        if (HasHelpCommand(args))
        {
            Help = true;
            return;
        }

        var switchMappings = new Dictionary<string, string>()
        {
            { "-a", "assets" },
            { "--assets", "assets" },
            { "-t", "target" },
            { "--target", "target" },
            { "-ns", "namespace" },
            { "--namespace", "namespace" },
            { "-s", "sizes" },
            { "--sizes", "sizes" },
            { "-n", "names" },
            { "--names", "names" },
            { "-l", "library" },
            { "--library", "library" },
        };

        var config = new ConfigurationBuilder().AddCommandLine(args, switchMappings)
                                               .Build();

        AssetsFolder = GetAbsoluteFolder(config.GetSection("assets").Value);
        TargetFolder = GetAbsoluteFolder(config.GetSection("target").Value);
        Namespace = config.GetSection("namespace").Value ?? DefaultNamespace;
        Sizes = (config.GetSection("sizes").Value ?? DefaultSizes).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToInt32(i));
        Names = (config.GetSection("names").Value ?? string.Empty).Split(",", StringSplitOptions.RemoveEmptyEntries);
        Library = config.GetSection("library").Value?.ToLower() ?? DefaultLibrary;
    }

    /// <summary>
    /// Gets the root directory containing all SVG icons.
    /// </summary>
    public DirectoryInfo AssetsFolder { get; } = new DirectoryInfo(Directory.GetCurrentDirectory());

    /// <summary>
    /// Gets the target directory where C# classes will be created.
    /// </summary>
    public DirectoryInfo TargetFolder { get; } = new DirectoryInfo(Directory.GetCurrentDirectory());

    /// <summary>
    /// Gets the namespace used for generated classes.
    /// </summary>
    public string Namespace { get; set; } = DefaultNamespace;

    /// <summary>
    /// Gets the list of icon sizes to generate.
    /// </summary>
    public IEnumerable<int> Sizes { get; set; } = Array.Empty<int>();

    /// <summary>
    /// Gets a value indicating whether the files should be generated.
    /// </summary>
    public IEnumerable<string> Names { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets a value indicating which files should be generated: icon or emoji.
    /// </summary>
    public string Library { get; set; } = DefaultLibrary;

    /// <summary>
    /// Gets a value indicating whether the help documentation should be displayed.
    /// </summary>
    public bool Help { get; } = false;

    /// <summary>
    /// Displays the help documentation.
    /// </summary>
    public static void DisplayHelp()
    {
        Console.WriteLine($"Microsoft FluentUI Icons Generator");
        Console.WriteLine();
        Console.WriteLine($"  FluentAssetsGenerator --folder:<Icons_Folder_Directory>");
        Console.WriteLine();
        Console.WriteLine($"  --Assets    | -a   The root directory containing all SVG icons,");
        Console.WriteLine($"                     downloaded from https://github.com/microsoft/fluentui-system-icons.");
        Console.WriteLine($"                     If not specified, the current working directory will be used.");
        Console.WriteLine();
        Console.WriteLine($"  --Library   | -l   The type of library to generate: icon or emoji.");
        Console.WriteLine($"                     If not specified, \"{DefaultLibrary}\" will be used.");
        Console.WriteLine();
        Console.WriteLine($"  --Namespace | -ns  The namespace used for generated classes.");
        Console.WriteLine($"                     If not specified, \"{DefaultNamespace}\" will be used.");
        Console.WriteLine();
        Console.WriteLine($"  --Names     | -n   The list of icon names to generate, separated by coma.");
        Console.WriteLine($"                     Example of icons: accessibility_32_filled,add_circle_20_filled");
        Console.WriteLine($"                     Example of emojis: accordion_flat,ambulance_high_contrast");
        Console.WriteLine($"                     By default: all icons");
        Console.WriteLine();
        Console.WriteLine($"  --Sizes     | -s   The list of icon sizes to generate, separated by coma.");
        Console.WriteLine($"                     Example: 12,24. By default: all sizes");
        Console.WriteLine($"                     (Not available for emoji library)");
        Console.WriteLine();
        Console.WriteLine($"  --Target    | -t   The target directory where C# classes will be created.");
        Console.WriteLine($"                     If not specified, the current working directory will be used.");
        Console.WriteLine();
        Console.WriteLine($"  --Help      | -h   Display this documentation.");
    }

    /// <summary>
    /// Gets a value indicating whether the help documentation should be displayed.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static bool HasHelpCommand(string[] args)
    {
        return args.Any(i => string.Compare(i, "-h", StringComparison.OrdinalIgnoreCase) == 0
                          || string.Compare(i, "--help", StringComparison.OrdinalIgnoreCase) == 0
                          || string.Compare(i, "/h", StringComparison.OrdinalIgnoreCase) == 0
                          || string.Compare(i, "/?", StringComparison.OrdinalIgnoreCase) == 0);
    }

    private static DirectoryInfo GetAbsoluteFolder(string? folder)
    {
        string currentPath = GetThisFilePath();
        string path = folder ?? currentPath;

        if (Path.IsPathRooted(path))
        {
            return new DirectoryInfo(path);
        }

        return new DirectoryInfo(Path.Combine(currentPath, path));
    }

    /// <summary>
    /// Gets the path of the current file.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string GetThisFilePath([CallerFilePath] string? path = null)
    {
        if (Debugger.IsAttached)
        {
            return Path.GetDirectoryName(path) ?? Directory.GetCurrentDirectory();
        }

        return Directory.GetCurrentDirectory();
    }
}
