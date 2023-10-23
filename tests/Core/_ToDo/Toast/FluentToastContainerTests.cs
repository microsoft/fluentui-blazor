using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Toast;
public class FluentToastContainerTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentToastContainer_Default()
    {
        //Arrange
        //Services.AddSingleton<IToastService,/*Add implementation for IToastService*/>();
        ToastPosition position = default!;
        int timeout = default!;
        int maxToastCount = default!;
        bool removeToastsOnNavigation = default!;
        bool showCloseButton = default!;
        var cut = TestContext.RenderComponent<FluentToastContainer>(parameters => parameters
            .Add(p => p.Position, position)
            .Add(p => p.Timeout, timeout)
            .Add(p => p.MaxToastCount, maxToastCount)
            .Add(p => p.RemoveToastsOnNavigation, removeToastsOnNavigation)
            .Add(p => p.ShowCloseButton, showCloseButton)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






