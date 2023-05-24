using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    public record StaticAssetsOptions()
    {
        public string[]? IconSizes { get; set; } = Array.Empty<string>();
        public string[]? IconVariants { get; set; } = Array.Empty<string>();
        public string[]? EmojiGroups { get; set; } = Array.Empty<string>();
        public string[]? EmojiStyles { get; set; } = Array.Empty<string>();

        public bool PublishEmojiAssets { get; set; } = false;
        public bool PublishIconAssets { get; set; } = false;
    }

    [Generator]
    public class ConfigurationGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValueProvider<AnalyzerConfigOptions> optionProvider = context.AnalyzerConfigOptionsProvider
                .Select((provider, _) => provider.GlobalOptions);

            context.RegisterSourceOutput(optionProvider, Execute);

        }

        public void Execute(SourceProductionContext context, AnalyzerConfigOptions options)
        {
            ReadGlobalOptions(options, out StaticAssetsOptions staticAssetsOptions);

            GenerateSource(context, staticAssetsOptions);
        }

        private void ReadGlobalOptions(AnalyzerConfigOptions options, out StaticAssetsOptions staticAssetsOptions)
        {
            bool publishIconAssets = false;
            bool publishEmojiAssets = false;

            string[]? iconSizes = Array.Empty<string>();
            string[]? iconVariants = Array.Empty<string>();
            string[]? emojiGroups = Array.Empty<string>();
            string[]? emojiStyles = Array.Empty<string>();

            if (TryReadGlobalOption(options, "PublishFluentIconAssets", out string? publishIconAssetsProp))
            {
                if (bool.TryParse(publishIconAssetsProp, out publishIconAssets))
                {
                    TryReadGlobalOption(options, "FluentIconSizes", out string? iconSizesProp);
                    iconSizes = iconSizesProp?.Split(',');
                    if (iconSizes?.Length == 1 && string.IsNullOrEmpty(iconSizes[0]))
                    {
                        iconSizes = new[]
                        {
                            "10",
                            "12",
                            "16",
                            "20",
                            "24",
                            "28",
                            "32",
                            "48"
                        };
                    }

                    TryReadGlobalOption(options, "FluentIconVariants", out var iconVariantsProp);
                    iconVariants = iconVariantsProp?.Split(',');
                    if (iconVariants?.Length == 1 && string.IsNullOrEmpty(iconVariants[0]))
                    {
                        iconVariants = new[]
                        {
                            "Filled",
                            "Regular"
                        };
                    }
                }
            }

            if (TryReadGlobalOption(options, "PublishFluentEmojiAssets", out string? publishedEmojiAssetsProp))
            {
                if (bool.TryParse(publishedEmojiAssetsProp, out publishEmojiAssets))
                {
                    TryReadGlobalOption(options, "FluentEmojiGroups", out var emojiGroupsProp);
                    emojiGroups = emojiGroupsProp?.Split(',');

                    if (emojiGroups?.Length == 1 && string.IsNullOrEmpty(emojiGroups[0]))
                    {
                        emojiGroups = new[]
                        {
                            "Activities",
                            "Animals_Nature",
                            "Flags",
                            "Food_Drink",
                            "Objects",
                            "People_Body",
                            "Smileys_Emotion",
                            "Symbols",
                            "Travel_Places"

                        };
                    }

                    TryReadGlobalOption(options, "FluentEmojiStyles", out var emojiStylesProp);
                    emojiStyles = emojiStylesProp?.Split(',');
                    if (emojiStyles?.Length == 1 && string.IsNullOrEmpty(emojiStyles[0]))
                    {
                        emojiStyles = new[]
                        {
                            "Color",
                            "Flat",
                            "HighContrast"
                        };
                    }
                }

            }
            staticAssetsOptions = new()
            {
                PublishIconAssets = publishIconAssets,
                IconSizes = iconSizes,
                IconVariants = iconVariants,

                PublishEmojiAssets = publishEmojiAssets,
                EmojiGroups = emojiGroups,
                EmojiStyles = emojiStyles,
            };
        }

        public bool TryReadGlobalOption(AnalyzerConfigOptions options, string property, out string? value)
        {
            return options.TryGetValue($"build_property.{property}", out value);
        }

        private void GenerateSource(SourceProductionContext context, StaticAssetsOptions options)
        {
            StringBuilder sb = new();
            sb.AppendLine($"namespace Microsoft.Fast.Components.FluentUI;");
            sb.AppendLine($"");
            sb.AppendLine("///<summary>");
            sb.AppendLine("/// This class contains the configuration for icons and emoji,");
            sb.AppendLine("/// generated from the settings in the project file.");
            sb.AppendLine("///</summary>");
            sb.AppendLine("public static class ConfigurationGenerator");
            sb.AppendLine("{");

            //Create IconConfiguration
            sb.AppendLine("\t///<summary>");
            sb.AppendLine("\t/// Returns the icon configuration.");
            sb.AppendLine("\t/// Generated from the settings in the project file.");
            sb.AppendLine("\t///</summary>");
            sb.AppendLine("\tpublic static IconConfiguration GetIconConfiguration()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tIconConfiguration config = new({options.PublishIconAssets.ToString().ToLower()});");
            if (options.PublishIconAssets)
            {
                FormatConfigSection(sb, "Sizes", "IconSize.Size", options.IconSizes);
                FormatConfigSection(sb, "Variants", "IconVariant.", options.IconVariants);
            }
            sb.AppendLine("\t\treturn config;");
            sb.AppendLine("\t}");


            //Create EmojiConfiguration
            sb.AppendLine("\t///<summary>");
            sb.AppendLine("\t/// Returns the emoji configuration.");
            sb.AppendLine("\t/// Geenerated from the settings in the project file.");
            sb.AppendLine("\t///</summary>");
            sb.AppendLine("\tpublic static EmojiConfiguration GetEmojiConfiguration()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tEmojiConfiguration config = new({options.PublishEmojiAssets.ToString().ToLower()});");
            if (options.PublishEmojiAssets)
            {
                FormatConfigSection(sb, "Groups", "EmojiGroup.", options.EmojiGroups);
                FormatConfigSection(sb, "Styles", "EmojiStyle.", options.EmojiStyles);
            }

            sb.AppendLine("\t\treturn config;");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            context.AddSource($"ConfiguratonGenerator.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static void FormatConfigSection(StringBuilder sb, string section, string identifier, string[]? options)
        {
            if (options is not null && options.Length > 0)
            {
                int max = options.Length - 1;
                sb.AppendLine($"\t\tconfig.{section} = new[] {{");
                for (int i = 0; i <= max; i++)
                {
                    if (!string.IsNullOrWhiteSpace(options[i]))
                    {
                        string option = options[i].Trim();
                        string endmarker = i <= max - 1 ? "," : string.Empty;
                        sb.AppendLine($"\t\t\t{identifier}{option}{endmarker}");
                    }
                }
                sb.AppendLine("\t\t};");
            }
        }
    }
}
