using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DateTime;

public class FluentTimePickerTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    [Fact]
    public void FluentTimePicker_Value_0123()
    {
        // Arrange
        using var ctx = new Bunit.TestContext();
        ctx.Services.AddSingleton(LibraryConfiguration);
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

    [Theory]
    [InlineData("01:23", "01:23:00")]
    [InlineData("15:17", "15:17:00")]
    [InlineData("25:22", null)]
    [InlineData("abc", null)]
    public void FluentTimePicker_TryParseValueFromString(string? value, string? time)
    {
        // Arrange
        var picker = new TestTimePicker();

        // Act
        var _ = picker.CallTryParseValueFromString(value, out var resultDate, out var _);

        // Assert
        if (resultDate != null)
        {
            Assert.Equal(System.DateTime.MinValue.Date, resultDate?.Date);
            Assert.Equal(time, resultDate?.ToString("HH:mm:ss"));
        }
        else
        {
            Assert.Null(resultDate);
        }
    }

    // Temporary class to expose protected method
    private class TestTimePicker : FluentTimePicker
    {
        public bool CallTryParseValueFromString(string? value, out System.DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            return base.TryParseValueFromString(value, out result, out validationErrorMessage);
        }
    }
}
