using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentBreadcrumbItem : FluentComponentBase
{
    /// <summary>
    /// The URL this item references.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }
}