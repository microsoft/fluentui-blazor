using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    [Generator]
    public class ConfigurationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Debugger.Launch();
            // No initialization required for this one
        }

        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder sb = new();

            string[]? iconSizes = Array.Empty<string>();
            string[]? iconVariants = Array.Empty<string>();
            string[]? emojiGroups = Array.Empty<string>();
            string[]? emojiStyles = Array.Empty<string>();

            bool publishEmojiAssets = false;
            bool publishIconAssets = true;

            if (TryReadGlobalOption(context, "PublishFluentIconAssets", out string? publishIconAssetsProp))
            {
                if (!bool.TryParse(publishIconAssetsProp, out publishIconAssets))
                {
                    publishIconAssets = true;
                }
                if (publishIconAssets)
                {
                    TryReadGlobalOption(context, "FluentIconSizes", out string? iconSizesProp);
                    iconSizes = iconSizesProp?.Split(',');

                    TryReadGlobalOption(context, "FluentIconVariants", out var iconVariantsProp);
                    iconVariants = iconVariantsProp?.Split(',');
                }
            }

            if (TryReadGlobalOption(context, "PublishFluentEmojiAssets", out string? publishEmojiAssetsProp))
            {
                if (!bool.TryParse(publishEmojiAssetsProp, out publishEmojiAssets))
                {
                    publishEmojiAssets = false;
                }
                if (publishEmojiAssets)
                {
                    TryReadGlobalOption(context, "FluentEmojiGroups", out var emojiGroupsProp);
                    emojiGroups = emojiGroupsProp?.Split(',');

                    TryReadGlobalOption(context, "FluentEmojiStyles", out var emojiStylesProp);
                    emojiStyles = emojiStylesProp?.Split(',');
                }
            }

            //sb.AppendLine($"using Microsoft.Extensions.DependencyInjection;");
            //sb.AppendLine($"");
            sb.AppendLine($"namespace Microsoft.Fast.Components.FluentUI;");
            sb.AppendLine($"");
            sb.AppendLine("public static class ConfigurationGenerator");
            sb.AppendLine("{");

            //Create IconConfiguration
            sb.AppendLine("\tpublic static IconConfiguration GetIconConfiguration()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tIconConfiguration config = new({publishIconAssets.ToString().ToLower()});");
            if (publishIconAssets)
            {
                FormatConfigSection(sb, "Sizes", "IconSize.Size", iconSizes);
                FormatConfigSection(sb, "Variants", "IconVariant.", iconVariants);
            }
            sb.AppendLine("\t\treturn config;");
            sb.AppendLine("\t}");


            //Create EmojiConfiguration
            sb.AppendLine("\tpublic static EmojiConfiguration GetEmojiConfiguration()");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tEmojiConfiguration config = new({publishEmojiAssets.ToString().ToLower()});");
            if (publishEmojiAssets)
            {
                FormatConfigSection(sb, "Groups", "EmojiGroup.", emojiGroups);
                FormatConfigSection(sb, "Styles", "EmojiStyle.", emojiStyles);
            }

            sb.AppendLine("\t\treturn config;");
            sb.AppendLine("\t}");

            sb.AppendLine("}");
            context.AddSource($"ConfiguratonGenerator.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public bool TryReadGlobalOption(GeneratorExecutionContext context, string property, out string? value)
        {
            return context.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.{property}", out value);
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
                        sb.AppendLine($"\t\t\t{identifier}{ToTitleCase(option)}{endmarker}");
                    }
                }
                sb.AppendLine("\t\t};");
            }
        }

        private static string ToTitleCase(string input)
        {
            string[] words = input.Split('_');
            StringBuilder sb = new();
            foreach (string word in words)
            {
                sb.Append(word[0].ToString().ToUpper());
                sb.Append(word.Substring(1));
                sb.Append("_");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();


        }
    }
}
