using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Label;
public class FluentInputLabelTests : TestBase
{
    [Fact]
    public void FluentInputLabel_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        string forId = default!;
        string label = default!;
        string ariaLabel = default!;
        IReadOnlyDictionary<string, object> additionalAttributes = default!;
        var cut = TestContext.RenderComponent<FluentInputLabel<string>>(parameters => parameters
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






