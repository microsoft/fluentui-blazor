# FluentDataGrid — Advanced Patterns

## Basic Usage

```razor
<FluentDataGrid Items="@people">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
    <PropertyColumn Property="@(p => p.Email)" />
    <PropertyColumn Property="@(p => p.BirthDate)" Format="yyyy-MM-dd" />
</FluentDataGrid>

@code {
    private IQueryable<Person> people = GetPeople().AsQueryable();
}
```

## Pagination

```razor
<FluentDataGrid Items="@people" Pagination="@pagination">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
    <PropertyColumn Property="@(p => p.Email)" />
</FluentDataGrid>

<FluentPaginator State="@pagination" />

@code {
    private IQueryable<Person> people = GetPeople().AsQueryable();
    private PaginationState pagination = new() { ItemsPerPage = 10 };
}
```

## Virtualization

For large datasets without pagination, use virtualization to render only
visible rows:

```razor
<FluentDataGrid Items="@people"
                Virtualize="true"
                ItemSize="46"
                OverscanCount="5"
                style="height: 500px; overflow-y: auto;">
    <PropertyColumn Property="@(p => p.Name)" />
</FluentDataGrid>
```

- `ItemSize` — expected row height in pixels (default: 32)
- `OverscanCount` — extra rows rendered above/below viewport (default: 3)

## Remote Data — ItemsProvider

For server-side data loading (API calls, EF Core via adapter):

```razor
<FluentDataGrid ItemsProvider="@dataProvider" Virtualize="true" ItemSize="46">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
    <PropertyColumn Property="@(p => p.City)" Sortable="true" />
</FluentDataGrid>

@code {
    private GridItemsProvider<Person> dataProvider = default!;

    protected override void OnInitialized()
    {
        dataProvider = async request =>
        {
            // request.StartIndex, request.Count, request.SortByColumn,
            // request.SortByAscending, request.CancellationToken
            var result = await FetchFromApi(request);
            return GridItemsProviderResult.From(result.Items, result.TotalCount);
        };
    }
}
```

## Entity Framework Adapter

Install: `Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter`

```csharp
// Program.cs
builder.Services.AddDataGridEntityFrameworkAdapter();
```

Then pass an `IQueryable<T>` from your `DbContext` directly to `Items`:

```razor
<FluentDataGrid Items="@db.People.AsNoTracking()">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
</FluentDataGrid>
```

The adapter translates sort/filter/page operations into efficient SQL queries.

## OData Adapter

Install: `Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapter`

## TemplateColumn

For custom cell rendering:

```razor
<FluentDataGrid Items="@people">
    <PropertyColumn Property="@(p => p.Name)" Sortable="true" />

    <TemplateColumn Title="Status">
        <FluentBadge Color="@(context.IsActive ? BadgeColor.Success : BadgeColor.Danger)">
            @(context.IsActive ? "Active" : "Inactive")
        </FluentBadge>
    </TemplateColumn>

    <TemplateColumn Title="Actions" Align="Align.End">
        <FluentButton Size="ButtonSize.Small"
                      IconStart="@(new Icons.Regular.Size16.Edit())"
                      OnClick="@(() => Edit(context))">
            Edit
        </FluentButton>
        <FluentButton Size="ButtonSize.Small"
                      IconStart="@(new Icons.Regular.Size16.Delete())"
                      Appearance="ButtonAppearance.Outline"
                      OnClick="@(() => Delete(context))">
            Delete
        </FluentButton>
    </TemplateColumn>
</FluentDataGrid>
```

## Column Options (Filter UI)

```razor
<PropertyColumn Property="@(p => p.City)" Sortable="true" Title="City">
    <ColumnOptions>
        <FluentTextField @bind-Value="cityFilter"
                         @bind-Value:after="@(() => StateHasChanged())"
                         Placeholder="Filter by city..." />
    </ColumnOptions>
</PropertyColumn>
```

The column options UI is accessible via a header button.

## Resizable Columns

```razor
<FluentDataGrid Items="@people"
                ResizableColumns="true"
                ResizeType="DataGridResizeType.Discrete">
    <PropertyColumn Property="@(p => p.Name)" />
    <PropertyColumn Property="@(p => p.Email)" />
</FluentDataGrid>
```

- `ResizableColumns="true"` — enables drag handles
- `ResizeType` — `Discrete` (10px steps) or `Exact` (pixel-precise) for keyboard resize
- `ResizeColumnOnAllRows` — when `true` (default), resize handles span full grid height

## Empty, Loading, and Error Content

```razor
<FluentDataGrid Items="@people" Loading="@isLoading">
    <PropertyColumn Property="@(p => p.Name)" />

    <EmptyContent>
        <p>No records found.</p>
    </EmptyContent>

    <LoadingContent>
        <FluentProgressBar />
    </LoadingContent>
</FluentDataGrid>
```

## Refresh Data Programmatically

```csharp
@code {
    private FluentDataGrid<Person> grid = default!;

    private async Task RefreshGrid()
    {
        await grid.RefreshDataAsync();
    }
}
```

```razor
<FluentDataGrid @ref="grid" Items="@people">
    ...
</FluentDataGrid>
```

## Manual Grid (No Auto-Columns)

For full control over grid template columns:

```razor
<FluentDataGrid Items="@people" GridTemplateColumns="200px 1fr 100px">
    ...
</FluentDataGrid>
```
