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
/// MCP prompts for troubleshooting Fluent UI Blazor issues.
/// </summary>
[McpServerPromptType]
public class TroubleshootPrompts
{
    /// <summary>
    /// Generates a prompt to help troubleshoot common Fluent UI Blazor issues.
    /// </summary>
    /// <param name="issueDescription">Description of the issue being experienced.</param>
    /// <param name="issueCategory">Category of the issue.</param>
    [McpServerPrompt(Name = "troubleshoot_issue")]
    [Description("Generates troubleshooting guidance for common Fluent UI Blazor issues, including rendering problems, styling issues, and component behavior.")]
    public static ChatMessage TroubleshootIssue(
        [Description("A description of the issue you're experiencing (e.g., 'dialog not appearing', 'styles not applied', 'component not rendering')")]
        string issueDescription,
        [Description("Category of the issue: 'rendering' (component not showing), 'styling' (visual issues), 'behavior' (functionality problems), 'performance' (slow/laggy), or 'general'. Default is 'general'.")]
        string issueCategory = "general")
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Troubleshoot Fluent UI Blazor Issue");
        sb.AppendLine();
        sb.AppendLine("## Issue Description");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Problem:** {issueDescription}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Category:** {issueCategory}");
        sb.AppendLine();

        sb.AppendLine("## Common Troubleshooting Steps");
        sb.AppendLine();

        switch (issueCategory.ToLowerInvariant())
        {
            case "rendering":
                GenerateRenderingTroubleshooting(sb);
                break;
            case "styling":
                GenerateStylingTroubleshooting(sb);
                break;
            case "behavior":
                GenerateBehaviorTroubleshooting(sb);
                break;
            case "performance":
                GeneratePerformanceTroubleshooting(sb);
                break;
            default:
                GenerateGeneralTroubleshooting(sb);
                break;
        }

        sb.AppendLine();
        sb.AppendLine("## Request");
        sb.AppendLine();
        sb.AppendLine("Based on the issue description above, please:");
        sb.AppendLine();
        sb.AppendLine("1. Identify the most likely cause(s)");
        sb.AppendLine("2. Provide step-by-step troubleshooting instructions");
        sb.AppendLine("3. Suggest specific fixes with code examples");
        sb.AppendLine("4. Recommend preventive measures");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void GenerateRenderingTroubleshooting(StringBuilder sb)
    {
        sb.AppendLine("### Rendering Issues Checklist");
        sb.AppendLine();
        sb.AppendLine("1. **Check Render Mode**");
        sb.AppendLine("   - Fluent UI Blazor requires interactive rendering");
        sb.AppendLine("   - Ensure `@rendermode InteractiveServer` or `InteractiveWebAssembly` is set");
        sb.AppendLine("   - Check Routes.razor for global render mode");
        sb.AppendLine();
        sb.AppendLine("2. **Verify FluentProviders**");
        sb.AppendLine("   - Ensure `<FluentProviders />` is in MainLayout.razor");
        sb.AppendLine("   - Check that it's placed at the end of the layout");
        sb.AppendLine();
        sb.AppendLine("3. **Check Service Registration**");
        sb.AppendLine("   - Verify `AddFluentUIComponents()` is called in Program.cs");
        sb.AppendLine();
        sb.AppendLine("4. **CSS Bundle Reference**");
        sb.AppendLine("   - Check for stylesheet link in App.razor/index.html");
        sb.AppendLine("   - Path: `_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css`");
        sb.AppendLine();
        sb.AppendLine("5. **Browser Console**");
        sb.AppendLine("   - Check for JavaScript errors");
        sb.AppendLine("   - Look for 404 errors on static files");
    }

    private static void GenerateStylingTroubleshooting(StringBuilder sb)
    {
        sb.AppendLine("### Styling Issues Checklist");
        sb.AppendLine();
        sb.AppendLine("1. **CSS Bundle**");
        sb.AppendLine("   - Verify the Fluent UI CSS bundle is included");
        sb.AppendLine("   - Check the order of CSS imports (Fluent UI should load early)");
        sb.AppendLine();
        sb.AppendLine("2. **CSS Conflicts**");
        sb.AppendLine("   - Check for conflicting Bootstrap or other framework styles");
        sb.AppendLine("   - Consider removing unused CSS frameworks");
        sb.AppendLine();
        sb.AppendLine("3. **Theme Configuration**");
        sb.AppendLine("   - Verify theme is properly configured");
        sb.AppendLine("   - Check if `<FluentProviders>` has correct theme settings");
        sb.AppendLine();
        sb.AppendLine("4. **CSS Isolation**");
        sb.AppendLine("   - Check scoped CSS specificity");
        sb.AppendLine("   - Use browser DevTools to inspect applied styles");
        sb.AppendLine();
        sb.AppendLine("5. **Design Tokens**");
        sb.AppendLine("   - Verify CSS custom properties are being set");
        sb.AppendLine("   - Check for missing token definitions");
    }

