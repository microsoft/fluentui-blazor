using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public static partial class Emojis
{
    private const string Namespace = "Microsoft.Fast.Components.FluentUI";
    private const string LibraryName = "Microsoft.Fast.Components.FluentUI.Emojis.dll";

    /// <summary>
    /// Returns a new instance of the emoji.
    /// </summary>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="EmojiInfo.Name"/> is not found in predefined emojis.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static CustomEmoji GetInstance(EmojiInfo emoji)
    {
        var assembly = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .FirstOrDefault(i => i.ManifestModule.Name == LibraryName);

        if (assembly != null)
        {
            var allEmojis = assembly.GetTypes()
                                    .Where(i => i.BaseType == typeof(Emoji));

            // Ex. Microsoft.Fast.Components.FluentUI.Emojis+Activities+Color+Default+Baseball
            var group = emoji.Group.ToString().Replace("_", string.Empty);
            var emojiFullName = $"{Namespace}.Emojis+{group}+{emoji.Style}+{emoji.Skintone}+{emoji.Name}";
            var emojiType = allEmojis.FirstOrDefault(i => i.FullName == emojiFullName);

            if (emojiType != null)
            {
                var newEmoji = Activator.CreateInstance(emojiType);
                if (newEmoji != null)
                {
                    return new CustomEmoji((Emoji)newEmoji);
                }
            }
        }

        throw new ArgumentException($"Emoji '{emoji.Name}' not found.");
    }
}
