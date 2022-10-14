using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

public partial class Layout : FluentComponentBase
{
    protected string? ClassValue =>
        new CssBuilder(Class)
            .AddClass("layout")
            .Build();

    protected string? StyleValue =>
        new StyleBuilder()
            .AddStyle(Style)
            .Build();
}
