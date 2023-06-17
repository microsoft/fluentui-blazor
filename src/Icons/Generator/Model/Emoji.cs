using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace Microsoft.Fast.Components.FluentUI.IconsGenerator.Model;

/// <summary>
/// Emoji model.
/// </summary>
[DebuggerDisplay("{Name} {Variant} {Size}")]
internal class Emoji
{
    /// <summary>
    /// Convert the "metadata.json" file to an Emoji
    /// </summary>
    /// <param name="file"></param>
    public Emoji(FileInfo file)
    {
        File = file;

        string content = System.IO.File.ReadAllText(file.FullName);
        var metaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(content)!;

        Name = Tools.ToPascalCase(metaData.Cldr);
        Group = Tools.ToPascalCase(metaData.Group);
        Keywords = metaData.Keywords;
    }

    /// <summary>
    /// Gets the name of the emoji.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Gets the name of the emoji.
    /// </summary>
    public string Group { get; } = string.Empty;

    /// <summary>
    /// Gets the name of the emoji.
    /// </summary>
    public string[] Keywords { get; } = Array.Empty<string>();

    /// <summary>
    /// Gets the key of the icon: Regular_20_AddCircle
    /// </summary>
    public string Key => $"{Group}_{Name}";

    /// <summary>
    /// Gets the file info for the emoji
    /// </summary>
    public FileInfo File { get; }

    private class MetaData
    {
        /// <summary />
        public string Cldr { get; set; } = string.Empty;

        /// <summary />
        public string Group { get; set; } = string.Empty;

        /// <summary />
        public string[] Keywords { get; set; } = Array.Empty<string>();

        /// <summary />
        public string Tts { get; set; } = string.Empty;
    }
}
