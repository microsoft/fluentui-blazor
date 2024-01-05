using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentCardHeader : IDisposable
{
    protected string? ClassValue => new CssBuilder("fluent-card-header")
        .AddClass(Class)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered as the main header title.
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered as a short description related to the title.
    /// </summary>
    [Parameter]
    public RenderFragment? Description { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered as an image or avatar related to the card.
    /// </summary>
    [Parameter]
    public RenderFragment? Image { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered on the far end of the header, used for action buttons.
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

    private string? _headerElementId;

    private string? HeaderElementId
    {
        get
        {
            if (Header is null)
            {
                return null;
            }

            if (_headerElementId is not null)
            {
                return _headerElementId;
            }

            _headerElementId = Id is not null
                ? $"{Id}-header"
                : Identifier.NewId();

            return _headerElementId;
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        if (Card is null)
        {
            throw new ArgumentNullException(nameof(Card), $"{nameof(FluentCardHeader)} must be used inside {nameof(FluentCard)}");
        }

        Card.RegisterHeader(this);
    }

    /// <inheritdoc />
    public void Dispose() => Card.UnregisterHeader(this);
}