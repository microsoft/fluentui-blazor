using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentNavLink : FluentNavBase
{
    private readonly RenderFragment _renderContent;

    internal string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-nav-item")
        .Build();

    internal string? LinkClassValue => new CssBuilder("fluent-nav-link")
        .AddClass($"disabled", Disabled)
        .Build();

    internal Dictionary<string, object?> Attributes
    {
        get => Disabled ? [] : new Dictionary<string, object?>
        {
            { "href", Href },
            { "target", Target },
            { "rel", !string.IsNullOrWhiteSpace(Target) ? "noopener noreferrer" : string.Empty }
        };
    }

    public FluentNavLink()
    {
        _renderContent = RenderContent;
    }
}
