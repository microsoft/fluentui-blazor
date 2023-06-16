namespace Microsoft.Fast.Components.FluentUI.IconsGenerator;

internal class Program
{
    static void Main(string[] args)
    {
        var configuration = new Configuration(args);

        // Help
        if (configuration.Help)
        {
            configuration.DisplayHelp();
            return;
        }

        // Validate the configuration
        if (!configuration.AssetsFolder.Exists)
        {
            Console.WriteLine($"Error: The assets folder '{configuration.AssetsFolder.FullName}' does not exist.");
            return;
        }

        if (!configuration.TargetFolder.Exists)
        {
            configuration.TargetFolder.Create();
        }

        // Initialize the factory
        var factory = new IconsCodeGenerator(configuration)
        {
            Logger = (message) => Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} - {message}")
        };

        // Start the generation
        var assets = factory.ReadAllAssets()
                            // All names (if not specified) or Only specified names
                            .Where(i => configuration.Names.Any() == false ||
                                        configuration.Names.Any(name => String.Compare(name.Replace("_", string.Empty), i.Key.Replace("_", string.Empty), StringComparison.InvariantCultureIgnoreCase) == 0))
                            // All sizes (if not specified) or Only specified sizes
                            .Where(i => configuration.Sizes.Any() == false ||
                                        configuration.Sizes.Contains(i.Size));

        factory.GenerateMainIconsClass(assets);
        factory.GenerateClasses(assets);

        // Sample
        // var x = Icons.Regular.Size24.Accessibility;
    }
}