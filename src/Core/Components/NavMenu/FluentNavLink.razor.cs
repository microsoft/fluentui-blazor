using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavLink : FluentNavBase
{
    private readonly RenderFragment _renderContent;

    internal string? ClassValue => new CssBuilder("fluent-nav-item")
        .AddClass(Class)
        .Build();

    internal string? LinkClassValue => new CssBuilder("fluent-nav-link")
        .AddClass($"disabled", Disabled)
        .Build();

    internal Dictionary<string, object?> Attributes
    {
        get => Disabled ? new Dictionary<string, object?>() : new Dictionary<string, object?>
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