## About 
Use this package if the data you want to display in the `FluentDataGrid` (component in the `Microsoft,Fast.Components.FluentUI library`) comes from EF Core:

- ... with [Blazor Server and any EF Core-supported database](https://docs.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core)
- ... with [Blazor WebAssembly and EF Core's Sqlite support](https://www.youtube.com/watch?v=2UPiKgHv8YE)

EF Core's DataContext gives you a DbSet property for each table in your database. Simply supply this as the grid's RowsData parameter:
```
@inject ApplicationDbContext MyDbContext

<FluentDataGrid RowsData="@MyDbContext.People">
    ...
</FluentDataGrid>
```
You may also use any EF-supported LINQ operator to filter the data before passing it:
```
@inject ApplicationDbContext MyDbContext

<FluentDataGrid RowsData="@MyDbContext.Documents.Where(d => d.CategoryId == currentCategoryId)">
    ...
</FluentDataGrid>
```

The `FluentDataGrid` recognizes EF-supplied `IQueryable` instances and knows how to resolve queries asynchronously for efficiency.

## Installation
Install the package by running the command:
```
dotnet add package Microsoft.Fast.Components.FluentUI.DataGrid.EntityFrameworkAdapter
```

## Usage
In your Program.cs file you need to add the following:
```
builder.Services.AddDataGridEntityFrameworkAdapter();
```
