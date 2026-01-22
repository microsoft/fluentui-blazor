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
/// MCP prompts for configuring theming in Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class ConfigureThemingPrompts
{
    /// <summary>
    /// Generates a prompt to help configure theming in Fluent UI Blazor.
    /// </summary>
    /// <param name="themingGoal">What the user wants to achieve with theming.</param>
    /// <param name="includeCustomColors">Whether to include custom brand colors.</param>
    /// <param name="supportDarkMode">Whether to support dark mode.</param>
    [McpServerPrompt(Name = "configure_theming")]
    [Description("Generates guidance for configuring themes, colors, and design tokens in Fluent UI Blazor applications.")]
    public static ChatMessage ConfigureTheming(
        [Description("What you want to achieve with theming (e.g., 'add company branding', 'implement dark mode toggle', 'customize colors')")]
        string themingGoal,
        [Description("Whether to include custom brand colors (default: true)")]
        bool includeCustomColors = true,
        [Description("Whether to support dark mode (default: true)")]
        bool supportDarkMode = true)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Configure Theming in Fluent UI Blazor");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Objective:** {themingGoal}");
        sb.AppendLine();

        AppendArchitecture(sb);
        AppendThemeProvider(sb);

        if (includeCustomColors)
        {
            AppendCustomColors(sb);
        }

        if (supportDarkMode)
        {
            AppendDarkMode(sb);
        }

        AppendDesignTokens(sb);
        AppendBestPractices(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendArchitecture(StringBuilder sb)
    {
        sb.AppendLine("## Theming Architecture");
        sb.AppendLine();
        sb.AppendLine("Fluent UI Blazor uses CSS custom properties (design tokens) for theming:");
        sb.AppendLine();
        sb.AppendLine("- Colors (foreground, background, brand)");
        sb.AppendLine("- Typography (font family, sizes, weights)");
        sb.AppendLine("- Spacing, borders, and shadows");
        sb.AppendLine();
    }

    private static void AppendThemeProvider(StringBuilder sb)
    {
        sb.AppendLine("## Theme Provider Setup");
        sb.AppendLine();
        sb.AppendLine("In `MainLayout.razor`:");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentProviders Theme=\"@currentTheme\">");
        sb.AppendLine("    @Body");
        sb.AppendLine("</FluentProviders>");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    private DesignTheme currentTheme = new()");
        sb.AppendLine("    {");
        sb.AppendLine("        Mode = DesignMode.System, // System, Light, or Dark");
        sb.AppendLine("    };");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendCustomColors(StringBuilder sb)
    {
        sb.AppendLine("## Custom Brand Colors");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("private DesignTheme currentTheme = new()");
        sb.AppendLine("{");
        sb.AppendLine("    Mode = DesignMode.Light,");
        sb.AppendLine("    PrimaryColor = \"#0078D4\", // Your brand color");
        sb.AppendLine("};");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("**Key color tokens:** `--colorBrandBackground`, `--colorNeutralBackground1`, etc.");
        sb.AppendLine();
    }

    private static void AppendDarkMode(StringBuilder sb)
    {
        sb.AppendLine("## Dark Mode Implementation");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentSwitch @bind-Value=\"@isDarkMode\" Label=\"Dark Mode\" />");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    private bool isDarkMode;");
        sb.AppendLine("    private DesignTheme Theme => new()");
        sb.AppendLine("    {");
        sb.AppendLine("        Mode = isDarkMode ? DesignMode.Dark : DesignMode.Light,");
        sb.AppendLine("    };");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("Use `DesignMode.System` to follow OS preference automatically.");
        sb.AppendLine();
    }

    private static void AppendDesignTokens(StringBuilder sb)
    {
        sb.AppendLine("## CSS Design Tokens Reference");
        sb.AppendLine();
        sb.AppendLine("```css");
        sb.AppendLine("/* Typography */");
        sb.AppendLine("font-family: var(--fontFamilyBase);");
        sb.AppendLine("font-size: var(--fontSizeBase300);");
        sb.AppendLine();
        sb.AppendLine("/* Colors */");
        sb.AppendLine("color: var(--colorNeutralForeground1);");
        sb.AppendLine("background: var(--colorNeutralBackground1);");
        sb.AppendLine();
        sb.AppendLine("/* Spacing */");
        sb.AppendLine("padding: var(--spacingVerticalM) var(--spacingHorizontalL);");
        sb.AppendLine("gap: var(--spacingHorizontalM);");
        sb.AppendLine();
        sb.AppendLine("/* Borders & Shadows */");
        sb.AppendLine("border-radius: var(--borderRadiusMedium);");
        sb.AppendLine("box-shadow: var(--shadow4);");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendBestPractices(StringBuilder sb)
    {
        sb.AppendLine("## Best Practices");
        sb.AppendLine();
        sb.AppendLine("1. **Use design tokens** instead of hardcoded colors");
        sb.AppendLine("2. **Test both themes** - ensure light and dark mode work");
        sb.AppendLine("3. **Persist user preference** in localStorage or user settings");
        sb.AppendLine("4. **Consider accessibility** - ensure sufficient color contrast");
        sb.AppendLine();
        sb.AppendLine("Please generate a complete theming implementation based on the requirements.");
    }
}
