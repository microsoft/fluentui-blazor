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
/// MCP prompts for implementing accessibility in Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class AccessibilityPrompts
{
    /// <summary>
    /// Generates a prompt to help implement accessibility in Fluent UI Blazor applications.
    /// </summary>
    /// <param name="componentOrFeature">The component or feature to make accessible.</param>
    /// <param name="wcagLevel">WCAG compliance level to target.</param>
    [McpServerPrompt(Name = "implement_accessibility")]
    [Description("Generates guidance for implementing accessibility (a11y) best practices in Fluent UI Blazor applications.")]
    public static ChatMessage ImplementAccessibility(
        [Description("The component or feature to make accessible (e.g., 'form', 'navigation', 'data table', 'dialog')")]
        string componentOrFeature,
        [Description("WCAG compliance level: 'A', 'AA' (recommended), or 'AAA'. Default is 'AA'.")]
        string wcagLevel = "AA")
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Implement Accessibility in Fluent UI Blazor");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"## Target: {componentOrFeature}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"## WCAG Level: {wcagLevel}");
        sb.AppendLine();

        AppendAccessibilityFeatures(sb);
        AppendImplementationGuidelines(sb);
        AppendComponentGuidelines(sb);
        AppendTestingGuidelines(sb);
        AppendRequest(sb, componentOrFeature, wcagLevel);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendAccessibilityFeatures(StringBuilder sb)
    {
        sb.AppendLine("## Fluent UI Blazor Accessibility Features");
        sb.AppendLine();
        sb.AppendLine("Fluent UI Blazor components are built with accessibility in mind and include:");
        sb.AppendLine();
        sb.AppendLine("- **ARIA attributes** - Proper roles, labels, and states");
        sb.AppendLine("- **Keyboard navigation** - Full keyboard support");
        sb.AppendLine("- **Focus management** - Visible focus indicators");
        sb.AppendLine("- **Screen reader support** - Semantic HTML and announcements");
        sb.AppendLine("- **Color contrast** - Compliant color combinations");
        sb.AppendLine();
    }

    private static void AppendImplementationGuidelines(StringBuilder sb)
    {
        sb.AppendLine("## Implementation Guidelines");
        sb.AppendLine();

        // Semantic HTML
        sb.AppendLine("### 1. Semantic HTML");
        sb.AppendLine();
        sb.AppendLine("- Use appropriate HTML elements (button, nav, main, etc.)");
        sb.AppendLine("- Add landmark roles for page structure");
        sb.AppendLine("- Use heading hierarchy correctly (h1, h2, h3...)");
        sb.AppendLine();

        // Labels
        sb.AppendLine("### 2. Labels and Descriptions");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("@* Always provide labels for form inputs *@");
        sb.AppendLine("<FluentTextInput Label=\"Email Address\" @bind-Value=\"email\" />");
        sb.AppendLine();
        sb.AppendLine("@* For icon-only buttons, use aria-label *@");
        sb.AppendLine("<FluentButton AriaLabel=\"Delete item\" OnClick=\"Delete\">");
        sb.AppendLine("    <FluentIcon Value=\"@(new Icons.Regular.Size16.Delete())\" />");
        sb.AppendLine("</FluentButton>");
        sb.AppendLine("```");
        sb.AppendLine();

        // Keyboard
        sb.AppendLine("### 3. Keyboard Navigation");
        sb.AppendLine();
        sb.AppendLine("- Tab order should be logical");
        sb.AppendLine("- Interactive elements must be focusable");
        sb.AppendLine("- Use `TabIndex` parameter when needed");
        sb.AppendLine();

        // Focus
        sb.AppendLine("### 4. Focus Management");
        sb.AppendLine();
        sb.AppendLine("- Dialogs trap focus automatically");
        sb.AppendLine("- Return focus to trigger element when closing overlays");
        sb.AppendLine();

        // Color
        sb.AppendLine("### 5. Color and Contrast");
        sb.AppendLine();
        sb.AppendLine("- Don't rely solely on color to convey meaning");
        sb.AppendLine("- Use icons, patterns, or text alongside color");
        sb.AppendLine();
    }

    private static void AppendComponentGuidelines(StringBuilder sb)
    {
        sb.AppendLine("## Component-Specific Guidelines");
        sb.AppendLine();
        sb.AppendLine("| Component | Key A11y Features |");
        sb.AppendLine("|-----------|-------------------|");
        sb.AppendLine("| FluentButton | AriaLabel, keyboard activation |");
        sb.AppendLine("| FluentTextInput | Label, Required indicator, validation |");
        sb.AppendLine("| FluentDataGrid | Arrow key navigation, sort announcements |");
        sb.AppendLine("| FluentDialog | Focus trap, Escape to close |");
        sb.AppendLine("| FluentMenu | Arrow navigation, type-ahead |");
        sb.AppendLine("| FluentTabs | Arrow key navigation, Tab panel roles |");
        sb.AppendLine();
    }

    private static void AppendTestingGuidelines(StringBuilder sb)
    {
        sb.AppendLine("## Testing Accessibility");
        sb.AppendLine();
        sb.AppendLine("1. **Keyboard Testing** - Navigate without a mouse");
        sb.AppendLine("2. **Screen Reader** - Test with NVDA, JAWS, or VoiceOver");
        sb.AppendLine("3. **Browser Tools** - Use Accessibility DevTools");
        sb.AppendLine("4. **Automated Testing** - axe DevTools, Lighthouse");
        sb.AppendLine();
    }

    private static void AppendRequest(StringBuilder sb, string componentOrFeature, string wcagLevel)
    {
        sb.AppendLine("## Request");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Please provide specific accessibility guidance for implementing `{componentOrFeature}` at WCAG {wcagLevel} level, including:");
        sb.AppendLine();
        sb.AppendLine("1. Required ARIA attributes");
        sb.AppendLine("2. Keyboard interaction patterns");
        sb.AppendLine("3. Screen reader announcements");
        sb.AppendLine("4. Code examples with proper accessibility");
        sb.AppendLine("5. Testing checklist");
    }
}
