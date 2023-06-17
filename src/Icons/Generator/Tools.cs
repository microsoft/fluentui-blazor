namespace Microsoft.Fast.Components.FluentUI.IconsGenerator;

internal static class Tools
{
    /// <summary />
    public static readonly char[] InvalidCharacters = new[] { '_', ' ', '-', '.', ',', '&', '’', '\'', ':', '(', ')', '“', '”', '"', '!' };

    /// <summary />
    public static string ToPascalCase(string value, string separator = "")
    {
        return ToPascalCase(value.Split(InvalidCharacters, StringSplitOptions.RemoveEmptyEntries), separator);
    }

    /// <summary />
    public static string ToPascalCase(string[] words, string separator = "")
    {
        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            if (word.Length > 0)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
        }

        return string.Join(separator, words);
    }
}
