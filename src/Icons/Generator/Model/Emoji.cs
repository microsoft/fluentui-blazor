using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Microsoft.Fast.Components.FluentUI.IconsGenerator.Model;

/// <summary>
/// Emoji model.
/// </summary>
[DebuggerDisplay("{Name} [{Group}]")]
internal class Emoji
{
    /// <summary>
    /// List of categories
    /// </summary>
    public static readonly string[] Categories = new[]
    {
        "Color",
        "Flat",
        "High Contrast",
    };

    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Convert the "metadata.json" file to an Emoji
    /// </summary>
    /// <param name="file"></param>
    public Emoji(FileInfo file)
    {
        File = file;

        string content = System.IO.File.ReadAllText(file.FullName);
        Meta = JsonSerializer.Deserialize<MetaData>(content, JsonOptions)!;

        Name = Tools.ToPascalCase(Meta.Cldr);
        Group = Tools.ToPascalCase(Meta.Group);
        Keywords = Meta.Keywords;
    }

    public MetaData Meta { get; } = new MetaData();

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

    /// <summary>
    /// Returns the SVG content of the emoji, with or without the root SVG element.
    /// </summary>
    /// <param name="category">Category from <see cref="Emoji.Categories"/></param>
    /// <param name="removeSvgRoot"></param>
    /// <returns></returns>
    public (string Content, Size Size) GetContent(string category, bool removeSvgRoot = true)
    {
        var path = Path.Combine(File.Directory!.FullName, category);
        var filename = Directory.GetFiles(path, "*.svg").FirstOrDefault() ?? "FileNotFound";
        var content = System.IO.File.ReadAllText(filename);
        var size = new Size();

        string viewboxRegex = @"viewBox=""([^""]+)""";

        Match match = Regex.Match(content, viewboxRegex);
        if (match.Success)
        {
            var sizeValues = match.Groups[1].Value.Split(' ');
            if (sizeValues.Length > 3)
            {
                size = new Size(int.Parse(sizeValues[2]), int.Parse(sizeValues[3]));
            }
        }

        if (!removeSvgRoot)
        {
            return (content, size);
        }

        string pattern = @"<svg\swidth=""\d+""\sheight=""\d+""\sviewBox=""0\s0\s\d+\s\d+""(?:\sfill=""\w+"")?\sxmlns=""http:\/\/www\.w3\.org\/2000\/svg"">";
        return (
                Regex.Replace(content, pattern, string.Empty)
                     .Replace("</svg>", "")
                     .Replace("\n", "")
                     .Replace("\r", ""),
                size
                );
    }

    public class MetaData
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
