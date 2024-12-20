using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentSearch : FluentInputBase<string?>
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Search/FluentSearch.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

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
    /// Gets or sets a regular expression that the value must match to pass validation.
    /// </summary>
    [Parameter]
    public string? Pattern { get; set; }

    /// <summary>
    /// Gets or sets the size of the text field.
    /// </summary>
    [Parameter]
    public int? Size { get; set; }

    /// <summary>
    /// Gets or sets the if spellcheck should be used.
    /// </summary>
    [Parameter]
    public bool? Spellcheck { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <see cref="AspNetCore.Components.Appearance"/>
    /// </summary>
    [Parameter]
    public FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Gets or sets whether a form or an input field should have autocomplete "on" or "off" or another value.
    /// </summary>
    [Parameter]
    public string? AutoComplete { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public FluentSearch()
    {
        Id = Identifier.NewId();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            await Module.InvokeVoidAsync("addAriaHidden", Id);

            if (AutoComplete != null)
            {
                await Module.InvokeVoidAsync("setControlAttribute", Id, "autocomplete", AutoComplete);
            }
        }
    }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
