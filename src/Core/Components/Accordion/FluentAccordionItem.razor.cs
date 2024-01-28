using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentAccordionItem : FluentComponentBase, IDisposable
{
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
    /// Gets or sets a value indicating whether the item is expanded or collapsed.
    /// </summary>
    [Parameter]
    public bool? Expanded { get; set; }

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

    public void Dispose() => Owner?.Unregister(this);
}
