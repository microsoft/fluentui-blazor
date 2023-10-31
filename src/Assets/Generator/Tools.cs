using System.IO.Compression;
using System.Text;

namespace Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator;

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

    /// <summary />
    public static byte[] Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }
    }

    /// <summary />
    public static string Unzip(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                CopyTo(gs, mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }

    /// <summary />
    private static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }
}
