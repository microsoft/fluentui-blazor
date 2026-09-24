---
title: Virtualize
route: /DataGrid/Virtualize
---
# Virtualize

It can be expensive both to fetch and to render large numbers of items. If the amount of data you're
displaying might be large, you should use either paging or virtualization.

Virtualization provides the appearance of continuous scrolling through an arbitrarily-large data set,
while only needing to fetch and render the rows that are currently in the scroll viewport. This can provide
excellent performance even when the data set is vast. FluentDataGrid's virtualization feature is built on Blazor's
built-in [Virtualize component](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/virtualization?view=aspnetcore-6.0),
so it shares the same capabilities, requirements, and limitations.

Enabling virtualization is just a matter of passing `Virtualize="true"`. For it to work
properly and reliably, every row rendered must have the same known height. **This is handled by the `FluentDataGrid` code**

{{ DataGridVirtualize }}

## Positioning and anchoring in .NET 11

When targeting .NET 11, a virtualized grid can open at a specific zero-based row index, scroll to a row
programmatically, and keep the viewport anchored as provider data changes. Out-of-range indexes are clamped by
the underlying `Virtualize` component.

An `ItemComparer` should compare stable item identity rather than object references when an items provider
returns new instances across requests.

```razor
@using Microsoft.AspNetCore.Components.Web.Virtualization

<FluentButton OnClick="ScrollToTopAsync">Back to top</FluentButton>

<div style="height: 400px; overflow-y: auto;">
	<FluentDataGrid @ref="_grid"
					TGridItem="Product"
					ItemsProvider="LoadProductsAsync"
					Virtualize="true"
					InitialItemIndex="500"
					AnchorMode="VirtualizeAnchorMode.End"
					ItemComparer="ProductIdComparer.Instance"
					ItemSize="32"
					DisplayMode="DataGridDisplayMode.Table">
		<PropertyColumn Property="@(product => product.Name)" />
	</FluentDataGrid>
</div>

@code {
	private FluentDataGrid<Product>? _grid;

	private Task ScrollToTopAsync()
		=> _grid!.ScrollToItemAsync(0);

	private sealed class ProductIdComparer : IEqualityComparer<Product>
	{
		public static ProductIdComparer Instance { get; } = new();

		public bool Equals(Product? first, Product? second)
			=> first?.Id == second?.Id;

		public int GetHashCode(Product product)
			=> product.Id.GetHashCode();
	}
}
```

`InitialItemIndex` is applied only on the first interactive render. Use `ScrollToItemAsync` after the grid has
rendered for later navigation. Calling it before rendering or while `Virtualize` is disabled throws an
`InvalidOperationException`. Cancellation and repeated-call behavior are delegated to the underlying virtualizer;
when calls overlap, the last call wins.
