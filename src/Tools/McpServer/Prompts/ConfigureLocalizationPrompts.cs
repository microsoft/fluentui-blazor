// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

/// <summary>
/// MCP prompts for configuring localization in Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class ConfigureLocalizationPrompts
{
    /// <summary>
    /// Generates a prompt to help configure localization in Fluent UI Blazor.
    /// </summary>
    /// <param name="languages">Comma-separated list of languages to support.</param>
    /// <param name="useResourceFiles">Whether to use embedded resource files (.resx).</param>
    [McpServerPrompt(Name = "configure_localization")]
    [Description("Generates guidance for setting up localization to translate Fluent UI Blazor component strings into different languages.")]
    public static ChatMessage ConfigureLocalization(
        [Description("Comma-separated list of languages to support (e.g., 'en,fr,de,es'). Default is 'en,fr'.")]
        string languages = "en,fr",
        [Description("Whether to use embedded resource files (.resx) for translations. Default is true.")]
        bool useResourceFiles = true)
    {
        var languageList = languages.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var sb = new StringBuilder();
        sb.AppendLine("# Configure Localization in Fluent UI Blazor");
        sb.AppendLine();

        AppendLanguagesSection(sb, languageList);
        AppendOverview(sb);
        AppendSetupServices(sb, languageList);

        if (useResourceFiles)
        {
            AppendResourceFileApproach(sb, languageList);
        }
        else
        {
            AppendInlineApproach(sb);
        }

        AppendRegistration(sb);
        AppendAvailableKeys(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendLanguagesSection(StringBuilder sb, string[] languageList)
    {
        sb.AppendLine("## Languages to Support");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"- {string.Join("\n- ", languageList)}");
        sb.AppendLine();
    }

    private static void AppendOverview(StringBuilder sb)
    {
        sb.AppendLine("## Overview");
        sb.AppendLine();
        sb.AppendLine("Fluent UI Blazor provides English language strings by default. To translate component texts,");
        sb.AppendLine("you need to implement a custom `IFluentLocalizer`.");
        sb.AppendLine();
    }

    private static void AppendSetupServices(StringBuilder sb, string[] languageList)
    {
        sb.AppendLine("## Implementation Steps");
        sb.AppendLine();
        sb.AppendLine("### Step 1: Add Localization Services");
        sb.AppendLine();
        sb.AppendLine("In `Program.cs`:");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("// Add localization services");
        sb.AppendLine("builder.Services.AddLocalization();");
        sb.AppendLine();
        sb.AppendLine("// Configure supported cultures");
        sb.AppendLine("app.UseRequestLocalization(new RequestLocalizationOptions()");
        sb.Append("    .AddSupportedCultures(new[] { ");
        sb.Append(string.Join(", ", languageList.Select(l => $"\"{l}\"")));
        sb.AppendLine(" })");
        sb.Append("    .AddSupportedUICultures(new[] { ");
        sb.Append(string.Join(", ", languageList.Select(l => $"\"{l}\"")));
        sb.AppendLine(" }));");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendResourceFileApproach(StringBuilder sb, string[] languageList)
    {
        sb.AppendLine("### Step 2: Create Resource Files");
        sb.AppendLine();
        sb.AppendLine("Create a `Resources` folder and add resource files:");
        sb.AppendLine();

        foreach (var lang in languageList)
        {
            if (lang.Equals("en", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("- `Resources/FluentLocalizer.resx` (default/English)");
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"- `Resources/FluentLocalizer.{lang}.resx`");
            }
        }

        sb.AppendLine();
        sb.AppendLine("Set: **Build Action:** Embedded Resource, **Custom Tool:** ResXFileCodeGenerator");
        sb.AppendLine();

        AppendResourceFileLocalizer(sb);
    }

    private static void AppendResourceFileLocalizer(StringBuilder sb)
    {
        sb.AppendLine("### Step 3: Create Custom Localizer");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("public class CustomFluentLocalizer : IFluentLocalizer");
        sb.AppendLine("{");
        sb.AppendLine("    public string this[string key, params object[] arguments]");
        sb.AppendLine("    {");
        sb.AppendLine("        get");
        sb.AppendLine("        {");
        sb.AppendLine("            var localizedString = Resources.FluentLocalizer.ResourceManager");
        sb.AppendLine("                .GetString(key, CultureInfo.CurrentCulture);");
        sb.AppendLine("            return localizedString == null");
        sb.AppendLine("                ? IFluentLocalizer.GetDefault(key, arguments)");
        sb.AppendLine("                : string.Format(CultureInfo.CurrentCulture, localizedString, arguments);");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendInlineApproach(StringBuilder sb)
    {
        sb.AppendLine("### Step 2: Create Custom Localizer (Inline Translations)");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("public class CustomFluentLocalizer : IFluentLocalizer");
        sb.AppendLine("{");
        sb.AppendLine("    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()");
        sb.AppendLine("    {");
        sb.AppendLine("        [\"fr\"] = new() { [\"MessageBox_ButtonCancel\"] = \"Annuler\" }");
        sb.AppendLine("    };");
        sb.AppendLine();
        sb.AppendLine("    public string this[string key, params object[] arguments] => GetTranslation(key, arguments);");
        sb.AppendLine();
        sb.AppendLine("    private static string GetTranslation(string key, object[] arguments)");
        sb.AppendLine("    {");
        sb.AppendLine("        var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;");
        sb.AppendLine("        if (Translations.TryGetValue(culture, out var dict) && dict.TryGetValue(key, out var val))");
        sb.AppendLine("            return string.Format(CultureInfo.CurrentCulture, val, arguments);");
        sb.AppendLine("        return IFluentLocalizer.GetDefault(key, arguments);");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendRegistration(StringBuilder sb)
    {
        sb.AppendLine("### Register the Localizer");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("builder.Services.AddFluentUIComponents(config =>");
        sb.AppendLine("{");
        sb.AppendLine("    config.Localizer = new CustomFluentLocalizer();");
        sb.AppendLine("});");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendAvailableKeys(StringBuilder sb)
    {
        sb.AppendLine("## Available Localization Keys");
        sb.AppendLine();
        sb.AppendLine("Common keys: `MessageBox_ButtonOk`, `MessageBox_ButtonCancel`, `DataGrid_EmptyContent`, etc.");
        sb.AppendLine();
        sb.AppendLine("Find all keys in `Localization/LanguageResource.resx` in the library source.");
        sb.AppendLine();
        sb.AppendLine("Please generate a complete localization implementation for the specified languages.");
    }
}
