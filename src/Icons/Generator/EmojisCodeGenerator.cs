namespace Microsoft.Fast.Components.FluentUI.IconsGenerator;

internal class EmojisCodeGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmojisCodeGenerator"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public EmojisCodeGenerator(Configuration configuration)
    {
        Configuration = configuration;
        Logger = (message) => { };
    }

    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    public Action<string> Logger { get; init; }

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    private Configuration Configuration { get; }

    /// <summary>
    /// Reads all SVG files in the assets folder.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Model.Emoji> ReadAllAssets()
    {
        const string searchPattern = "metadata.json";
        var emojis = new Dictionary<string, Model.Emoji>();

        Logger.Invoke($"Reading all metadata.json files in {Configuration.AssetsFolder}.");
        var allFiles = Configuration.AssetsFolder.GetFiles(searchPattern, SearchOption.AllDirectories);

        foreach (var file in allFiles)
        {
            var newEmoji = new Model.Emoji(file);
            var key = newEmoji.Key.ToLower();

            if (!emojis.ContainsKey(key))
            {
                emojis.Add(newEmoji.Key.ToLower(), newEmoji);
            }
        }

        return emojis.Values
                     .OrderBy(i => i.Group)
                     .ThenBy(i => i.Name)
                     .ToArray();
    }
}
