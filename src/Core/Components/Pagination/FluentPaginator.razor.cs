// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component that provides a user interface for <see cref="PaginationState"/>.
/// </summary>
public partial class FluentPaginator : FluentComponentBase, IDisposable
{
    private readonly EventCallbackSubscriber<PaginationState> _totalItemCountChanged;
    private readonly EventCallbackSubscriber<PaginationState> _currentPageItemsChanged;

    /// <summary>
    /// Constructs an instance of <see cref="FluentPaginator" />.
    /// </summary>
    public FluentPaginator(LibraryConfiguration configuration) : base(configuration)
    {
        // The "total item count" handler doesn't need to do anything except cause this component to re-render
        _totalItemCountChanged = new(new EventCallback<PaginationState>(this, @delegate: null));
        _currentPageItemsChanged = new(new EventCallback<PaginationState>(this, @delegate: null));
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-paginator")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the callback that is invoked when the current page index changes.
    /// </summary>
    /// <remarks>The callback receives the new page index as an <see cref="int"/> parameter. Use this property
    /// to handle page index changes in a parent component or service.</remarks>
    [Parameter]
    public EventCallback<int> CurrentPageIndexChanged { get; set; }

    /// <summary>
    /// Disables the pagination buttons
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the associated <see cref="PaginationState"/>. This parameter is required.
    /// </summary>
    [Parameter, EditorRequired]
    public required PaginationState State { get; set; }

    /// <summary>
    /// Optionally supplies a template for rendering the page count summary.
    /// The following values can be included:
    /// {your State parameter name}.TotalItemCount (for the total number of items)
    /// </summary>
    [Parameter]
    public RenderFragment? SummaryTemplate { get; set; }

    /// <summary>
    /// Optionally supplies a template for rendering the pagination summary.
    /// The following values can be included:
    /// {your State parameter name}.CurrentPageIndex (zero-based, so +1 for the current page number)
    /// {your State parameter name}.LastPageIndex (zero-based, so +1 for the total number of pages)
    /// </summary>
    [Parameter]
    public RenderFragment? PaginationTextTemplate { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        _totalItemCountChanged.SubscribeOrMove(State.TotalItemCountChangedSubscribable);
        _currentPageItemsChanged.SubscribeOrMove(State.CurrentPageItemsChanged);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _totalItemCountChanged.Dispose();
        _currentPageItemsChanged.Dispose();
    }

    private bool CanGoBack => State.CurrentPageIndex > 0;

    private bool CanGoForwards => State.CurrentPageIndex < State.LastPageIndex;

    private Task GoFirstAsync() => GoToPageAsync(0);

    private Task GoPreviousAsync() => GoToPageAsync(State.CurrentPageIndex - 1);

    private Task GoNextAsync() => GoToPageAsync(State.CurrentPageIndex + 1);

    private Task GoLastAsync() => GoToPageAsync(State.LastPageIndex.GetValueOrDefault(0));

    private async Task GoToPageAsync(int pageIndex)
    {
        await State.SetCurrentPageIndexAsync(pageIndex);
        if (CurrentPageIndexChanged.HasDelegate)
        {
            await CurrentPageIndexChanged.InvokeAsync(State.CurrentPageIndex);
        }
    }
}
