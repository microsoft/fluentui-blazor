using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAccordionItem : FluentComponentBase, IDisposable
{
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

    /// <summary>
    /// Configures the <see link="https://www.w3.org/TR/wai-aria-1.1/#aria-level">level</see> of the
    /// heading element.
    /// Possible values: 1 | 2 | 3 | 4 | 5 | 6
    /// </summary>
    [Parameter]
    public string? HeadingLevel { get; set; }

    /// <summary>
    /// The item ID
    /// </summary>
    internal string Id { get; } = Identifier.NewId();


    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    public void Dispose()
    {
        Owner?.Unregister(this);
    }
}