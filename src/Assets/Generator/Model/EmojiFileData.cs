using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator.Model;

/// <summary />
[DebuggerDisplay("{File.Name} / {SkinTone} / {Style}")]
public class EmojiFileData
{
    private const string DefaultSkinTone = "Default";

    /// <summary />
    internal EmojiFileData(Emoji emoji, FileInfo file, bool hasSkinTone)
    {
        Emoji = emoji;
        File = file;

        // [Name]/[SkinTone]/[Style]
        if (hasSkinTone)
        {
            SkinTone = Tools.ToPascalCase(file.Directory!.Parent!.Name);
            Style = Tools.ToPascalCase(file.Directory!.Name);
        }

        // [Name]/[Style]
        else
        {
            SkinTone = DefaultSkinTone;
            Style = Tools.ToPascalCase(file.Directory!.Name);
        }
    }

    /// <summary />
    internal Emoji Emoji { get; }

    /// <summary />
    public FileInfo File { get; }

    /// <summary />
    public string SkinTone { get; }

    /// <summary />
    public string Style { get; }

    /// <summary>
    /// Gets the Emoji Size, only after calling <see cref="GetContent(bool)"/>.
    /// </summary>
    public int GetSizeWidth()
    {
        var content = System.IO.File.ReadAllText(File.FullName);
        return GetSize(content).Width;
}

/// <summary>
/// Returns the SVG content of the emoji, with or without the root SVG element.
/// </summary>
/// <param name="file"></param>
/// <param name="removeSvgRoot"></param>
/// <returns></returns>
public (string Content, Size Size) GetContent(bool removeSvgRoot = true)
    {
        var content = System.IO.File.ReadAllText(File.FullName);
        var size = GetSize(content);

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

    private Size GetSize(string content)
    {
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

            if (size.Width == 33 || size.Height == 33)
            {
                size = new Size(32, 32);
            }
        }

        return size;
    }
}
