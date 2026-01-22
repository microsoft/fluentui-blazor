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
/// MCP prompts for creating forms with Fluent UI Blazor components.
/// </summary>
[McpServerPromptType]
public class CreateFormPrompts
{
    /// <summary>
    /// Generates a prompt to help create a form with Fluent UI Blazor components.
    /// </summary>
    /// <param name="formDescription">Description of the form to create.</param>
    /// <param name="includeValidation">Whether to include form validation.</param>
    /// <param name="useEditForm">Whether to use EditForm with DataAnnotations.</param>
    [McpServerPrompt(Name = "create_form")]
    [Description("Generates code and instructions for creating a form with Fluent UI Blazor components, including validation and data binding.")]
    public static ChatMessage CreateForm(
        [Description("A description of the form to create (e.g., 'user registration form with name, email, password', 'contact form with message')")]
        string formDescription,
        [Description("Whether to include form validation using DataAnnotations (default: true)")]
        bool includeValidation = true,
        [Description("Whether to wrap the form in an EditForm component (default: true)")]
        bool useEditForm = true)
    {
        var sb = new StringBuilder();
        AppendHeader(sb, formDescription);
        AppendFormStructure(sb, useEditForm);
        AppendInputComponents(sb);
        AppendValidationSection(sb, includeValidation);
        AppendLayoutSection(sb);
        AppendExampleStructure(sb, useEditForm, includeValidation);
        AppendRequirements(sb, includeValidation);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendHeader(StringBuilder sb, string formDescription)
    {
        sb.AppendLine("# Create a Fluent UI Blazor Form");
        sb.AppendLine();
        sb.AppendLine("## Form Requirements");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Create a form based on this description: **{formDescription}**");
        sb.AppendLine();
        sb.AppendLine("## Guidelines");
        sb.AppendLine();
        sb.AppendLine("Please generate a complete form implementation that follows these guidelines:");
        sb.AppendLine();
    }

    private static void AppendFormStructure(StringBuilder sb, bool useEditForm)
    {
        sb.AppendLine("### Form Structure");
        sb.AppendLine();

        if (useEditForm)
        {
            sb.AppendLine("- Use `<EditForm>` component as the container");
            sb.AppendLine("- Set `novalidate=\"true\"` on the EditForm to use Fluent UI validation styling");
            sb.AppendLine("- Use `OnValidSubmit` for form submission handling");
        }
        else
        {
            sb.AppendLine("- Create a simple form without EditForm wrapper");
            sb.AppendLine("- Handle form submission manually");
        }

        sb.AppendLine();
    }

    private static void AppendInputComponents(StringBuilder sb)
    {
        sb.AppendLine("### Available Input Components");
        sb.AppendLine();
        sb.AppendLine("Use these Fluent UI Blazor components for form fields:");
        sb.AppendLine();
        sb.AppendLine("- `FluentTextInput` - For single-line text input");
        sb.AppendLine("- `FluentTextArea` - For multi-line text input");
        sb.AppendLine("- `FluentSelect` / `FluentCombobox` - For dropdown selections");
        sb.AppendLine("- `FluentCheckbox` - For boolean values");
        sb.AppendLine("- `FluentSwitch` - For toggle switches");
        sb.AppendLine("- `FluentDatePicker` - For date selection");
        sb.AppendLine("- `FluentTimePicker` - For time selection");
        sb.AppendLine("- `FluentRadio` / `FluentRadioGroup` - For radio button selections");
        sb.AppendLine("- `FluentSlider` - For numeric range input");
        sb.AppendLine("- `FluentNumberField` - For numeric input");
    }

    private static void AppendValidationSection(StringBuilder sb, bool includeValidation)
    {
        if (includeValidation)
        {
            sb.AppendLine();
            sb.AppendLine("### Validation");
            sb.AppendLine();
            sb.AppendLine("- Include a model class with DataAnnotation attributes");
            sb.AppendLine("- Add `<DataAnnotationsValidator />` inside the EditForm");
            sb.AppendLine("- Use `<FluentValidationSummary />` for validation summary");
            sb.AppendLine("- Set `Required=\"true\"` on required fields for visual indication");
        }
    }

    private static void AppendLayoutSection(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("### Layout");
        sb.AppendLine();
        sb.AppendLine("- Use `<FluentStack Orientation=\"Orientation.Vertical\">` to stack form fields");
        sb.AppendLine("- Add appropriate labels using the `Label` parameter on input components");
        sb.AppendLine("- Include a submit button: `<FluentButton Type=\"ButtonType.Submit\" Appearance=\"ButtonAppearance.Primary\">`");
        sb.AppendLine();
    }

    private static void AppendExampleStructure(StringBuilder sb, bool useEditForm, bool includeValidation)
    {
        sb.AppendLine("### Example Structure");
        sb.AppendLine();
        sb.AppendLine("```razor");

        if (useEditForm)
        {
            sb.AppendLine("<EditForm Model=\"@model\" OnValidSubmit=\"@HandleValidSubmit\" novalidate=\"true\">");

            if (includeValidation)
            {
                sb.AppendLine("    <DataAnnotationsValidator />");
                sb.AppendLine("    <FluentValidationSummary />");
            }

            sb.AppendLine("    <FluentStack Orientation=\"Orientation.Vertical\">");
            sb.AppendLine("        <FluentTextInput @bind-Value=\"model.PropertyName\" Label=\"Field Label\" Required=\"true\" />");
            sb.AppendLine("        <!-- Add more fields as needed -->");
            sb.AppendLine("        <FluentButton Type=\"ButtonType.Submit\" Appearance=\"ButtonAppearance.Primary\">Submit</FluentButton>");
            sb.AppendLine("    </FluentStack>");
            sb.AppendLine("</EditForm>");
        }
        else
        {
            sb.AppendLine("<FluentStack Orientation=\"Orientation.Vertical\">");
            sb.AppendLine("    <FluentTextInput @bind-Value=\"model.PropertyName\" Label=\"Field Label\" />");
            sb.AppendLine("    <!-- Add more fields as needed -->");
            sb.AppendLine("    <FluentButton OnClick=\"@HandleSubmit\" Appearance=\"ButtonAppearance.Primary\">Submit</FluentButton>");
            sb.AppendLine("</FluentStack>");
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendRequirements(StringBuilder sb, bool includeValidation)
    {
        sb.AppendLine("## Requirements");
        sb.AppendLine();
        sb.AppendLine("Generate the complete implementation including:");
        sb.AppendLine();
        sb.AppendLine("1. The Razor component markup");
        sb.AppendLine("2. The @code block with model and event handlers");

        if (includeValidation)
        {
            sb.AppendLine("3. A model class with appropriate DataAnnotation validation attributes");
        }
    }
}
