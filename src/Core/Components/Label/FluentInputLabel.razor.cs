using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentInputLabel
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/Label/FluentInputLabel.razor.js";

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the HTML label `for` attribute.
    /// See https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/for
    /// </summary>
    [Parameter]
    public string? ForId { get; set; }

    /// <summary>
    /// Gets or sets the text to be displayed as a label, just above the component.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Content displayed just above the component
    /// </summary>
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed as a label, just above the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the text to be used as the `aria-label` attribute of the input.
    /// If not set, the <see cref="Label"/> will be used.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }


    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && ShouldRenderAriaLabel)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await Module.InvokeVoidAsync("setInputAriaLabel", ForId, string.IsNullOrWhiteSpace(Title) ? Label : Title);
        }
    }

    /// <summary />
    private bool ShouldRenderAriaLabel => !string.IsNullOrWhiteSpace(ForId)
                                       && (!string.IsNullOrWhiteSpace(Label) ||
                                           !string.IsNullOrWhiteSpace(Title));
}
