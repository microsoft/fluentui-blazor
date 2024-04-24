using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentEmoji is a component that renders an emoji from the Microsoft FluentUI emoji set.
/// </summary>
public partial class FluentEmoji<Emoji> : FluentComponentBase
    where Emoji : AspNetCore.Components.Emoji, new()
{
    private Emoji _emoji = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width ?? $"{_emoji.Width}px")
        .AddStyle("cursor", "pointer", OnClick.HasDelegate)
        .AddStyle("display", "inline-block", !ContainsSVG())
        .Build();

    /// <summary>
    /// Gets or sets the slot where the emoji is displayed in
    /// </summary>
    [Parameter]
    public string? Slot { get; set; } = null;

    /// <summary>
    /// Gets or sets the title for the emoji
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = null;

    /// <summary>
    /// Gets or sets the emoji width.
    /// If not set, the emoji size will be used.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the Emoji object to render.
    /// </summary>
    [Parameter]
    public Emoji Value
    {
        get => _emoji;
        set => _emoji = value;
    }

    /// <summary>
    /// Allows for capturing a mouse click on an emoji
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary />
    protected virtual Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            return OnClick.InvokeAsync(e);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (_emoji == null)
        {
            _emoji = new Emoji();
        }
    }

    /// <summary>
    /// Returns true if the emoji contains a SVG content.
    /// </summary>
    /// <returns></returns>
    private bool ContainsSVG()
    {
        return !string.IsNullOrEmpty(_emoji.Content) &&
               (_emoji.Content.StartsWith("<path ") ||
                _emoji.Content.StartsWith("<rect ") ||
                _emoji.Content.StartsWith("<g ") ||
                _emoji.Content.StartsWith("<circle ") ||
                _emoji.Content.StartsWith("<mark "));

    }
}
