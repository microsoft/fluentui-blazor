using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentSystemEmoji is a component that renders an emoji from the Microsoft FluentUI emoji set.
/// </summary>
public partial class FluentSystemEmoji<Emoji> : FluentComponentBase
    where Emoji : FluentUI.Emoji, new()
{
    private Emoji _emoji = new();

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width ?? $"{_emoji.Width}px")
        .AddStyle("cursor", "pointer", OnClick.HasDelegate)
        .AddStyle("display", "inline-block", !ContainsSVG())
        .AddStyle(Style)
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
