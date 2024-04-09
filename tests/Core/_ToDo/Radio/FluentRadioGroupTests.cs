namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Radio;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class FluentRadioGroupTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    public FluentRadioGroupTests()
    {

        TestContext.Services.AddSingleton(LibraryConfiguration);
    }
    [Fact]
    public void FluentRadioGroup_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        Orientation? orientation = default!;

        var cut = TestContext.RenderComponent<FluentRadioGroup<bool>>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

