using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Forms;
public class FluentValidationSummaryTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentValidationSummary_Default()
    {
        //Arrange
        //EditContext fluentEditContext = default!;
        var cut = TestContext.RenderComponent<FluentValidationSummary>();
        //Act

        //Assert
        cut.Verify();
    }
}

