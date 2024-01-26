using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Pagination;
public class FluentPaginatorTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentPaginator_Default()
    {
        //Arrange
        var summaryTemplate = "<b>render me</b>";
        Action<int> currentPageIndexChanged = _ => { };
        PaginationState state = default!;
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

