using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Microsoft.Fast.Components.FluentUI.IconsGenerator.Model;

/// <summary>
/// Emoji model.
/// </summary>
[DebuggerDisplay("{Name} [{Group}] - {Count} files")]
internal class Emoji
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Convert the "metadata.json" file to an Emoji
    /// </summary>
    /// <param name="metaDataFile"></param>
    public Emoji(FileInfo metaDataFile)
    {
        MetaFile = metaDataFile;

        string content = System.IO.File.ReadAllText(metaDataFile.FullName);
        Meta = JsonSerializer.Deserialize<EmojiMetaData>(content, JsonOptions)!;

        Name = Tools.ToPascalCase(Meta.Cldr);
        Group = Tools.ToPascalCase(Meta.Group);
        Keywords = Meta.Keywords;
    }

    public EmojiMetaData Meta { get; } = new EmojiMetaData();

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
    public FileInfo MetaFile { get; }

    /// <summary>
    /// Folder structure: [Name]/[SkinTone]/[Style] - Ex. Artist/Default/Color
    ///                   [Name]/[Style]            - Ex. Accordion/High Contrast
    /// </summary>
    public EmojiFileData[] Files
    {
        get
        {
            var files = MetaFile.Directory!.GetFiles("*.svg", SearchOption.AllDirectories);
            var hasSkinTone = files.Length > 4;

            return files.Select(i => new EmojiFileData(i, hasSkinTone)).ToArray();
        }
    }

    /// <summary />
    private int Count => Files.Count();
}
