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
        sb.AppendLine("- Wrap your content with `<FluentProviders>` component");
        sb.AppendLine("- Set the `Theme` parameter to a `DesignTheme` instance");
        sb.AppendLine("- Configure `Mode` property to `DesignMode.System`, `DesignMode.Light`, or `DesignMode.Dark`");
        sb.AppendLine();
    }

    private static void AppendCustomColors(StringBuilder sb)
    {
        sb.AppendLine("## Custom Brand Colors");
        sb.AppendLine();
        sb.AppendLine("To customize brand colors:");
        sb.AppendLine();
        sb.AppendLine("- Set `PrimaryColor` property on `DesignTheme` with your brand color hex value");
        sb.AppendLine("- Key color tokens include: `--colorBrandBackground`, `--colorNeutralBackground1`, etc.");
        sb.AppendLine();
    }

    private static void AppendDarkMode(StringBuilder sb)
    {
        sb.AppendLine("## Dark Mode Implementation");
        sb.AppendLine();
        sb.AppendLine("To implement dark mode toggle:");
        sb.AppendLine();
        sb.AppendLine("- Use a `FluentSwitch` component to toggle between modes");
        sb.AppendLine("- Set `Mode` to `DesignMode.Dark` or `DesignMode.Light` based on user preference");
        sb.AppendLine("- Use `DesignMode.System` to follow OS preference automatically");
        sb.AppendLine();
    }

    private static void AppendDesignTokens(StringBuilder sb)
    {
        sb.AppendLine("## CSS Design Tokens Reference");
        sb.AppendLine();
        sb.AppendLine("Key design token categories:");
        sb.AppendLine();
        sb.AppendLine("- **Typography**: `--fontFamilyBase`, `--fontSizeBase300`");
        sb.AppendLine("- **Colors**: `--colorNeutralForeground1`, `--colorNeutralBackground1`");
        sb.AppendLine("- **Spacing**: `--spacingVerticalM`, `--spacingHorizontalL`");
        sb.AppendLine("- **Borders & Shadows**: `--borderRadiusMedium`, `--shadow4`");
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
        sb.AppendLine("**Important:** Use the available MCP tools to retrieve component documentation and code examples for the theming implementation.");
    }
}
