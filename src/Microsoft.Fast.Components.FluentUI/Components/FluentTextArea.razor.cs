using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTextArea : FluentInputBase<string?>
{
    /// <summary>
    /// Gets or sets if the text area is readonly
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }

    /// <summary>
    /// Gets or sets if the text area is resizeable. See <see cref="FluentUI.Resize"/>
    /// </summary>
    [Parameter]
    public TextAreaResize? Resize { get; set; }

    /// <summary>
    /// Gets or sets if the text area is auto focussed
    /// </summary>
    [Parameter]
    public bool? Autofocus { get; set; }

    /// <summary>
    /// The <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">id</see> the <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/form">form</see> the element is associated to
    /// </summary>
    [Parameter]
    public string? Form { get; set; }

    /// <summary>
    /// Allows associating a <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see> to the element by <see href="https://developer.mozilla.org/en-US/docs/Web/API/Element/id">id</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// The maximum number of characters a user can enter.
    /// </summary>
    [Parameter]
    public int? Maxlength { get; set; }

    /// <summary>
    /// The minimum number of characters a user can enter.
    /// </summary>
    [Parameter]
    public int? Minlength { get; set; }

    /// <summary>
    /// The name of the element.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }


    /// <summary>
    /// Gets or sets the placholder text
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Sizes the element horizontally by a number of character columns.
    /// </summary>
    [Parameter]
    public int? Cols { get; set; }

    /// <summary>
    /// Sizes the element vertically by a number of character rows.
    /// </summary>
    [Parameter]
    public int? Rows { get; set; }

    /// <summary>
    /// Sets if the element is eligible for spell checking
    /// but the UA.
    /// </summary>
    [Parameter]
    public bool Spellcheck { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <see cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets if the text area is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the text area is required
    /// </summary>
    [Parameter]
    public bool? Required { get; set; }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}