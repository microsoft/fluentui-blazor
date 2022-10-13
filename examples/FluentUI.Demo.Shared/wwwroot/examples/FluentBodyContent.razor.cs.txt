using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
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
