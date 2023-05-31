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
        var factory = new CodeGenerator(configuration)
        {
            Logger = (message) => Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} - {message}")
        };

        // Start the generation
        var assets = factory.ReadAllAssets()
                            .Where(i => configuration.Sizes.Contains(i.Size)); // Only specified sizes
        
        factory.GenerateMainIconsClass(assets);
        factory.GenerateClasses(assets);

        // Sample
        // var x = Icons.Regular.Size24.Accessibility;
    }
}