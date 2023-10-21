using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Toast;
public class FluentToastTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentToast_Default()
    {
        //Arrange
        ToastInstance instance = default!;
        
        var cut = TestContext.RenderComponent<FluentToast>(parameters => parameters
            .Add(p => p.Instance, instance)
        );
        //Act

        //Assert
        cut.Verify();
    }
}






