using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSearch : FluentInputBase<string?>
{
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
    /// Gets or sets the visual appearance. See <see cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

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