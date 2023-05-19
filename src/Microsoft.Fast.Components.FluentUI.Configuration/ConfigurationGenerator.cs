using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    [Generator]
    public class ConfigurationGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValueProvider<AnalyzerConfigOptions> optionProvider = context.AnalyzerConfigOptionsProvider
                .Select((provider, _) => provider.GlobalOptions);

            context.RegisterSourceOutput(optionProvider, GenerateSource);
        }

        public void GenerateSource(SourceProductionContext context, AnalyzerConfigOptions options)
        {
            StringBuilder sb = new();

            string[]? iconSizes = Array.Empty<string>();
            string[]? iconVariants = Array.Empty<string>();
            string[]? emojiGroups = Array.Empty<string>();
            string[]? emojiStyles = Array.Empty<string>();

            bool publishedEmojiAssets = false;
            bool publishedIconAssets = false;

            if (TryReadGlobalOption(options, "PublishFluentIconAssets", out string? publishedIconAssetsProp))
            {
                if (bool.TryParse(publishedIconAssetsProp, out publishedIconAssets))
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
                if (bool.TryParse(publishedEmojiAssetsProp, out publishedEmojiAssets))
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

            //sb.AppendLine($"using Microsoft.Extensions.DependencyInjection;");
            //sb.AppendLine($"");
            
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
            sb.AppendLine("\t/// Returns the icon configuration,");
            sb.AppendLine("\t/// generated from the settings in the project file.");
            sb.AppendLine("\t///</summary>");
            sb.AppendLine("\tpublic static IconConfiguration GetIconConfiguration()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tIconConfiguration config = new({publishedIconAssets.ToString().ToLower()});");
            if (publishedIconAssets)
            {
                FormatConfigSection(sb, "Sizes", "IconSize.Size", iconSizes);
                FormatConfigSection(sb, "Variants", "IconVariant.", iconVariants);
            }
            sb.AppendLine("\t\treturn config;");
            sb.AppendLine("\t}");


            //Create EmojiConfiguration
            sb.AppendLine("\t///<summary>");
            sb.AppendLine("\t/// Returns the emoji configuration,");
            sb.AppendLine("\t/// generated from the settings in the project file.");
            sb.AppendLine("\t///</summary>");
            sb.AppendLine("\tpublic static EmojiConfiguration GetEmojiConfiguration()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tEmojiConfiguration config = new({publishedEmojiAssets.ToString().ToLower()});");
            if (publishedEmojiAssets)
            {
                FormatConfigSection(sb, "Groups", "EmojiGroup.", emojiGroups);
                FormatConfigSection(sb, "Styles", "EmojiStyle.", emojiStyles);
            }

            sb.AppendLine("\t\treturn config;");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            context.AddSource($"ConfiguratonGenerator.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public bool TryReadGlobalOption(AnalyzerConfigOptions options, string property, out string? value)
        {
            return options.TryGetValue($"build_property.{property}", out value);
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
