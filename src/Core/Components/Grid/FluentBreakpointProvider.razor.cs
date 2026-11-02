// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Publishes the current viewport breakpoint to components using a named cascading parameter.
/// This component does not render any HTML.
/// </summary>
/// <remarks>
/// Enable <see cref="LibraryConfiguration.UseCascadingViewportBreakpoint"/> during service registration.
/// <see cref="FluentProviders"/> includes this provider automatically when the option is enabled.
/// </remarks>
public partial class FluentBreakpointProvider : FluentComponentBase
{
    /// <summary>
    /// The name used to receive the current viewport breakpoint as a nullable <see cref="GridItemSize"/>.
    /// </summary>
    public const string CascadingParameterName = "FluentUI.ViewportBreakpoint";

    private bool _isDisposed;
    private DotNetObjectReference<FluentBreakpointProvider>? _dotNetHelper;
    private ViewportBreakpointSource? _breakpointSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentBreakpointProvider"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public FluentBreakpointProvider(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || _isDisposed || LibraryConfiguration?.UseCascadingViewportBreakpoint != true)
        {
            return;
        }

        _breakpointSource = GetCachedServiceOrNull<ViewportBreakpointSource>()
                         ?? throw new InvalidOperationException("Enable UseCascadingViewportBreakpoint when calling AddFluentUIComponents.");

        _dotNetHelper = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Utilities.BreakpointListener.initialize", Id, _dotNetHelper);
    }

    /// <summary>
    /// Receives viewport breakpoint changes from JavaScript.
    /// </summary>
    /// <param name="size">The breakpoint name.</param>
    /// <returns>A task representing the cascading value notification.</returns>
    [JSInvokable]
    public Task OnBreakpointChangedAsync(string size)
    {
        if (_isDisposed || !Enum.TryParse<GridItemSize>(size, ignoreCase: true, out var breakpoint))
        {
            return Task.CompletedTask;
        }

        var source = _breakpointSource
            ?? throw new InvalidOperationException("The viewport breakpoint provider has not been initialized.");

        return source.SetBreakpointAsync(breakpoint);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        try
        {
            if (_dotNetHelper is not null)
            {
                await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Utilities.BreakpointListener.dispose", Id);
            }
        }
        finally
        {
            _dotNetHelper?.Dispose();
            _dotNetHelper = null;
            await base.DisposeAsync();
        }
    }
}
