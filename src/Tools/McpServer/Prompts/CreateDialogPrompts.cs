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
        sb.AppendLine("```razor");
        sb.AppendLine("@inject IDialogService DialogService");
        sb.AppendLine();
        sb.AppendLine("<FluentButton OnClick=\"@OpenDialogAsync\">Open Dialog</FluentButton>");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    private async Task OpenDialogAsync()");
        sb.AppendLine("    {");
        sb.AppendLine("        var result = await DialogService.ShowDialogAsync<YourDialogComponent>(options =>");
        sb.AppendLine("        {");

        var sizeValue = string.IsNullOrEmpty(size)
            ? "Medium"
            : char.ToUpperInvariant(size[0]) + size[1..].ToLowerInvariant();
        sb.AppendLine(CultureInfo.InvariantCulture, $"            options.Size = DialogSize.{sizeValue};");
        sb.AppendLine("            options.Modal = true;");
        sb.AppendLine("            ");
        sb.AppendLine("            // Optional: Pass parameters");
        sb.AppendLine("            options.Parameters.Add(\"ParameterName\", value);");
        sb.AppendLine("        });");
        sb.AppendLine();
        sb.AppendLine("        if (!result.Cancelled)");
        sb.AppendLine("        {");
        sb.AppendLine("            // Handle the result");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendQuickMethods(StringBuilder sb)
    {
        sb.AppendLine("## Quick Dialog Methods");
        sb.AppendLine();
        sb.AppendLine("For simple scenarios, use built-in message box methods:");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("// Simple info message");
        sb.AppendLine("await DialogService.ShowInfoAsync(\"Title\", \"Message\");");
        sb.AppendLine();
        sb.AppendLine("// Confirmation dialog");
        sb.AppendLine("var confirmed = await DialogService.ShowConfirmationAsync(\"Are you sure?\");");
        sb.AppendLine();
        sb.AppendLine("// Warning message");
        sb.AppendLine("await DialogService.ShowWarningAsync(\"Warning Title\", \"Warning message\");");
        sb.AppendLine();
        sb.AppendLine("// Error message");
        sb.AppendLine("await DialogService.ShowErrorAsync(\"Error Title\", \"Error details\");");
        sb.AppendLine("```");
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
        sb.AppendLine("Please generate the complete implementation based on the requirements.");
    }

    private static void GenerateSimpleDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Simple Dialog Component");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("@inherits FluentDialogInstance");
        sb.AppendLine();
        sb.AppendLine("<FluentDialogBody>");
        sb.AppendLine("    <p>Your dialog content goes here.</p>");
        sb.AppendLine("</FluentDialogBody>");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    protected override void OnInitializeDialog(DialogOptionsHeader header, DialogOptionsFooter footer)");
        sb.AppendLine("    {");
        sb.AppendLine("        header.Title = \"Dialog Title\";");
        sb.AppendLine("        footer.SecondaryAction.Visible = false; // Hide cancel button");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
    }

    private static void GenerateFormDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Form Dialog Component");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("@inherits FluentDialogInstance");
        sb.AppendLine();
        sb.AppendLine("<FluentDialogBody>");
        sb.AppendLine("    <EditForm Model=\"@model\" OnValidSubmit=\"@HandleSubmit\">");
        sb.AppendLine("        <DataAnnotationsValidator />");
        sb.AppendLine("        <FluentStack Orientation=\"Orientation.Vertical\">");
        sb.AppendLine("            <FluentTextInput @bind-Value=\"model.Name\" Label=\"Name\" Required=\"true\" />");
        sb.AppendLine("            <FluentTextInput @bind-Value=\"model.Email\" Label=\"Email\" Required=\"true\" />");
        sb.AppendLine("        </FluentStack>");
        sb.AppendLine("    </EditForm>");
        sb.AppendLine("</FluentDialogBody>");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    private MyModel model = new();");
        sb.AppendLine();
        sb.AppendLine("    protected override void OnInitializeDialog(DialogOptionsHeader header, DialogOptionsFooter footer)");
        sb.AppendLine("    {");
        sb.AppendLine("        header.Title = \"Edit Information\";");
        sb.AppendLine("        footer.PrimaryAction.Label = \"Save\";");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    protected override async Task OnActionClickedAsync(bool primary)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (primary)");
        sb.AppendLine("            await DialogInstance.CloseAsync(model);");
        sb.AppendLine("        else");
        sb.AppendLine("            await DialogInstance.CancelAsync();");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    private async Task HandleSubmit()");
        sb.AppendLine("    {");
        sb.AppendLine("        await DialogInstance.CloseAsync(model);");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
    }

    private static void GenerateConfirmationDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Confirmation Dialog");
        sb.AppendLine();
        sb.AppendLine("For simple confirmations, use the built-in method:");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("var result = await DialogService.ShowConfirmationAsync(");
        sb.AppendLine("    \"Are you sure you want to delete this item?\",");
        sb.AppendLine("    \"Confirm Delete\");");
        sb.AppendLine();
        sb.AppendLine("if (result.Cancelled == false)");
        sb.AppendLine("{");
        sb.AppendLine("    // User confirmed, perform delete");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("For custom confirmation dialogs:");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("@inherits FluentDialogInstance");
        sb.AppendLine();
        sb.AppendLine("<FluentDialogBody>");
        sb.AppendLine("    <FluentIcon Value=\"@(new Icons.Regular.Size24.Warning())\" Color=\"Color.Warning\" />");
        sb.AppendLine("    <p>@Message</p>");
        sb.AppendLine("</FluentDialogBody>");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    [Parameter]");
        sb.AppendLine("    public string Message { get; set; } = \"Are you sure?\";");
        sb.AppendLine();
        sb.AppendLine("    protected override void OnInitializeDialog(DialogOptionsHeader header, DialogOptionsFooter footer)");
        sb.AppendLine("    {");
        sb.AppendLine("        header.Title = \"Confirm Action\";");
        sb.AppendLine("        footer.PrimaryAction.Label = \"Yes\";");
        sb.AppendLine("        footer.SecondaryAction.Label = \"No\";");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
    }

    private static void GenerateCustomDialogGuide(StringBuilder sb)
    {
        sb.AppendLine("### Fully Custom Dialog Component");
        sb.AppendLine();
        sb.AppendLine("Use `FluentDialogBody` with templates for full control:");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("<FluentDialogBody>");
        sb.AppendLine("    <TitleTemplate>");
        sb.AppendLine("        <FluentStack>");
        sb.AppendLine("            <FluentIcon Value=\"@(new Icons.Regular.Size24.Settings())\" />");
        sb.AppendLine("            <span>@Dialog.Options.Header.Title</span>");
        sb.AppendLine("        </FluentStack>");
        sb.AppendLine("    </TitleTemplate>");
        sb.AppendLine();
        sb.AppendLine("    <ChildContent>");
        sb.AppendLine("        <p>Your custom content here</p>");
        sb.AppendLine("    </ChildContent>");
        sb.AppendLine();
        sb.AppendLine("    <ActionTemplate>");
        sb.AppendLine("        <FluentButton OnClick=\"@(e => Dialog.CancelAsync())\">Cancel</FluentButton>");
        sb.AppendLine("        <FluentButton OnClick=\"@CustomAction\">Custom Action</FluentButton>");
        sb.AppendLine("        <FluentButton OnClick=\"@(e => Dialog.CloseAsync())\" Appearance=\"ButtonAppearance.Primary\">OK</FluentButton>");
        sb.AppendLine("    </ActionTemplate>");
        sb.AppendLine("</FluentDialogBody>");
        sb.AppendLine();
        sb.AppendLine("@code {");
        sb.AppendLine("    [CascadingParameter]");
        sb.AppendLine("    public required IDialogInstance Dialog { get; set; }");
        sb.AppendLine();
        sb.AppendLine("    private async Task CustomAction()");
        sb.AppendLine("    {");
        sb.AppendLine("        // Perform custom action");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("```");
    }
}
