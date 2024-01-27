using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

//[Obsolete("This component has been replaced with the FluentNavLink and will be removed in a future version.")]
public partial class FluentNavMenuLink : FluentNavMenuItemBase, IDisposable
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-link")
        .AddClass("navmenu-child-element")
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .Build();

    public FluentNavMenuLink()
    {
        Id = Identifier.NewId();
    }

    private async Task HandleSelectedChangedAsync(bool value)
    {
        if (value == Selected)
        {
            return;
        }

        Selected = value;
        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(value);
        }
    }
}
