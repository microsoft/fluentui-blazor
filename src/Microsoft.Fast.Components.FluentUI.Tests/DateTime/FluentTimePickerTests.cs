using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.DateTime;

public class FluentTimePickerTests : TestBase
{
    [Fact]
    public void FluentTimePicker_Value_0123()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        System.DateTime? value = new System.DateTime(2000, 02, 03, 01, 23, 00); // 01:23

        // Act
        var picker = ctx.RenderComponent<FluentTimePicker>(parameters =>
        {
            parameters.Add(p => p.Value, value);
            parameters.Add(p => p.ValueChanged, (System.DateTime? newValue) => value = newValue);
            parameters.Add(p => p.ValueExpression, () => value);
        });

        // Assert
        picker.Verify();
    }
}
