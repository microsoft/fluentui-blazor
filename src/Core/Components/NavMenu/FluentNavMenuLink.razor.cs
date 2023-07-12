using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenuLink : FluentNavMenuItem, IDisposable
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-link")
        .AddClass("navmenu-child-element")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    private FluentTreeItem _treeItem = null!;

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
