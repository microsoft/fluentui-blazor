using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentCard
{
    protected string? StyleValue => new StyleBuilder(Style)
       .AddStyle("content-visibility", "visible", !AreaRestricted)
       .AddStyle("contain", "style", !AreaRestricted)
       .Build();
    
    /// <summary>
    /// By default, content in the card is restricted to the area of the card itself. 
    /// If you want content to be able to overflow the card, set this property to false.
    /// </summary>
    [Parameter]
    public bool AreaRestricted { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
