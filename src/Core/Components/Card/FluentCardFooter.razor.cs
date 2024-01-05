using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentCardFooter : IDisposable
{
    protected string? ClassValue => new CssBuilder("fluent-card-footer")
        .AddClass(Class)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered on the far end of the footer, used for action buttons.
    /// </summary>
    [Parameter]
    public RenderFragment? Action { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentCard component
    /// </summary>
    [CascadingParameter]
    private FluentCard Card { get; set; } = default!;

    /// <summary />
    protected override void OnInitialized()
    {
        if (Card is null)
        {
            throw new ArgumentNullException(nameof(Card), $"{nameof(FluentCardFooter)} must be used inside {nameof(FluentCard)}");
        }

        Card.RegisterFooter(this);
    }

    /// <inheritdoc />
    public void Dispose() => Card.UnregisterFooter(this);
}