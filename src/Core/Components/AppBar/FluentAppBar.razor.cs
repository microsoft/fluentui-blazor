using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;


public partial class FluentAppBar : FluentComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal string? ClassValue => new CssBuilder("nav-menu-container")
        .AddClass(Class)
        .Build();
}
