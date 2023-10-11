using Bunit;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.DataGrid;
public class FluentDataGridTests : TestBase
{
    public FluentDataGridTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.Fast.Components.FluentUI/Components/DataGrid/FluentDataGrid.razor.js");
        TestContext.JSInterop.Setup<IJSObjectReference>("init", _ => true);
    }

    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentDataGrid_Default()
    {
        //Arrange
        //Services.AddSingleton<IServiceProvider,/*Add implementation for IServiceProvider*/>();
        string childContent = "<b>render me</b>";
        string emptyContent = "<b>render me</b>";
       
        bool virtualize = default!;
        float itemSize = default!;
        bool resizableColumns = default!;
        Microsoft.Fast.Components.FluentUI.PaginationState pagination = default!;
        bool noTabbing = default!;
        Microsoft.Fast.Components.FluentUI.GenerateHeaderOption? generateHeader = default!;
        string gridTemplateColumns = default!;


        var cut = TestContext.RenderComponent<FluentDataGrid<Customer>>(parameters => parameters
            .Add(p => p.Items, GetCustomers().AsQueryable())

            .AddChildContent(childContent)
            .Add(p => p.Virtualize, virtualize)
            .Add(p => p.ItemSize, itemSize)
            .Add(p => p.ResizableColumns, resizableColumns)
            .Add(p => p.Pagination, pagination)
            .Add(p => p.NoTabbing, noTabbing)
            .Add(p => p.GenerateHeader, generateHeader)
            .Add(p => p.GridTemplateColumns, gridTemplateColumns)

            .Add(p => p.EmptyContent, emptyContent)
        );
        //Act

        //Assert
        cut.Verify();
    }


    // Sample data...
    private IEnumerable<Customer> GetCustomers()
    {
        yield return new Customer(1, "Denis Voituron");
        yield return new Customer(2, "Vincent Baaij");
        yield return new Customer(3, "Bill Gates");
    }

    private record Customer(int Id, string Name);
}