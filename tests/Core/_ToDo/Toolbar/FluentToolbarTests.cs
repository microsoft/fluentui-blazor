using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Toolbar;
public class FluentToolbarTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    public FluentToolbarTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.Services.AddSingleton(LibraryConfiguration);
    }

    [Fact]
    public void FluentToolbar_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        Orientation? orientation = default!;
        var cut = TestContext.RenderComponent<FluentToolbar>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

