using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Pagination;
public class FluentPaginatorTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentPaginator_Default()
    {
        //Arrange
        string summaryTemplate = "<b>render me</b>";
        Action<System.Int32> currentPageIndexChanged = _ => { };
        Microsoft.Fast.Components.FluentUI.PaginationState state = default!;
        var cut = TestContext.RenderComponent<FluentPaginator>(parameters => parameters
            .Add(p => p.CurrentPageIndexChanged, currentPageIndexChanged)
            .Add(p => p.State, state)
            .Add(p => p.SummaryTemplate, summaryTemplate)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






