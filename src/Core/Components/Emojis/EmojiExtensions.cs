// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static partial class EmojiExtensions
{
    private const string LibraryName = "Microsoft.FluentUI.AspNetCore.Components.Emojis";

    /// <summary>
    /// Returns a new instance of the emoji.
    /// </summary>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="EmojiInfo.Name"/> is not found in predefined emojis.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static IEnumerable<EmojiInfo> GetAllEmojis()
    {
        var assembly = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .FirstOrDefault(i => i.ManifestModule.Name.StartsWith(LibraryName, StringComparison.Ordinal));

        if (assembly != null)
        {
            var allTypes = assembly.GetTypes()
                                   .Where(i => i.BaseType == typeof(Emoji));

            var allEmojis = allTypes.Select(type => Activator.CreateInstance(type) as EmojiInfo ?? new EmojiInfo());

            return allEmojis ?? Array.Empty<EmojiInfo>();
        }

        return Array.Empty<EmojiInfo>();
    }

    /// <summary />
    public static IEnumerable<EmojiInfo> AllEmojis
    {
        get
        {
            return GetAllEmojis();
        }
    }
}

