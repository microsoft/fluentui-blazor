## About 
Use this package if the data you want to display in the `FluentDataGrid` (component in the `Microsoft.FluentUI.AspNetCore.Components library`) comes from EF Core:

- ... with [Blazor Server and any EF Core-supported database](https://docs.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core)
- ... with [Blazor WebAssembly and EF Core's Sqlite support](https://www.youtube.com/watch?v=2UPiKgHv8YE)

EF Core's DataContext gives you a DbSet property for each table in your database. Simply supply this as the grid's `Items` parameter:
```
@inject ApplicationDbContext MyDbContext

<FluentDataGrid Items="@MyDbContext.People">
    ...
</FluentDataGrid>
```
You may also use any EF-supported LINQ operator to filter the data before passing it:
```
@inject ApplicationDbContext MyDbContext

<FluentDataGrid Items="@MyDbContext.Documents.Where(d => d.CategoryId == currentCategoryId)">
    ...
</FluentDataGrid>
```

The `FluentDataGrid` recognizes EF-supplied `IQueryable` instances and knows how to resolve queries asynchronously for efficiency.

## Installation
Install the package by running the command:
```
dotnet add package Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter
```

## Creating a derived AsyncQueryExecutor implementation
Starting with v4.11.4, the `EntityFrameworkAsyncQueryExecutor` class is made public
so that you can derive from it and override the `ExecuteAsync` method to provide
custom query execution logic. This is useful if you want to add custom query processing
logic, such as logging, caching, or query translation or if you want specific [error handling](https://github.com/microsoft/fluentui-blazor/issues/3269).

## Usage
When using the provided implementation, you need to add add the following in the `Program.cs` file:

```
builder.Services.AddDataGridEntityFrameworkAdapter();
```

When using a custom implementation, you need to add this custom implementation to the DI container in the `Program.cs` file yourself.
You do **not** call `AddDataGridEntityFrameworkAdapter` in this case.
```
builder.Services.AddScoped<IAsyncQueryExecutor, MyCustomAsyncQueryExecutor>();
```

## Support
The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it’s **not** officially
supported and isn’t committed to ship updates as part of any official .NET updates. It is built and maintained by Microsoft employees (**and** other contributors)
and offers support, like most other open source projects, on a best effort base through the GitHub repository **only**.
