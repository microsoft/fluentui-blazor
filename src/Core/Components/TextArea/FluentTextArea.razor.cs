using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTextArea : FluentInputBase<string?>
{
    /// <summary>
    /// Gets or sets a value indicating whether the text area is resizeable. See <see cref="AspNetCore.Components.TextAreaResize"/>
    /// </summary>
    [Parameter]
    public TextAreaResize? Resize { get; set; }

    /// <summary>
    /// Gets or sets the <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">id</see> the <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/form">form</see> the element is associated to.
    /// </summary>
    [Parameter]
    public string? Form { get; set; }

    /// <summary>
    /// Allows associating a <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see> to the element by <see href="https://developer.mozilla.org/en-US/docs/Web/API/Element/id">id</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of characters a user can enter.
    /// </summary>
    [Parameter]
    public int? Maxlength { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of characters a user can enter.
    /// </summary>
    [Parameter]
    public int? Minlength { get; set; }

    /// <summary>
    /// Gets or sets the size the element horizontally by a number of character columns.
    /// </summary>
    [Parameter]
    public int? Cols { get; set; }

    /// <summary>
    /// Gets or sets the size the element vertically by a number of character rows.
    /// </summary>
    [Parameter]
    public int? Rows { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is eligible for spell checking
    /// but the UA.
    /// </summary>
    [Parameter]
    public bool? Spellcheck { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <see cref="AspNetCore.Components.FluentInputAppearance"/>
    /// </summary>
    [Parameter]
    public FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
