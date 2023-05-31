using System.Diagnostics;

namespace Microsoft.Fast.Components.FluentUI.Icons.Generator.Model;

/// <summary>
/// Icon model.
/// </summary>
[DebuggerDisplay("{Name} {Variant} {Size}")]
internal class Icon
{
    private char[] InvalidCharacters = new[] { '_', ' ', '-', '.', ',' };

    /// <summary>
    /// Convert the file to an icon
    /// Example: "ic_fluent_add_circle_20_regular.svg" => "AddCircle", 20, "Regular"
    /// </summary>
    /// <param name="file"></param>
    public Icon(FileInfo file)
    {
        File = file;

        string filename = Path.GetFileNameWithoutExtension(file.Name);
        string[] nameParts = filename.Split(InvalidCharacters);

        if (nameParts.Length >= 5)
        {
            Variant = ToPascalCase(nameParts[^1]);
            Size = int.Parse(nameParts[^2]);
            Name = GetIconName(file);
        }

        if (Name == "Ltr")
        {
            Debugger.Break();
        }

        // Gets the icon name from the parent folder
        // Because the icon name is not always the same as the file name
        // It's possible to have subfolders to identify the country (en, it, etc.) or LTR/RTL.
        string GetIconName(FileInfo file)
        {
            string parentName = file.Directory!.Parent!.Name;
            string parentParentName = file.Directory!.Parent!.Parent!.Name;
            
            string name = parentName.Length <= 3 ? $"{parentParentName} {parentName}" : parentName;
            return ToPascalCase(name.Split(InvalidCharacters));
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

        return content.Replace($"<svg width=\"{Size}\" height=\"{Size}\" viewBox=\"0 0 {Size} {Size}\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">", "")
                      .Replace($"<svg width=\"{Size}\" height=\"{Size}\" viewBox=\"0 0 {Size} {Size}\" xmlns=\"http://www.w3.org/2000/svg\">", "")
                      .Replace("</svg>", "")
                      .Replace("\n", "")
                      .Replace("\r", "");
    }

    /// <summary />
    private string ToPascalCase(params string[] words)
    {
        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            if (word.Length > 0)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
        }

        return string.Join(string.Empty, words);
    }
}