    private static void GenerateBehaviorTroubleshooting(StringBuilder sb)
    {
        sb.AppendLine("### Behavior Issues Checklist");
        sb.AppendLine();
        sb.AppendLine("1. **Event Handlers**");
        sb.AppendLine("   - Check that callbacks are properly defined");
        sb.AppendLine("   - Verify async handlers are awaited");
        sb.AppendLine();
        sb.AppendLine("2. **Data Binding**");
        sb.AppendLine("   - Check @bind-Value syntax");
        sb.AppendLine("   - Verify property types match component expectations");
        sb.AppendLine();
        sb.AppendLine("3. **Component State**");
        sb.AppendLine("   - Ensure StateHasChanged() is called when needed");
        sb.AppendLine("   - Check for missing InvokeAsync for cross-thread calls");
        sb.AppendLine();
        sb.AppendLine("4. **Dialog/Drawer Service**");
        sb.AppendLine("   - Verify IDialogService is injected");
        sb.AppendLine("   - Check that dialog component inherits correctly");
        sb.AppendLine("   - Ensure FluentDialogBody is used in dialog components");
        sb.AppendLine();
        sb.AppendLine("5. **Parameter Types**");
        sb.AppendLine("   - Verify required parameters are provided");
        sb.AppendLine("   - Check for null reference exceptions");
    }

    private static void GeneratePerformanceTroubleshooting(StringBuilder sb)
    {
        sb.AppendLine("### Performance Issues Checklist");
        sb.AppendLine();
        sb.AppendLine("1. **DataGrid Optimization**");
        sb.AppendLine("   - Use virtualization for large datasets");
        sb.AppendLine("   - Set `DisplayMode=\"DataGridDisplayMode.Table\"` with virtualization");
        sb.AppendLine("   - Implement proper `ItemSize` for virtualized grids");
        sb.AppendLine();
        sb.AppendLine("2. **Reduce Re-renders**");
        sb.AppendLine("   - Use `@key` directive for list items");
        sb.AppendLine("   - Implement `ShouldRender()` where appropriate");
        sb.AppendLine("   - Avoid unnecessary StateHasChanged() calls");
        sb.AppendLine();
        sb.AppendLine("3. **Lazy Loading**");
        sb.AppendLine("   - Load data asynchronously");
        sb.AppendLine("   - Use pagination instead of loading all data");
        sb.AppendLine();
        sb.AppendLine("4. **Component Lifecycle**");
        sb.AppendLine("   - Dispose of resources properly");
        sb.AppendLine("   - Implement IDisposable/IAsyncDisposable");
    }

    private static void GenerateGeneralTroubleshooting(StringBuilder sb)
    {
        sb.AppendLine("### General Troubleshooting Checklist");
        sb.AppendLine();
        sb.AppendLine("1. **Installation Verification**");
        sb.AppendLine("   - Package installed: `Microsoft.FluentUI.AspNetCore.Components`");
        sb.AppendLine("   - Services registered: `AddFluentUIComponents()` in Program.cs");
        sb.AppendLine("   - CSS included in App.razor or index.html");
        sb.AppendLine("   - `<FluentProviders />` in MainLayout.razor");
        sb.AppendLine();
        sb.AppendLine("2. **Render Mode**");
        sb.AppendLine("   - Interactive rendering is required");
        sb.AppendLine("   - Check for static SSR which won't work with interactive components");
        sb.AppendLine();
        sb.AppendLine("3. **Browser DevTools**");
        sb.AppendLine("   - Check Console for errors");
        sb.AppendLine("   - Check Network tab for failed requests");
        sb.AppendLine("   - Inspect elements to see applied styles");
        sb.AppendLine();
        sb.AppendLine("4. **Clear Caches**");
        sb.AppendLine("   - Clear browser cache");
        sb.AppendLine("   - Delete bin/obj folders and rebuild");
        sb.AppendLine("   - Clear NuGet cache if needed");
        sb.AppendLine();
        sb.AppendLine("5. **Version Compatibility**");
        sb.AppendLine("   - Check .NET version requirements");
        sb.AppendLine("   - Verify package versions are compatible");
    }
}
