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
## Usage
When using the provided implementation, you need to add add the following in the `Program.cs` file:

```
builder.Services.AddDataGridEntityFrameworkAdapter();
```

## Changing the adapter's behavior
Starting with v4.11.4, the `EntityFrameworkAsyncQueryExecutor` exposes a way to ignore exceptions which may occur during query execution.
This can be useful when you want to handle exceptions in a custom way, for example, by logging them. To ignore exceptions, you can
supply a `Func<Exception, bool>` to the `IgnoreException` property of the `EntityFrameworkAsyncQueryExecutor` instance. The function
should return `true` if the exception should be ignored and `false` otherwise. An example:
```csharp
builder.Services.AddFluentUIComponents()
    .AddDataGridEntityFrameworkAdapter(ex => ex is SqlException sqlEx
        && sqlEx.Errors.OfType<SqlError>().Any(e => (e.Class == 11 && e.Number == 0) || (e.Class == 16 && e.Number == 3204)));
```

For more information see also https://github.com/microsoft/fluentui-blazor/issues/3269.


## Support
The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it’s **not** officially
supported and isn’t committed to ship updates as part of any official .NET updates. It is built and maintained by Microsoft employees (**and** other contributors)
and offers support, like most other open source projects, on a best effort base through the GitHub repository **only**.
