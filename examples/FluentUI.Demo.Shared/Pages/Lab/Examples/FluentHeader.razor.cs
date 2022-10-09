using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared
{
    public partial class FluentHeader : FluentComponentBase
    {
        protected string? ClassValue =>
            new CssBuilder(Class)
                .AddClass("fluent-header")
                .Build();

        protected string? StyleValue =>
            new StyleBuilder()
                .AddStyle("height", Height)
                .Build();


        [Parameter]
        public string Height { get; set; } = "50px";
    }
}
