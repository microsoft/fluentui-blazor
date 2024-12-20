## About 
Use this package if the data you want to display in the `FluentDataGrid` (component in the `Microsoft.FluentUI.AspNetCore.Components library`) comes from [Microsoft.OData.Client](https://www.nuget.org/packages/Microsoft.OData.Client).

Microsoft OData Client library's DataServiceContext gives you a DataServiceQuery property for each resource in your OData service endpoint. Simply supply this as the grid's `Items` parameter:
```
@inject DataServiceContext MyServiceContext

<FluentDataGrid Items="@MyServiceContext.People">
    ...
</FluentDataGrid>
```

You may also use any DataServiceQuery-supported LINQ operator to filter the data before passing it:
```
@inject DataServiceContext MyServiceContext

<FluentDataGrid Items="@MyServiceContext.Documents.Where(d => d.CategoryId == currentCategoryId)">
    ...
</FluentDataGrid>
```

The `FluentDataGrid` recognizes DataServiceQuery-supplied `IQueryable` instances and knows how to resolve queries asynchronously for efficiency.

## Installation
Install the package by running the command:
```
dotnet add package Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapter
```

## Usage
In your Program.cs file you need to add the following:
```
builder.Services.AddDataGridODataAdapter();
```

## Support
The Microsoft Fluent UI Blazor library is an open source project and is **not** an official part of ASP.NET Core, which means it’s **not** officially
supported and isn’t committed to ship updates as part of any official .NET updates. It is built and maintained by Microsoft employees (**and** other contributors)
and offers support, like most other open source projects, on a best effort base through the GitHub repository **only**.
