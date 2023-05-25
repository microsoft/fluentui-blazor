using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    public class StaticAssetsOptions : IEquatable<StaticAssetsOptions?>
    {
        public List<string> IconSizes { get; set; } = new();
        public List<string> IconVariants { get; set; } = new();
        public List<string> EmojiGroups { get; set; } = new();
        public List<string> EmojiStyles { get; set; } = new();

        public bool PublishEmojiAssets { get; set; } = false;
        public bool PublishIconAssets { get; set; } = false;

        public override bool Equals(object? obj)
        {
            return Equals(obj as StaticAssetsOptions);
        }

        public bool Equals(StaticAssetsOptions? other)
        {
            return other is not null &&
                   IconSizes.SequenceEqual(other.IconSizes) &&
                   IconVariants.SequenceEqual(other.IconVariants) &&
                   EmojiGroups.SequenceEqual(other.EmojiGroups) &&
                   EmojiStyles.SequenceEqual(other.EmojiStyles) &&
                   PublishEmojiAssets == other.PublishEmojiAssets &&
                   PublishIconAssets == other.PublishIconAssets;
        }

        public override int GetHashCode()
        {
            int hashCode = 2097633831;
            hashCode = hashCode * -1521134295 + IconSizes.GetHashCode();
            hashCode = hashCode * -1521134295 + IconVariants.GetHashCode();
            hashCode = hashCode * -1521134295 + EmojiGroups.GetHashCode();
            hashCode = hashCode * -1521134295 + EmojiStyles.GetHashCode();
            hashCode = hashCode * -1521134295 + PublishEmojiAssets.GetHashCode();
            hashCode = hashCode * -1521134295 + PublishIconAssets.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StaticAssetsOptions left, StaticAssetsOptions right)
        {
            return EqualityComparer<StaticAssetsOptions>.Default.Equals(left, right);
        }

        public static bool operator !=(StaticAssetsOptions left, StaticAssetsOptions right)
        {
            return !(left == right);
        }
    }

    [Generator]
    public class ConfigurationGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValueProvider<StaticAssetsOptions> optionsProvider = context.AnalyzerConfigOptionsProvider
                .Select((provider, _) => ReadGlobalOptions(provider.GlobalOptions));

            context.RegisterSourceOutput(optionsProvider, GenerateSource);

        }

        private StaticAssetsOptions ReadGlobalOptions(AnalyzerConfigOptions options)
        {
            bool publishIconAssets = false;
            bool publishEmojiAssets = false;

            List<string> iconSizes = new();
            List<string> iconVariants = new();
            List<string> emojiGroups = new();
            List<string> emojiStyles = new();

            if (TryReadGlobalOption(options, "PublishFluentIconAssets", out string? publishIconAssetsProp))
            {
                if (bool.TryParse(publishIconAssetsProp, out publishIconAssets))
                {
                    TryReadGlobalOption(options, "FluentIconSizes", out string? iconSizesProp);

                    if (string.IsNullOrEmpty(iconSizesProp))
                    {
                        iconSizesProp = "10,12,16,20,24,28,32,48";
                    }
                    iconSizes = iconSizesProp!.Split(',').ToList();

                    TryReadGlobalOption(options, "FluentIconVariants", out var iconVariantsProp);

                    if (string.IsNullOrEmpty(iconVariantsProp))
                    {
                        iconVariantsProp = "Filled,Regular";
                    }
                    iconVariants = iconVariantsProp!.Split(',').ToList();
                }
            }

            if (TryReadGlobalOption(options, "PublishFluentEmojiAssets", out string? publishedEmojiAssetsProp))
            {
                if (bool.TryParse(publishedEmojiAssetsProp, out publishEmojiAssets))
                {
                    TryReadGlobalOption(options, "FluentEmojiGroups", out var emojiGroupsProp);
                    if (string.IsNullOrEmpty(emojiGroupsProp))
                    {
                        emojiGroupsProp = "Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places";
                    }
                    emojiGroups = emojiGroupsProp!.Split(',').ToList();

                    TryReadGlobalOption(options, "FluentEmojiStyles", out var emojiStylesProp);
                    if (string.IsNullOrEmpty(emojiStylesProp))
                    {
                        emojiStylesProp = "Color,Flat,HighContrast";
                    }
                    emojiStyles = emojiStylesProp!.Split(',').ToList();
                }

            }
            return new()
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

        private static void FormatConfigSection(StringBuilder sb, string section, string identifier, List<string> options)
        {
            if (options is not null && options.Any())
            {

                sb.AppendLine($"\t\tconfig.{section} = new[] {{");
                foreach (string option in options)
                {
                    if (!string.IsNullOrWhiteSpace(option))
                    {
                        sb.AppendLine($"\t\t\t{identifier}{option.Trim()},");
                    }
                }
                sb.AppendLine("\t\t};");
            }
        }
    }
}
