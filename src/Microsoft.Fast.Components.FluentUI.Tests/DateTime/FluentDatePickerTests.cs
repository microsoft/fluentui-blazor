using System.Globalization;
using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.DateTime;

public class FluentDatePickerTests : TestBase
{
    [Fact]
    public void FluentDatePicker_Closed()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
        });
        var id = picker.Instance.Id;

        // Assert
        picker.MarkupMatches(@$"<fluent-text-field class=""fluent-datepicker""
                                                   placeholder=""dd/MM/yyyy""
                                                   id=""{id}""
                                                   appearance=""outline"">
                                    {FluentDatePicker.CalendarIcon}
                                </fluent-text-field>");
    }

    [Fact]
    public void FluentDatePicker_ClickToOpenCalendar()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>();
        var textfield = picker.Find("fluent-text-field");

        // Click
        textfield.Click();

        // Assert
        var calendar = picker.FindComponent<FluentCalendar>();

        Assert.True(picker.Instance.Opened);
        Assert.Null(calendar.Instance.Value);

        Assert.Equal(System.DateTime.Today.Year, calendar.Instance.PickerMonth.Year);
        Assert.Equal(System.DateTime.Today.Month, calendar.Instance.PickerMonth.Month);
        Assert.Equal(1, calendar.Instance.PickerMonth.Day);
    }

    [Fact]
    public void FluentDatePicker_SetToday_CalendarSelectedDay()
    {
        // Arrange
        using var ctx = new TestContext();
        var today = System.DateTime.Today;

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Value, today);
        });
        var textfield = picker.Find("fluent-text-field");

        // Click
        textfield.Click();

        // Assert
        var calendar = picker.FindComponent<FluentCalendar>();
        Assert.Equal(today, calendar.Instance.Value);
    }

    [Fact]
    public void FluentDatePicker_WriteValidDateInTextField()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
        });

        var textField = picker.Find("fluent-text-field");

        // Set a new date value
        textField.Change("3/12/2022");

        // Assert
        Assert.Equal(System.DateTime.Parse("2022-03-12"), picker.Instance.Value);
    }

    [Fact]
    public void FluentDatePicker_WriteInvalidDateInTextField()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
        });
        var textfield = picker.Find("fluent-text-field");

        // Set a new date value
        textfield.Change("3/32/2022");

        // Assert
        Assert.Null(picker.Instance.Value);
    }
}
