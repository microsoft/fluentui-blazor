using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

public partial class FluentLayout : FluentComponentBase
{
    protected string? ClassValue =>
        new CssBuilder(Class)
            .AddClass("fluent-layout")
            .Build();

    protected string? StyleValue =>
        new StyleBuilder()
            .AddStyle(Style)
            .Build();
}
