using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentBreadcrumbItem : FluentComponentBase
{
    [Parameter]
    public string? Href { get; set; }
}