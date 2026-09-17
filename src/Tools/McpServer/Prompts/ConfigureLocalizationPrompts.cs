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
        sb.AppendLine("- Add `builder.Services.AddLocalization();`");
        sb.AppendLine("- Configure supported cultures using `UseRequestLocalization`");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- Supported languages: {string.Join(", ", languageList)}");
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
        sb.AppendLine("Create a class implementing `IFluentLocalizer` that:");
        sb.AppendLine();
        sb.AppendLine("- Retrieves localized strings from resource files using `ResourceManager`");
        sb.AppendLine("- Falls back to default values using `IFluentLocalizer.GetDefault()` when translation is not found");
        sb.AppendLine("- Formats strings with arguments using `string.Format()`");
        sb.AppendLine();
    }

    private static void AppendInlineApproach(StringBuilder sb)
    {
        sb.AppendLine("### Step 2: Create Custom Localizer (Inline Translations)");
        sb.AppendLine();
        sb.AppendLine("Create a class implementing `IFluentLocalizer` that:");
        sb.AppendLine();
        sb.AppendLine("- Stores translations in a dictionary organized by culture");
        sb.AppendLine("- Looks up translations based on `CultureInfo.CurrentCulture`");
        sb.AppendLine("- Falls back to default values using `IFluentLocalizer.GetDefault()`");
        sb.AppendLine();
    }

    private static void AppendRegistration(StringBuilder sb)
    {
        sb.AppendLine("### Register the Localizer");
        sb.AppendLine();
        sb.AppendLine("Register your custom localizer in `Program.cs` by setting `config.Localizer` in `AddFluentUIComponents()`.");
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
        sb.AppendLine("**Important:** Use the available MCP tools to retrieve component documentation and code examples for the localization implementation.");
    }
}
