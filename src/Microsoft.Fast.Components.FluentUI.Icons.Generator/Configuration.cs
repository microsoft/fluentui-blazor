using Microsoft.Extensions.Configuration;

namespace Microsoft.Fast.Components.FluentUI.Icons.Generator;

/// <summary>
/// Configuration for the generator.
/// </summary>
internal class Configuration
{
    private const string DefaultNamespace = "Microsoft.Fast.Components.FluentUI.Icons";
    private const string DefaultSizes = "16,24,32";

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
        };

        var config = new ConfigurationBuilder().AddCommandLine(args, switchMappings)
                                               .Build();

        AssetsFolder = new DirectoryInfo(config.GetSection("assets").Value ?? Directory.GetCurrentDirectory());
        TargetFolder = new DirectoryInfo(config.GetSection("target").Value ?? Directory.GetCurrentDirectory());
        Namespace = config.GetSection("namespace").Value ?? DefaultNamespace;
        Sizes = (config.GetSection("sizes").Value ?? DefaultSizes).Split(",").Select(i => Convert.ToInt32(i));
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

    public IEnumerable<int> Sizes { get; set; } = DefaultSizes.Split(",").Select(i => Convert.ToInt32(i));

    /// <summary>
    /// Gets a value indicating whether the help documentation should be displayed.
    /// </summary>
    public bool Help { get; } = false;

    /// <summary>
    /// Displays the help documentation.
    /// </summary>
    public void DisplayHelp()
    {
        Console.WriteLine("Microsoft FluentUI Icons Generator");
        Console.WriteLine();
        Console.WriteLine("  FluentIconsGenerator --folder:<Icons_Folder_Directory>");
        Console.WriteLine();
        Console.WriteLine("  --Assets    | -a   The root directory containing all SVG icons,");
        Console.WriteLine("                     downloaded from https://github.com/microsoft/fluentui-system-icons.");
        Console.WriteLine("                     If not specified, the current working directory will be used.");
        Console.WriteLine();
        Console.WriteLine("  --Target    | -t   The target directory where C# classes will be created.");
        Console.WriteLine("                     If not specified, the current working directory will be used.");
        Console.WriteLine();
        Console.WriteLine("  --Namespace | -ns  The namespace used for generated classes.");
        Console.WriteLine("                     If not specified, \"Microsoft.Fast.Components.FluentUI.Icons\" will be used.");
        Console.WriteLine();
        Console.WriteLine("  --Help      | -h   Display this documentation.");
    }

    /// <summary>
    /// Gets a value indicating whether the help documentation should be displayed.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private bool HasHelpCommand(string[] args)
    {
        return args.Any(i => string.Compare(i, "-h", StringComparison.OrdinalIgnoreCase) == 0
                          || string.Compare(i, "--help", StringComparison.OrdinalIgnoreCase) == 0
                          || string.Compare(i, "/h", StringComparison.OrdinalIgnoreCase) == 0
                          || string.Compare(i, "/?", StringComparison.OrdinalIgnoreCase) == 0);
    }
}
