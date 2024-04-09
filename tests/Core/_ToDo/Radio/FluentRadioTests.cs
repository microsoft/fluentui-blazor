namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Radio;
using Bunit;
using Xunit;

public class FluentRadioTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentRadio_Default()
    {
        //Arrange
        var labelTemplate = "<b>render me</b>";
        var childContent = "<b>render me</b>";
        bool readOnly = default!;
        string label = default!;
        string ariaLabel = default!;
        bool disabled = default!;
        string name = default!;
        bool required = default!;
        bool? Checked = default!;
        var cut = TestContext.RenderComponent<FluentRadio<bool>>(parameters => parameters
            .Add(p => p.ReadOnly, readOnly)
                .Add(p => p.Label, label)
                .Add(p => p.LabelTemplate, labelTemplate)
                .Add(p => p.AriaLabel, ariaLabel)
                .Add(p => p.Disabled, disabled)
                .Add(p => p.Name, name)
                .Add(p => p.Required, required)
                .Add(p => p.Checked, Checked)
                .AddChildContent(childContent)
            );
        //Act

        //Assert
        cut.Verify();
    }
}

