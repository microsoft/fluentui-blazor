using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

public partial class FluentLayout : FluentComponentBase
{
    protected string? _class =>
        new CssBuilder(Class)
            .AddClass("fluent-layout")
            .Build();

    protected string? _style =>
        new StyleBuilder()
            .AddStyle(Style)
            .Build();
}
