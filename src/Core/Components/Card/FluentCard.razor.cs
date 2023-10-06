using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentCard
{
    protected string? StyleValue => new StyleBuilder(Style)
       .AddStyle("content-visibility", "visible", !AreaRestricted)
       .AddStyle("contain", "style", !AreaRestricted)
       .Build();
    
    /// <summary>
    /// By default, content in the card is restricted to the area of the card itself. 
    /// If you want content to be able to overflow the card, set this property to false.
    [Parameter]
    public bool AreaRestricted { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}