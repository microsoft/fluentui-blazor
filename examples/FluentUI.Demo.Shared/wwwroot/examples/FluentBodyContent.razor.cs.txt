using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

public partial class FluentBodyContent : FluentComponentBase
{
    protected string? ClassValue =>
        new CssBuilder(Class)
            .AddClass("fluent-body-content")
            .Build();

    protected string? StyleValue =>
        new StyleBuilder()
            .AddStyle(Style)
            .Build();

}
