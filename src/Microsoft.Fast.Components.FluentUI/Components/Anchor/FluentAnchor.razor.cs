using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentAnchor : FluentComponentBase, IAsyncDisposable
{

    private IJSObjectReference _jsModule = default!;
    private string? _targetId = null;
    private bool _preventDefault = false;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;


    /// <summary>
    /// Prompts the user to save the linked URL. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Download { get; set; }

    /// <summary>
    /// The URL the hyperlink references. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// Use Target parameter to specify where.
    /// </summary>
    [Parameter, EditorRequired]
    public string? Href { get; set; }

    /// <summary>
    /// Hints at the language of the referenced resource. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Hreflang { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Ping { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Referrerpolicy { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Rel { get; set; }

    /// <summary>
    /// The target attribute specifies where to open the link, if Href is specified. 
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a">a element</see> for more information.
    /// </summary>
    [Parameter]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="Appearance"/>
    /// Defaults to <seealso cref="Appearance.Neutral"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = FluentUI.Appearance.Neutral;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnParametersSet()
    {
        // If the Href has been specified (as it should) and if starts with '#,'
        // we assume the rest of the value contains the id of the element the link points to.
        if (!string.IsNullOrEmpty(Href) && Href.StartsWith('#'))
        {
            // We don't want the default click action to occur, but rather take care of the click in our own method.
            _preventDefault = true;

            // Handle text fragment (https://example.org/#test:~:text=foo)
            // https://github.com/WICG/scroll-to-text-fragment/

            _targetId = Href.Substring(1);
            int index = _targetId.IndexOf(":~:", StringComparison.Ordinal);
            if (index > 0)
            {
                _targetId = _targetId.Substring(0, index);
            }
        }
        base.OnParametersSet();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/Microsoft.Fast.Components.FluentUI/Components/Anchor/FluentAnchor.razor.js");
        }
    }

    private async Task OnClickAsync()
    {
        if (!string.IsNullOrEmpty(_targetId) && _jsModule != null)
        {
            // If the target ID has been specified, we know this is an anchor link that we need to scroll to
            await _jsModule.InvokeVoidAsync("scrollIntoView", _targetId);
        }
    }

    /// <inheritdoc />
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not needed")]
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}