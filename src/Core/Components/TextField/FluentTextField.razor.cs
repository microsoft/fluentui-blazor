using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTextField : FluentInputBase<string?>
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/TextField/FluentTextField.razor.js";

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the text filed type. See <see cref="FluentUI.TextFieldType"/>
    /// </summary>
    [Parameter]
    public TextFieldType? TextFieldType { get; set; }

    /// <summary>
    /// Allows associating a <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see> to the element by <see href="https://developer.mozilla.org/en-US/docs/Web/API/Element/id">id</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// Gets or sets the maximum length
    /// </summary>
    [Parameter]
    public int? Maxlength { get; set; }

    /// <summary>
    /// Gets or sets the minimum length
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
    /// Gets or sets the visual appearance. See <see cref="FluentUI.FluentInputAppearance"/>
    /// </summary>
    [Parameter]
    public FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Specifies whether a form or an input field should have autocomplete on or off.
    /// An Id value must be set to use this property.
    /// </summary>
    [Parameter]
    public bool? AutoComplete { get; set; }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (AutoComplete != null && !string.IsNullOrEmpty(Id))
            {
                Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
                var autocomplete = AutoComplete == true ? "on" : "off";
                await Module.InvokeVoidAsync("setAutocomplete", Id, autocomplete);
            }
        }
    }
}