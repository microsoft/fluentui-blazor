// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentAccordionItem : FluentComponentBase, IDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Accordion/FluentAccordionItem.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTreeView.
    /// </summary>
    [CascadingParameter]
    public FluentAccordion Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the heading of the accordion item.
    /// Use either this or the <see cref="HeadingTemplate"/> parameter."/>
    /// If both are set, this parameter will be used.
    /// </summary>
    [Parameter]
    public string? Heading { get; set; }

    /// <summary>
    /// Gets or sets the heading content of the accordion item.
    /// Use either this or the <see cref="Heading"/> parameter."/>
    /// If both are set, this parameter will not be used.
    /// </summary>
    [Parameter]
    public RenderFragment? HeadingTemplate { get; set; }

    /// <summary>
    /// Gets or sets the tooltip for the heading of the accordion item.
    /// </summary>
    [Parameter]
    public string? HeadingTooltip { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is expanded or collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; } = false;

    /// <summary>
    /// Gets or sets a callback for when the expanded state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Gets or sets the <see href="https://www.w3.org/TR/wai-aria-1.1/#aria-level">level</see> of the heading element.
    /// Possible values: 1 | 2 | 3 | 4 | 5 | 6
    /// </summary>
    [Parameter]
    public string? HeadingLevel { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public FluentAccordionItem()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (HeadingTooltip != null && !string.IsNullOrEmpty(Id))
            {
                Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
                await Module.InvokeVoidAsync("setControlAttribute", Id, "title", HeadingTooltip);
            }
        }
    }

    private async Task HandleOnAccordionItemChangedAsync(AccordionChangeEventArgs args)
    {
        if (args is not null)
        {
            var id = args.ActiveId;
            if (id is not null && Id == id && ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(args.Expanded);
            }
        }
    }

    public void Dispose() => Owner?.Unregister(this);
}
