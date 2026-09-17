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
/// MCP prompts for creating drawer/panel components with Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class CreateDrawerPrompts
{
    /// <summary>
    /// Generates a prompt to help create a drawer (panel) with Fluent UI Blazor.
    /// </summary>
    /// <param name="drawerDescription">Description of the drawer content and purpose.</param>
    /// <param name="position">Position of the drawer: 'start' (left) or 'end' (right).</param>
    /// <param name="isModal">Whether the drawer should be modal.</param>
    [McpServerPrompt(Name = "create_drawer")]
    [Description("Generates code for creating a drawer (panel) component that slides in from the side, using the FluentUI DialogService.")]
    public static ChatMessage CreateDrawer(
        [Description("A description of what the drawer should contain and its purpose (e.g., 'navigation menu', 'settings panel', 'user profile editor')")]
        string drawerDescription,
        [Description("Position of the drawer: 'start' (left) or 'end' (right). Default is 'end'.")]
        string position = "end",
        [Description("Whether the drawer should be modal (blocks interaction with the rest of the page). Default is true.")]
        bool isModal = true)
    {
        var sb = new StringBuilder();
        AppendHeader(sb, drawerDescription, position, isModal);
        AppendDrawerComponent(sb);
        AppendParentComponent(sb, position, isModal);
        AppendKeyPoints(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendHeader(StringBuilder sb, string drawerDescription, string position, bool isModal)
    {
        sb.AppendLine("# Create a Fluent UI Blazor Drawer (Panel)");
        sb.AppendLine();
        sb.AppendLine("## Drawer Requirements");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Create a drawer with this purpose: **{drawerDescription}**");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Position:** {(position.Equals("start", StringComparison.OrdinalIgnoreCase) ? "Left (Start)" : "Right (End)")}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Modal:** {(isModal ? "Yes (blocks interaction with the page)" : "No (allows interaction with the page)")}");
        sb.AppendLine();
        sb.AppendLine("## Implementation Guide");
        sb.AppendLine();
    }

    private static void AppendDrawerComponent(StringBuilder sb)
    {
        sb.AppendLine("### Step 1: Create the Drawer Component");
        sb.AppendLine();
        sb.AppendLine("Create a new Razor component for your drawer that:");
        sb.AppendLine();
        sb.AppendLine("- Inherits from `FluentDialogInstance`");
        sb.AppendLine("- Uses `<FluentDialogBody>` to contain the drawer content");
        sb.AppendLine("- Optionally adds `[Parameter]` properties for data passed to the drawer");
        sb.AppendLine("- Overrides `OnInitializeDialog()` to customize header title and footer buttons");
        sb.AppendLine("- Overrides `OnActionClickedAsync()` to handle save/cancel actions");
        sb.AppendLine("- Uses `DialogInstance.CloseAsync()` to close with a result");
        sb.AppendLine("- Uses `DialogInstance.CancelAsync()` to cancel without a result");
        sb.AppendLine();
    }

    private static void AppendParentComponent(StringBuilder sb, string position, bool isModal)
    {
        sb.AppendLine("### Step 2: Show the Drawer from a Parent Component");
        sb.AppendLine();
        sb.AppendLine("To open the drawer from a parent component:");
        sb.AppendLine();
        sb.AppendLine("- Inject `IDialogService`");
        sb.AppendLine("- Call `ShowDrawerAsync<YourDrawerComponent>()` with options");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- Set `options.Alignment` to `DialogAlignment.{(position.Equals("start", StringComparison.OrdinalIgnoreCase) ? "Start" : "End")}`");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- Set `options.Modal` to `{(isModal ? "true" : "false")}`");
        sb.AppendLine("- Use `options.Parameters.Add()` to pass data to the drawer");
        sb.AppendLine("- Handle the result by checking `result.Cancelled` and `result.Value`");
        sb.AppendLine();
    }

    private static void AppendKeyPoints(StringBuilder sb)
    {
        sb.AppendLine("## Key Points");
        sb.AppendLine();
        sb.AppendLine("- **FluentDialogBody** is required to display the drawer correctly");
        sb.AppendLine("- **FluentDialogInstance** base class provides `DialogInstance` for closing/canceling");
        sb.AppendLine("- Use `options.Parameters.Add()` to pass data to the drawer");
        sb.AppendLine("- Use `DialogInstance.CloseAsync()` to close with a result");
        sb.AppendLine("- Use `DialogInstance.CancelAsync()` to cancel without a result");
        sb.AppendLine("- Modal drawers can be closed by clicking outside (light dismiss)");
        sb.AppendLine("- Non-modal drawers must be closed via action buttons");
        sb.AppendLine();
        sb.AppendLine("**Important:** Use the available MCP tools to retrieve component documentation and code examples for the drawer implementation.");
    }
}
