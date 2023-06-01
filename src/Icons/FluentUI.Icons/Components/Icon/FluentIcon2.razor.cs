using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentIcon2 : FluentComponentBase
{
    private string _icon = string.Empty;

    /// <summary>
    /// Icon to be used can either be svg paths.
    /// </summary>
    [Parameter]
    public string Icon
    {
        get
        {
            return _icon;
        }
        set
        {
            _icon = value;

            if (Size == null)
            {
                var iconDetails = value.ExtractSystemIconDetails();
                Size = iconDetails.Size > 0 ? iconDetails.Size : null;
            }
        }
    }

    /// <summary>
    /// Gets or sets the slot where the icon is displayed in
    /// </summary>
    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets or sets the title for the icon
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = null;

    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="FluentUI.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the icon size, used to create the viewbox, width and height attributes.
    /// </summary>
    [Parameter]
    public int? Size { get; set; }

    /// <summary>
    /// Child content of component.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Allows for capturing a mouse click on an icon
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary />
    protected Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            return OnClick.InvokeAsync(e);
        }

        return Task.CompletedTask;
    }
}
