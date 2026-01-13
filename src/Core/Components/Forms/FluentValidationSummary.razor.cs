// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays a summary of validation messages for a specified model within a cascaded <see cref="EditContext"/>.
/// </summary>
public partial class FluentValidationSummary
{
    private IEnumerable<string>? _validationMessages;

    /// <summary />
    protected string? ClassValue => new CssBuilder()
        .AddClass("fluent-validation-errors")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("color", "var(--colorPaletteRedForeground1)", when: UseErrorTextColor)
        .AddStyle("color", "var(--colorNeutralForeground2)", when: !UseErrorTextColor)
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether error messages are displayed using a distinct error text color.
    /// </summary>
    [Parameter]
    public bool UseErrorTextColor { get; set; } = true;

    /// <summary>
    /// Gets or sets the <see cref="EditContext"/> for the form.
    /// </summary>
    [CascadingParameter]
    public EditContext FluentEditContext { get; set; } = default!;

    /// <summary />
    protected override void OnInitialized()
    {

        _validationMessages = Model is null
            ? FluentEditContext.GetValidationMessages()
            : FluentEditContext.GetValidationMessages(new FieldIdentifier(Model, string.Empty));
    }
}
