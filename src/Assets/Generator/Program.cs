namespace Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator;

internal class Program
{
    public static void Main(string[] args)
    {
        var configuration = new Configuration(args);

        // Help
        if (configuration.Help)
        {
            Configuration.DisplayHelp();
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

        switch (configuration.Library)
        {
            // *** Icons ***
            case "icon":

                // Initialize the factory
                var factoryIcons = new IconsCodeGenerator(configuration)
                {
                    Logger = (message) => Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} - {message}")
                };

                // Start the generation
                var assets = factoryIcons.ReadAllAssets()
                                         // All names (if not specified) or Only specified names
                                         .Where(i => configuration.Names.Any() == false ||
                                                     configuration.Names.Any(name => string.Compare(name.Replace("_", string.Empty), i.Key.Replace("_", string.Empty), StringComparison.InvariantCultureIgnoreCase) == 0))
                                         // All sizes (if not specified) or Only specified sizes
                                         .Where(i => configuration.Sizes.Any() == false ||
                                                     configuration.Sizes.Contains(i.Size));

                if (configuration.Names.Any())
                {
                    factoryIcons.GenerateOneClass(assets, "CoreIcons");
                }
                else
                {
                    //factoryIcons.GenerateMainIconsClass(assets);
                    factoryIcons.GenerateClasses(assets);
                }

                break;

            // *** Emojis ***
            case "emoji":

                // Initialize the factory
                var factoryEmojis = new EmojisCodeGenerator(configuration)
                {
                    Logger = (message) => Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} - {message}")
                };

                // Start the generation
                var emojis = factoryEmojis.ReadAllAssets()
                                          // All names (if not specified) or Only specified names
                                          .Where(i => configuration.Names.Any() == false ||
                                                     configuration.Names.Any(name => string.Compare(name + ".svg", i.File.Name, StringComparison.InvariantCultureIgnoreCase) == 0));

                // Remove the duplicates
                var duplicateBoy = emojis.FirstOrDefault(i => i.File.Name == "boy_high_contrast_default.svg");
                if (duplicateBoy != null)
                {
                    emojis.ToList().Remove(duplicateBoy);
                }

                factoryEmojis.GenerateClasses(emojis);
                factoryEmojis.GenerateMainEmojisClass(emojis);

                break;

        }

        // Sample
        // var x = Icons.Regular.Size24.Accessibility;
    }
}
