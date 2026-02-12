// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

/// <summary>
/// MCP prompts for icon discovery and usage in Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class IconPrompts
{
    private readonly IconService _iconService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconPrompts"/> class.
    /// </summary>
    public IconPrompts(IconService iconService)
    {
        _iconService = iconService;
    }

    /// <summary>
    /// Helps find the best Fluent UI icon for a specific purpose or UI element.
    /// </summary>
    [McpServerPrompt(Name = "find_icon")]
    [Description("Find the best Fluent UI icon for a specific purpose or UI element. Describes what the icon should represent and the AI will search the catalog and recommend the best match.")]
    public ChatMessage FindIcon(
        [Description("What the icon should represent (e.g., 'delete a record', 'user profile', 'send email', 'warning message').")]
        string description,
        [Description("Optional: where the icon will be used (e.g., 'toolbar button', 'navigation menu', 'status indicator', 'empty state').")]
        string context = "")
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Find the Best Fluent UI Icon");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**What the icon should represent:** {description}");

        if (!string.IsNullOrWhiteSpace(context))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Where it will be used:** {context}");
        }

        sb.AppendLine();
        sb.AppendLine("## Instructions");
        sb.AppendLine();
        sb.AppendLine("1. Use the **SearchIcons** tool to find icons matching the description above.");
        sb.AppendLine("   - Try multiple search terms if the first doesn't yield good results.");
        sb.AppendLine("   - The tool supports synonyms (e.g., 'trash' → Delete, 'bell' → Alert).");
        sb.AppendLine();
        sb.AppendLine("2. Use the **GetIconDetails** tool on the best candidates to see all available variants and sizes.");
        sb.AppendLine();
        sb.AppendLine("3. Recommend the **best icon** based on:");
        sb.AppendLine("   - Semantic match to the described purpose");
        sb.AppendLine("   - Availability of the needed variant and size for the context");
        sb.AppendLine("   - Consistency with Fluent UI design guidelines");
        sb.AppendLine();

        AppendSizeGuidance(sb, context);

        sb.AppendLine("## Output Format");
        sb.AppendLine();
        sb.AppendLine("For each recommended icon, provide:");
        sb.AppendLine("- Icon name and why it's a good match");
        sb.AppendLine("- Recommended variant and size for the given context");
        sb.AppendLine("- Ready-to-paste Blazor code: `<FluentIcon Value=\"@(new Icons.{Variant}.Size{Size}.{Name}())\" />`");
        sb.AppendLine("- Alternative icon suggestions if applicable");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    /// <summary>
    /// Generates complete Blazor code for using a specific icon in different contexts.
    /// </summary>
    [McpServerPrompt(Name = "use_icon")]
    [Description("Generate complete Blazor code for using a specific Fluent UI icon in various contexts (buttons, menus, navigation, etc.) with proper accessibility attributes.")]
    public ChatMessage UseIcon(
        [Description("The icon name to use (e.g., 'Bookmark', 'Alert', 'Delete', 'Settings').")]
        string iconName,
        [Description("Optional: where the icon will be used — 'button', 'menu', 'toolbar', 'navigation', 'status', 'header', 'form', 'table', or any free-form description.")]
        string useCase = "")
    {
        var icon = _iconService.GetIconByName(iconName);
        var iconExists = icon is not null;

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Use Icon: {iconName}");
        sb.AppendLine();

        AppendIconAvailability(sb, icon, iconName);

        if (!string.IsNullOrWhiteSpace(useCase))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Use case:** {useCase}");
            sb.AppendLine();
        }

        AppendUseIconInstructions(sb, iconExists);
        AppendUseCaseGuidance(sb, useCase);
        AppendAccessibilitySection(sb, icon, iconExists);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private void AppendIconAvailability(StringBuilder sb, Models.IconModel? icon, string iconName)
    {
        if (icon is null)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"⚠️ The icon '{iconName}' was not found in the catalog.");
            sb.AppendLine("Use the **SearchIcons** tool to find the correct icon name, then provide the code.");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Available variants:** {string.Join(", ", icon.VariantNames)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Available sizes:** {string.Join(", ", icon.AllSizes)}");

            var (defaultVariant, defaultSize) = _iconService.GetRecommendedDefault(icon);
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Recommended default:** `Icons.{defaultVariant}.Size{defaultSize}.{icon.Name}`");
            sb.AppendLine();
        }
    }

    private static void AppendUseIconInstructions(StringBuilder sb, bool iconExists)
    {
        sb.AppendLine("## Instructions");
        sb.AppendLine();

        if (iconExists)
        {
            sb.AppendLine("1. Use the **GetIconUsage** tool to generate code examples for this icon.");
        }
        else
        {
            sb.AppendLine("1. Use the **SearchIcons** tool to find the correct icon name.");
            sb.AppendLine("2. Use the **GetIconDetails** tool to see available variants and sizes.");
            sb.AppendLine("3. Use the **GetIconUsage** tool to generate code examples.");
        }

        sb.AppendLine();
        sb.AppendLine("2. Adapt the code to the specific use case, considering:");
        sb.AppendLine();
    }

    private void AppendAccessibilitySection(StringBuilder sb, Models.IconModel? icon, bool iconExists)
    {
        sb.AppendLine("## Accessibility Requirements");
        sb.AppendLine();
        sb.AppendLine("- **Decorative icons** (next to text labels): no additional attributes needed");
        sb.AppendLine("- **Interactive icons** (clickable): add `Title` and/or `Tooltip` for screen readers");
        sb.AppendLine("- **Standalone icons** (no text label): always add `Title` for accessibility");
        sb.AppendLine("- **Focusable icons**: set `Focusable=\"true\"` for keyboard navigation");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("@* Accessible standalone icon *@");

        if (iconExists)
        {
            var (v, s) = _iconService.GetRecommendedDefault(icon!);
            sb.AppendLine(CultureInfo.InvariantCulture, $"<FluentIcon Value=\"@(new Icons.{v}.Size{s}.{icon!.Name}())\"");
            sb.AppendLine(CultureInfo.InvariantCulture, $"             Title=\"{icon.Name}\"");
            sb.AppendLine("             Focusable=\"true\" />");
        }
        else
        {
            sb.AppendLine("<FluentIcon Value=\"@(new Icons.Regular.Size20.{IconName}())\"");
            sb.AppendLine("             Title=\"Descriptive text\"");
            sb.AppendLine("             Focusable=\"true\" />");
        }

        sb.AppendLine("```");
    }

    private static void AppendSizeGuidance(StringBuilder sb, string context)
    {
        sb.AppendLine("## Size Guidance");
        sb.AppendLine();
        sb.AppendLine("Choose the icon size based on the context:");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(context))
        {
            var contextLower = context.ToLowerInvariant();
            if (contextLower.Contains("button", StringComparison.Ordinal) || contextLower.Contains("toolbar", StringComparison.Ordinal))
            {
                sb.AppendLine("→ For **buttons and toolbars**: use **Size20** or **Size24**");
            }
            else if (contextLower.Contains("nav", StringComparison.Ordinal) || contextLower.Contains("menu", StringComparison.Ordinal))
            {
                sb.AppendLine("→ For **navigation and menus**: use **Size20** or **Size24**");
            }
            else if (contextLower.Contains("inline", StringComparison.Ordinal) || contextLower.Contains("text", StringComparison.Ordinal) || contextLower.Contains("compact", StringComparison.Ordinal))
            {
                sb.AppendLine("→ For **inline/compact UI**: use **Size16**");
            }
            else if (contextLower.Contains("header", StringComparison.Ordinal) || contextLower.Contains("hero", StringComparison.Ordinal) || contextLower.Contains("empty", StringComparison.Ordinal))
            {
                sb.AppendLine("→ For **headers and empty states**: use **Size32** or **Size48**");
            }
            else if (contextLower.Contains("status", StringComparison.Ordinal) || contextLower.Contains("indicator", StringComparison.Ordinal) || contextLower.Contains("badge", StringComparison.Ordinal))
            {
                sb.AppendLine("→ For **status indicators and badges**: use **Size12** or **Size16**");
            }

            sb.AppendLine();
        }

        sb.AppendLine("| Size | Best For |");
        sb.AppendLine("|------|----------|");
        sb.AppendLine("| 10-12 | Micro indicators, badges |");
        sb.AppendLine("| 16 | Compact UI, inline with text |");
        sb.AppendLine("| 20 | Default for most components |");
        sb.AppendLine("| 24 | Buttons, toolbars, navigation |");
        sb.AppendLine("| 28-32 | Headers, emphasis areas |");
        sb.AppendLine("| 48 | Hero sections, empty states |");
        sb.AppendLine();
    }

    private static void AppendUseCaseGuidance(StringBuilder sb, string useCase)
    {
        if (string.IsNullOrWhiteSpace(useCase))
        {
            AppendDefaultGuidance(sb);
            return;
        }

        var useCaseLower = useCase.ToLowerInvariant();

        if (useCaseLower.Contains("button", StringComparison.Ordinal))
        {
            AppendButtonGuidance(sb);
        }
        else if (useCaseLower.Contains("menu", StringComparison.Ordinal) || useCaseLower.Contains("nav", StringComparison.Ordinal))
        {
            AppendMenuNavGuidance(sb);
        }
        else if (useCaseLower.Contains("toolbar", StringComparison.Ordinal))
        {
            AppendToolbarGuidance(sb);
        }
        else if (useCaseLower.Contains("status", StringComparison.Ordinal) || useCaseLower.Contains("indicator", StringComparison.Ordinal))
        {
            AppendStatusGuidance(sb);
        }
        else if (useCaseLower.Contains("header", StringComparison.Ordinal) || useCaseLower.Contains("hero", StringComparison.Ordinal))
        {
            AppendHeaderHeroGuidance(sb);
        }
        else if (useCaseLower.Contains("form", StringComparison.Ordinal) || useCaseLower.Contains("input", StringComparison.Ordinal))
        {
            AppendFormInputGuidance(sb);
        }
        else if (useCaseLower.Contains("table", StringComparison.Ordinal) || useCaseLower.Contains("grid", StringComparison.Ordinal) || useCaseLower.Contains("datagrid", StringComparison.Ordinal))
        {
            AppendTableGridGuidance(sb);
        }
        else
        {
            AppendDefaultGuidance(sb);
        }
    }

    private static void AppendDefaultGuidance(StringBuilder sb)
    {
        sb.AppendLine("- Choose the right **variant** (Regular for default, Filled for active/emphasis)");
        sb.AppendLine("- Choose the right **size** for the context");
        sb.AppendLine("- Add proper **accessibility** attributes");
        sb.AppendLine("- Use **Slot** parameter when inside FluentButton or similar components");
        sb.AppendLine();
    }

    private static void AppendButtonGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Button Usage");
        sb.AppendLine("- Use `Slot=\"@FluentSlot.Start\"` to place the icon before the label");
        sb.AppendLine("- Use `Slot=\"@FluentSlot.End\"` to place the icon after the label");
        sb.AppendLine("- Use **Size20** for standard buttons, **Size16** for compact buttons");
        sb.AppendLine("- Use **Regular** variant for default state");
        sb.AppendLine();
    }

    private static void AppendMenuNavGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Menu / Navigation Usage");
        sb.AppendLine("- Use **Size20** or **Size24** for menu items");
        sb.AppendLine("- Use **Regular** for inactive items, **Filled** for the active/selected item");
        sb.AppendLine("- Place icon with `Slot=\"@FluentSlot.Start\"` before the menu label");
        sb.AppendLine();
    }

    private static void AppendToolbarGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Toolbar Usage");
        sb.AppendLine("- Use **Size20** for standard toolbar buttons");
        sb.AppendLine("- Add `Tooltip` for icon-only toolbar buttons");
        sb.AppendLine("- Use consistent sizing across all toolbar icons");
        sb.AppendLine();
    }

    private static void AppendStatusGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Status Indicator Usage");
        sb.AppendLine("- Use **Size16** or **Size12** for inline status indicators");
        sb.AppendLine("- Apply appropriate `Color` (e.g., `Color.Success`, `Color.Error`, `Color.Warning`)");
        sb.AppendLine("- Consider pairing with a text label for accessibility");
        sb.AppendLine();
    }

    private static void AppendHeaderHeroGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Header / Hero Usage");
        sb.AppendLine("- Use **Size32** or **Size48** for visual impact");
        sb.AppendLine("- Consider **Light** variant for a more elegant appearance (if available)");
        sb.AppendLine("- Apply brand colors via `Color=\"@Color.Primary\"`");
        sb.AppendLine();
    }

    private static void AppendFormInputGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Form / Input Usage");
        sb.AppendLine("- Use **Size20** for input field icons");
        sb.AppendLine("- Place with `Slot=\"@FluentSlot.Start\"` in text inputs");
        sb.AppendLine("- Use **Regular** variant for a clean look");
        sb.AppendLine();
    }

    private static void AppendTableGridGuidance(StringBuilder sb)
    {
        sb.AppendLine("### Table / DataGrid Usage");
        sb.AppendLine("- Use **Size16** or **Size20** for column actions");
        sb.AppendLine("- Add `OnClick` handler for interactive icons");
        sb.AppendLine("- Add `Tooltip` for icon-only action columns");
        sb.AppendLine();
    }
}
