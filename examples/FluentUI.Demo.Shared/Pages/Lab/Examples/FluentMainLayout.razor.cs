using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

public partial class FluentMainLayout : FluentComponentBase
{
    protected string? ClassValue =>
        new CssBuilder(Class)
            .Build();

    protected string? StyleValue =>
        new StyleBuilder()
            .AddStyle($"--fluent-header-height: {HeaderHeight}")
            .Build();

    [Parameter]
    public RenderFragment? Header { get; set; }

    [Parameter]
    public RenderFragment? SubHeader { get; set; }

    [Parameter]
    public string HeaderHeight { get; set; } = "50px";

    [Parameter]
    public string? NavMenuTitle { get; set; }

    [Parameter]
    public RenderFragment? NavMenuItems { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }
}
