using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Label;
public class FluentInputLabelTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    [Fact]
    public void FluentInputLabel_Default()
    {
        //Arrange
        TestContext.Services.AddSingleton(LibraryConfiguration);
        var childContent = "<b>render me</b>";
        string forId = default!;
        string label = default!;
        string ariaLabel = default!;
        IReadOnlyDictionary<string, object> additionalAttributes = default!;
        var cut = TestContext.RenderComponent<FluentInputLabel>(parameters => parameters
            .Add(p => p.ForId, forId)
            .Add(p => p.Label, label)
            .AddChildContent(childContent)
            .Add(p => p.AriaLabel, ariaLabel)
            .Add(p => p.AdditionalAttributes, additionalAttributes)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

