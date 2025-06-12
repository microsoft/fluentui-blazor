namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A callback that provides data for a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <param name="request">Parameters describing the data being requested.</param>
/// <returns>A <see cref="T:ValueTask{GridItemsProviderResult{TResult}}" /> that gives the data to be displayed.</returns>
public delegate ValueTask<GridItemsProviderResult<TGridItem>> GridItemsProvider<TGridItem>(
    GridItemsProviderRequest<TGridItem> request);
