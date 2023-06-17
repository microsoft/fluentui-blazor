namespace Microsoft.Fast.Components.FluentUI.IconsGenerator;

internal static class Tools
{
    /// <summary />
    public static readonly char[] InvalidCharacters = new[] { '_', ' ', '-', '.', ',', '&' };

    /// <summary />
    public static string ToPascalCase(string value)
    {
        return ToPascalCase(value.Split(InvalidCharacters));
    }

    /// <summary />
    public static string ToPascalCase(string[] words)
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
