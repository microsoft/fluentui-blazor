using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.DateTime;

public class FluentTimePickerTests : TestBase
{
    [Fact]
    public void FluentTimePicker_Value_0123()
    {
        /*
        WHY ???

        FluentTimePicker requires a value for the 'ValueExpression' parameter.
        Normally this is provided automatically when using 'bind-Value'.

        // Arrange
        using var ctx = new Bunit.TestContext();

        // Act
        var picker = ctx.RenderComponent<FluentTimePicker>(parameters =>
        {
            parameters.Add(p => p.Value, new System.DateTime(2000, 02, 03, 01, 23, 00));   // 01:23
        });

        // Assert
        picker.Verify();

        */
    }
}
