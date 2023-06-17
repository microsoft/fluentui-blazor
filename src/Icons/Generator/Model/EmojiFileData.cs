using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Microsoft.Fast.Components.FluentUI.IconsGenerator.Model;

/// <summary />
[DebuggerDisplay("{File.Name} / {SkinTone} / {Style}")]
public class EmojiFileData
{
    /// <summary />
    public EmojiFileData(FileInfo file, bool hasSkinTone)
    {
        File = file;

        // [Name]/[SkinTone]/[Style]
        if (hasSkinTone)
        {
            SkinTone = file.Directory!.Parent!.Name;
            Style = file.Directory!.Name;
        }

        // [Name]/[Style]
        else
        {
            SkinTone = string.Empty;
            Style = file.Directory!.Name;
        }
    }

    /// <summary />
    public FileInfo File { get; }

    /// <summary />
    public string SkinTone { get; }

    /// <summary />
    public string Style { get; }

    /// <summary>
    /// Returns the SVG content of the emoji, with or without the root SVG element.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="removeSvgRoot"></param>
    /// <returns></returns>
    public (string Content, Size Size) GetContent(bool removeSvgRoot = true)
    {
        var content = System.IO.File.ReadAllText(File.FullName);
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
}
