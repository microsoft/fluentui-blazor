using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordionItem : FluentComponentBase, IDisposable
{
    internal string AccordionItemId { get; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the owning FluentTreeView
    /// </summary>
    [CascadingParameter]
    public FluentAccordion Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the heading of the accordion item.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Heading { get; set; } = string.Empty;

    /// <summary>
    /// Expands or collapses the item.
    /// </summary>
    [Parameter]
    public bool? Expanded { get; set; }

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    public void Dispose()
    {
        Owner.Unregister(this);
    }
}