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
/// MCP prompts for creating dialog components with Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class CreateDialogPrompts
{
    /// <summary>
    /// Generates a prompt to help create a dialog with Fluent UI Blazor.
    /// </summary>
    /// <param name="dialogDescription">Description of the dialog content and purpose.</param>
    /// <param name="dialogType">Type of dialog: 'simple', 'form', 'confirmation', or 'custom'.</param>
    /// <param name="size">Size of the dialog: 'small', 'medium', 'large', or 'max'.</param>
    [McpServerPrompt(Name = "create_dialog")]
    [Description("Generates code for creating a modal dialog component using the FluentUI DialogService.")]
    public static ChatMessage CreateDialog(
        [Description("A description of what the dialog should contain and its purpose (e.g., 'confirm delete action', 'edit user profile', 'display details')")]
        string dialogDescription,
        [Description("Type of dialog: 'simple' (basic content), 'form' (with input fields), 'confirmation' (yes/no action), or 'custom' (fully customized). Default is 'simple'.")]
        string dialogType = "simple",
        [Description("Size of the dialog: 'small', 'medium', 'large', or 'max'. Default is 'medium'.")]
        string size = "medium")
    {
        var sb = new StringBuilder();
        AppendHeader(sb, dialogDescription, dialogType, size);
        AppendDialogGuide(sb, dialogType);
        AppendOpeningDialog(sb, size);
        AppendQuickMethods(sb);
        AppendKeyPointsAndBestPractices(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendHeader(StringBuilder sb, string dialogDescription, string dialogType, string size)
    {
        sb.AppendLine("# Create a Fluent UI Blazor Dialog");
        sb.AppendLine();
        sb.AppendLine("## Dialog Requirements");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Create a dialog with this purpose: **{dialogDescription}**");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Type:** {dialogType}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- **Size:** {size}");
        sb.AppendLine();
        sb.AppendLine("## Implementation Guide");
        sb.AppendLine();
    }

    private static void AppendDialogGuide(StringBuilder sb, string dialogType)
    {
        switch (dialogType)
        {
            case var type when string.Equals(type, "confirmation", System.StringComparison.OrdinalIgnoreCase):
                GenerateConfirmationDialogGuide(sb);
                break;
            case var type when string.Equals(type, "form", System.StringComparison.OrdinalIgnoreCase):
                GenerateFormDialogGuide(sb);
                break;
            case var type when string.Equals(type, "custom", System.StringComparison.OrdinalIgnoreCase):
                GenerateCustomDialogGuide(sb);
                break;
            default:
                GenerateSimpleDialogGuide(sb);
                break;
        }

        sb.AppendLine();
    }

    private static void AppendOpeningDialog(StringBuilder sb, string size)
    {
        sb.AppendLine("### Opening the Dialog");
        sb.AppendLine();
        sb.AppendLine("To open a dialog from a parent component:");
        sb.AppendLine();
        sb.AppendLine("- Inject `IDialogService`");
        sb.AppendLine("- Call `ShowDialogAsync<YourDialogComponent>()` with options");
        sb.AppendLine(CultureInfo.InvariantCulture, $"- Set `options.Size` to `DialogSize.{(string.IsNullOrEmpty(size) ? "Medium" : char.ToUpperInvariant(size[0]) + size[1..].ToLowerInvariant())}`");
        sb.AppendLine("- Set `options.Modal` to `true` for modal dialogs");
        sb.AppendLine("- Use `options.Parameters.Add()` to pass data to the dialog");
        sb.AppendLine("- Handle the result by checking `result.Cancelled`");
        sb.AppendLine();
    }

    private static void AppendQuickMethods(StringBuilder sb)
    {
        sb.AppendLine("## Quick Dialog Methods");
        sb.AppendLine();
        sb.AppendLine("For simple scenarios, use built-in message box methods from `IDialogService`:");
        sb.AppendLine();
        sb.AppendLine("- `ShowInfoAsync()` - Display an info message");
        sb.AppendLine("- `ShowConfirmationAsync()` - Confirmation dialog with yes/no");
        sb.AppendLine("- `ShowWarningAsync()` - Display a warning message");
        sb.AppendLine("- `ShowErrorAsync()` - Display an error message");
        sb.AppendLine();
    }

    private static void AppendKeyPointsAndBestPractices(StringBuilder sb)
    {
        sb.AppendLine("## Key Points");
        sb.AppendLine();
        sb.AppendLine("- **FluentDialogBody** is required inside your dialog component");
        sb.AppendLine("- Inherit from **FluentDialogInstance** for automatic button handling");
        sb.AppendLine("- Use **TitleTemplate**, **ChildContent**, and **ActionTemplate** for custom layouts");
        sb.AppendLine("- Use `options.Parameters.Add()` to pass data to the dialog");
        sb.AppendLine("- Handle results with `DialogInstance.CloseAsync(result)` or `DialogInstance.CancelAsync()`");
        sb.AppendLine();
        sb.AppendLine("## Best Practices");
        sb.AppendLine();
        sb.AppendLine("- Don't open a dialog from within another dialog");
        sb.AppendLine("- Limit to 3 action buttons maximum");
        sb.AppendLine("- Ensure at least one focusable element exists");
        sb.AppendLine("- Use appropriate sizes for the content");
        sb.AppendLine();
        sb.AppendLine("**Important:** Use the available MCP tools to retrieve component documentation and code examples for the dialog implementation.");
    }

    private static void GenerateSimpleDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Simple Dialog Component");
        sb.AppendLine();
        sb.AppendLine("For a simple dialog:");
        sb.AppendLine();
        sb.AppendLine("- Inherit from `FluentDialogInstance`");
        sb.AppendLine("- Use `<FluentDialogBody>` to contain your content");
        sb.AppendLine("- Override `OnInitializeDialog()` to set title and footer options");
    }

    private static void GenerateFormDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Form Dialog Component");
        sb.AppendLine();
        sb.AppendLine("For a form dialog:");
        sb.AppendLine();
        sb.AppendLine("- Inherit from `FluentDialogInstance`");
        sb.AppendLine("- Use `<FluentDialogBody>` containing an `<EditForm>`");
        sb.AppendLine("- Add `<DataAnnotationsValidator />` and form inputs");
        sb.AppendLine("- Override `OnInitializeDialog()` to set title and action labels");
        sb.AppendLine("- Override `OnActionClickedAsync()` to handle save/cancel actions");
    }

    private static void GenerateConfirmationDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Confirmation Dialog");
        sb.AppendLine();
        sb.AppendLine("For simple confirmations, use the built-in `ShowConfirmationAsync()` method.");
        sb.AppendLine();
        sb.AppendLine("For custom confirmation dialogs:");
        sb.AppendLine();
        sb.AppendLine("- Inherit from `FluentDialogInstance`");
        sb.AppendLine("- Use `<FluentDialogBody>` with warning icon and message");
        sb.AppendLine("- Override `OnInitializeDialog()` to set Yes/No button labels");
    }

    private static void GenerateCustomDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Fully Custom Dialog Component");
        sb.AppendLine();
        sb.AppendLine("For full control over the dialog:");
        sb.AppendLine();
        sb.AppendLine("- Use `<FluentDialogBody>` with template sections");
        sb.AppendLine("- Add `<TitleTemplate>` for custom header content");
        sb.AppendLine("- Use `<ChildContent>` for the main dialog body");
        sb.AppendLine("- Add `<ActionTemplate>` for custom action buttons");
        sb.AppendLine("- Access the dialog instance via `[CascadingParameter] IDialogInstance Dialog`");
    }
}
