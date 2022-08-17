using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSearch : FluentInputBase<string?>
{
    /// <summary>
    /// Gets or sets if the text area is readonly
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }

    /// <summary>
    /// Gets or sets if the text area is auto focussed
    /// </summary>
    [Parameter]
    public bool? Autofocus { get; set; }

    /// <summary>
    /// Gets or sets the placholder text
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Allows associating a <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see> to the element by <see href="https://developer.mozilla.org/en-US/docs/Web/API/Element/id">id</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// Gets or sets the maximum length
    /// </summary>
    [Parameter]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum length
    /// </summary>
    [Parameter]
    public int? MinLength { get; set; }

    /// <summary>
    /// A regular expression that the value must match to pass validation.
    /// </summary>
    [Parameter]
    public string? Pattern { get; set; }

    /// <summary>
    /// Gets or sets the size of the text field
    /// </summary>
    [Parameter]
    public int? Size { get; set; }

    /// <summary>
    /// Gets or sets the if spellcheck should be used
    /// </summary>
    [Parameter]
    public bool? Spellcheck { get; set; }

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
    /// <summary>
    /// Gets or sets the visual appearance. See <see cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}