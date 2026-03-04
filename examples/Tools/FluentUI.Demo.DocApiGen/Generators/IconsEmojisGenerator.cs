// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using FluentUI.Demo.DocApiGen.Abstractions;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Generates Summary mode documentation (only [Parameter] properties).
/// Supports JSON and C# output formats.
/// </summary>
public sealed class IconsEmojisGenerator : DocumentationGeneratorBase
{
    private readonly GenerationMode _mode;
    /// <summary>
    /// Initializes a new instance of the <see cref="IconsEmojisGenerator"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    /// <param name="mode">The generation mode (Icons or Emojis).</param>
    public IconsEmojisGenerator(Assembly assembly, FileInfo xmlDocumentation, GenerationMode mode)
        : base(assembly, xmlDocumentation)
    {
        _mode = mode;
    }

    /// <inheritdoc/>
    public override GenerationMode Mode => _mode;

    /// <summary>
    /// Generates documentation for icons or emojis based on the Mode property.
    /// </summary>
    /// <param name="formatter"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    /// <example>
    /// For Icons mode, the output JSON structure will be:
    /// {
    ///   "AddCircle": {
    ///     "Filled": [10, 12, 16, 20, 24, 28, 32, 48],
    ///     "Regular": [10, 12, 16, 20, 24, 28, 32, 48]
    ///   },
    ///   "Accessibility": {
    ///     "Filled": [16, 20, 24],
    ///     "Regular": [16, 20, 24]
    ///   }
    /// }
    /// 
    /// For Emojis mode, the output JSON structure will be:
    /// {
    ///   "GrinningFace": {
    ///     "Group": "Smileys_Emotion",
    ///     "Style": ["Color", "Flat"],
    ///     "Skintone": ["Default"],
    ///     "Size": [16, 20],
    ///     "KeyWords": "happy, smile, joy"
    ///   },
    ///   "ThumbsUp": {
    ///     "Group": "People_Body",
    ///     "Style": ["Color", "Flat"],
    ///     "Skintone": ["Default", "Light"],
    ///     "Size": [16, 20],
    ///     "KeyWords": "thumbs up, like, approve"
    ///   }
    /// }
    /// </example>
    public override string Generate(IOutputFormatter formatter)
    {
        switch (Mode)
        {
            case GenerationMode.Icons:
                var allIcons = Microsoft.FluentUI.AspNetCore.Components.IconsExtensions.GetAllIcons();

                // Group icons by Name -> Variant -> Sizes
                var iconStructure = allIcons
                    .GroupBy(icon => icon.Name)
                    .OrderBy(g => g.Key)
                    .ToDictionary(
                        nameGroup => nameGroup.Key,
                        nameGroup => nameGroup
                            .GroupBy(icon => icon.Variant.ToString())
                            .OrderBy(g => g.Key)
                            .ToDictionary(
                                variantGroup => variantGroup.Key,
                                variantGroup => variantGroup
                                    .Select(icon => (int)icon.Size)
                                    .Where(size => size > 0) // Exclude Custom (0)
                                    .OrderBy(size => size)
                                    .ToArray()
                            )
                    );

                return JsonSerializer.Serialize(iconStructure, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            case GenerationMode.Emojis:
                var allEmojis = Microsoft.FluentUI.AspNetCore.Components.EmojiExtensions.GetAllEmojis();

                // Group emojis by Name -> Style, Skintone, Size arrays
                var emojiStructure = allEmojis
                    .GroupBy(emoji => emoji.Name)
                    .OrderBy(g => g.Key)
                    .ToDictionary(
                        nameGroup => nameGroup.Key,
                        nameGroup => new
                        {
                            Group = nameGroup.First().Group.ToString(),
                            Style = nameGroup
                                .Select(e => e.Style.ToString())
                                .Distinct()
                                .OrderBy(s => s)
                                .ToArray(),
                            Skintone = nameGroup
                                .Select(e => e.Skintone.ToString())
                                .Distinct()
                                .OrderBy(s => s)
                                .ToArray(),
                            Size = nameGroup
                                .Select(e => (int)e.Size)
                                .Where(size => size > 0)
                                .Distinct()
                                .OrderBy(size => size)
                                .ToArray(),
                            KeyWords = nameGroup.First().KeyWords
                        }
                    );

                return JsonSerializer.Serialize(emojiStructure, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            default:
                throw new NotSupportedException($"Generation mode '{Mode}' is not supported by {nameof(IconsEmojisGenerator)}.");
        }
    }
}
