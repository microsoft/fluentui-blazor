// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;

/// <summary>
/// MCP Prompt for generating Fluent UI Blazor forms.
/// </summary>
[McpServerPromptType]
public class CreateFormPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFormPrompt"/> class.
    /// </summary>
    public CreateFormPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "create_form")]
    [Description("Generate a complete Fluent UI Blazor form with validation.")]
    public ChatMessage CreateForm(
        [Description("Describe the form fields needed (e.g., 'name, email, phone, address with country dropdown')")] string formFields,
        [Description("Optional: the model class name for the form data")] string? modelName = null,
        [Description("Optional: validation requirements (e.g., 'email required and valid, phone optional')")] string? validation = null)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Create a Fluent UI Blazor Form");
        sb.AppendLine();

        sb.AppendLine("## Available Form Components");
        sb.AppendLine();

        // List relevant form components
        var formComponents = new[] { "FluentTextField", "FluentTextArea", "FluentSelect", "FluentCheckbox", "FluentRadio", "FluentSwitch", "FluentDatePicker", "FluentTimePicker", "FluentNumberField" };

        foreach (var compName in formComponents)
        {
            var comp = _documentationService.GetAllComponents().FirstOrDefault(c => c.Name == compName);
            if (comp != null)
            {
                sb.AppendLine($"- **{comp.Name}**: {comp.Summary}");
            }
        }

        sb.AppendLine();
        sb.AppendLine("## Form Requirements");
        sb.AppendLine();
        sb.AppendLine($"**Fields**: {formFields}");

        if (!string.IsNullOrEmpty(modelName))
        {
            sb.AppendLine($"**Model Class**: {modelName}");
        }

        if (!string.IsNullOrEmpty(validation))
        {
            sb.AppendLine($"**Validation**: {validation}");
        }

        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Generate a complete Fluent UI Blazor form that includes:");
        sb.AppendLine("1. A model class with appropriate data annotations");
        sb.AppendLine("2. An EditForm with proper data binding");
        sb.AppendLine("3. FluentValidationMessage components for each field");
        sb.AppendLine("4. Appropriate Fluent UI input components for each field type");
        sb.AppendLine("5. Submit and cancel buttons");
        sb.AppendLine("6. Form submission handling");
        sb.AppendLine();
        sb.AppendLine("Use best practices for Blazor forms and validation.");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}
