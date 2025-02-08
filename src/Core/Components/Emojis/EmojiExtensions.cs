// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static partial class EmojiExtensions
{
    private const string Namespace = "Microsoft.FluentUI.AspNetCore.Components";
    private const string LibraryName = "Microsoft.FluentUI.AspNetCore.Components.Emojis.{0}";    // {0} must be replaced with the "Group": SmileysEmotion, PeopleBody, etc.

    /// <summary>
    /// Returns a new instance of the emoji.
    /// </summary>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// If the assembly is not yet loaded, it will be loaded by the method `Assembly.Load`.
    /// To avoid any issues, the assembly must be loaded before calling this method (e.g. adding an emoji in your code).
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="EmojiInfo.Name"/> is not found in predefined emojis.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static CustomEmoji GetInstance(this EmojiInfo emoji)
    {
        var group = emoji.Group.ToString().Replace("_", string.Empty);
        var assemblyName = string.Format(LibraryName, group);
        var assembly = GetAssembly(assemblyName);

        if (assembly != null)
        {
            var allEmojis = assembly.GetTypes()
                                    .Where(i => i.BaseType == typeof(Emoji));

            // Ex. Microsoft.FluentUI.AspNetCore.Components.Emojis.Activities.Color.Default+Baseball
            var emojiFullName = $"{Namespace}.Emojis.{group}.{emoji.Style}.{emoji.Skintone}+{emoji.Name}";
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

        throw new ArgumentException($"Emoji 'Emojis.{group}.{emoji.Style}.{emoji.Skintone}.{emoji.Name}' not found.");
    }

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
        var allIcons = new List<EmojiInfo>();

        foreach (var group in Enum.GetValues(typeof(EmojiGroup)).Cast<EmojiGroup>())
        {
            var assemblyName = string.Format(LibraryName, group.ToString().Replace("_", string.Empty));
            var assembly = GetAssembly(assemblyName);

            if (assembly != null)
            {
                var allTypes = assembly.GetTypes()
                                       .Where(i => i.BaseType == typeof(Emoji)
                                                && i.Name != nameof(CustomEmoji));

                allIcons.AddRange(allTypes.Select(type => Activator.CreateInstance(type) as EmojiInfo ?? new EmojiInfo()));
            }
        }

        return allIcons;
    }

    /// <summary />
    public static IEnumerable<EmojiInfo> AllEmojis
    {
        get
        {
            return GetAllEmojis();
        }
    }

    /// <summary />
    private static Assembly? GetAssembly(string assemblyName)
    {
        try
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .FirstOrDefault(i => i.ManifestModule.Name == assemblyName + ".dll")
                ?? Assembly.Load(assemblyName);

        }
        catch (Exception)
        {
            return null;
        }
    }
}
