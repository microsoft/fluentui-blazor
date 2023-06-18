using System.Linq;
using Microsoft.Fast.Components.FluentUI.IconsGenerator.Model;

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
                                                     configuration.Names.Any(name => String.Compare(name.Replace("_", string.Empty), i.Key.Replace("_", string.Empty), StringComparison.InvariantCultureIgnoreCase) == 0))
                                         // All sizes (if not specified) or Only specified sizes
                                         .Where(i => configuration.Sizes.Any() == false ||
                                                     configuration.Sizes.Contains(i.Size));

                if (configuration.Names.Any())
                {
                    factoryIcons.GenerateOneClass(assets, "CoreIcons");
                }
                else
                {
                    factoryIcons.GenerateMainIconsClass(assets);
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
                                          .Where(i => i.Style == "Color"); // && i.SkinTone == "Dark" && i.Emoji.Name == "Artist");

                factoryEmojis.GenerateClasses(emojis);

                break;

        }

        // Sample
        // var x = Icons.Regular.Size24.Accessibility;
    }
}