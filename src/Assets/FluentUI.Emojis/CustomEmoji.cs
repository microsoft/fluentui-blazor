using System.IO.Compression;
using System.Text;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Custom emoji loaded from <see cref="Emojis.GetInstance(Microsoft.FluentUI.AspNetCore.Components.EmojiInfo)"/>
/// </summary>
public class CustomEmoji : Emoji
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomEmoji"/> class.
    /// </summary>
    public CustomEmoji()
        : base(string.Empty, EmojiSize.Size32, EmojiGroup.Objects, EmojiSkintone.Default, EmojiStyle.Color, Array.Empty<byte>())
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomEmoji"/> class.
    /// </summary>
    /// <param name="emoji"></param>
    public CustomEmoji(Emoji emoji)
        : base(emoji.Name, emoji.Size, emoji.Group, emoji.Skintone, emoji.Style, Zip(emoji.Content))
    { }

    /// <summary />
    private static byte[] Zip(string str)
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
