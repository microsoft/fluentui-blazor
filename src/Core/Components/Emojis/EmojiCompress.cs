using System.IO.Compression;
using System.Text;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class EmojiCompress
{
    /// <summary>
    /// Compresses a character string into an array of bytes.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] Zip(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);

        using MemoryStream? msi = new(bytes);
        using MemoryStream? mso = new();
        using (GZipStream? gs = new(mso, CompressionMode.Compress))
        {
            CopyTo(msi, gs);
        }

        return mso.ToArray();
    }

    /// <summary>
    /// Decompression of an array of bytes into a character string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Unzip(byte[] data)
    {
        using MemoryStream? msi = new(data);
        using MemoryStream? mso = new();
        using (GZipStream? gs = new(msi, CompressionMode.Decompress))
        {
            CopyTo(gs, mso);
        }

        return Encoding.UTF8.GetString(mso.ToArray());
    }

    /// <summary />
    private static void CopyTo(Stream src, Stream dest)
    {
        var bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

}
