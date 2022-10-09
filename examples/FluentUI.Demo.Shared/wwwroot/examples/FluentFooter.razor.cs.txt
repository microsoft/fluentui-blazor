using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared
{
    public partial class FluentFooter : FluentComponentBase
    {
        protected string? ClassValue =>
            new CssBuilder(Class)
                .AddClass("fluent-footer")
                .Build();

        protected string? StyleValue =>
            new StyleBuilder()
                .Build();
    }
}
