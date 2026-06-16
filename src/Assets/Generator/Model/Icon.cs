using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator.Model;

/// <summary>
/// Icon model.
/// </summary>
[DebuggerDisplay("{Name} {Variant} {Size}")]
internal class Icon
{
    private readonly string[] LangageSubFolders = new[] { "bg", "ca", "da", "de", "en", "es", "et", "fr", "gl", "hu", "it", "kk", "ko", "lt", "lv", "ms", "no", "pt", "ru", "sl", "sr-cyrl", "sr-latn", "sv" };

    /// <summary>
    /// Convert the file to an icon
    /// Examples:
    ///   "add_circle_20_regular.svg" => "AddCircle", 20, "Regular"
    ///   "fr/text_bold_24_filled.svg" => "TextBoldFr", 24, "Filled"
    /// </summary>
    /// <param name="file"></param>
    public Icon(FileInfo file)
    {
        File = file;

        string filename = Path.GetFileNameWithoutExtension(file.Name);
        string[] nameParts = filename.Split(Tools.InvalidCharacters);
        string folderName = file.Directory!.Name; // Check if the SVG is included in a "language" folder.

        // This file is in a language folder?
        if (!LangageSubFolders.Any(language => string.Compare(folderName, language, StringComparison.CurrentCultureIgnoreCase) == 0))
        {
            folderName = string.Empty;
        }

        if (nameParts.Length >= 3)
        {
            Variant = Tools.ToPascalCase(nameParts[^1]);
            Size = int.Parse(nameParts[^2]);
            Name = Tools.ToPascalCase(nameParts[..^2].Union(folderName.Split(Tools.InvalidCharacters)).ToArray());
        }
    }

    /// <summary>
    /// Gets the file info for the icon
    /// </summary>
    public FileInfo File { get; }

    /// <summary>
    /// Gets the name of the icon: AddCircle, AddCircleFilled, etc.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Gets the size of the icon: 20, 24, etc.
    /// </summary>
    public int Size { get; set; } = 0;

    /// <summary>
    /// Gets the variant of the icon: Filled, Regular, etc.
    /// </summary>
    public string Variant { get; } = string.Empty;

    /// <summary>
    /// Gets the key of the icon: Regular_20_AddCircle
    /// </summary>
    public string Key => $"{Name}_{Size}_{Variant}";

    /// <summary>
    /// Returns the SVG content of the icon, with or without the root SVG element.
    /// </summary>
    /// <param name="removeSvgRoot"></param>
    /// <returns></returns>
    public string GetContent(bool removeSvgRoot)
    {
        var content = System.IO.File.ReadAllText(File.FullName);

        if (!removeSvgRoot)
        {
            return content;
        }

        string pattern = @"<svg\swidth=""\d+""\sheight=""\d+""\sviewBox=""0\s0\s\d+\s\d+""(?:\sfill=""\w+"")?\sxmlns=""http:\/\/www\.w3\.org\/2000\/svg"">";
        return Regex.Replace(content, pattern, string.Empty)
                    .Replace("</svg>", "")
                    .Replace("\n", "")
                    .Replace("\r", "");
    }
}
