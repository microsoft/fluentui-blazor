// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DateTime;

public class FluentDatePickerTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    [Inject]
    public GlobalState GlobalState { get; set; } = new GlobalState();

    [Fact]
    public void FluentDatePicker_Closed()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
        });
        var id = picker.Instance.Id;

        // Assert
        picker.MarkupMatches(@$"<fluent-text-field class=""fluent-datepicker""
                                                   placeholder=""M/d/yyyy""
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
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);
        ctx.Services.AddSingleton(GlobalState);

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
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);
        ctx.Services.AddSingleton(GlobalState);
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
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);

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
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);

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

    [Fact]
    public void FluentDatePicker_MonthsViewGetCorrectPlaceholder()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.View, CalendarViews.Months);
        });
        var textfield = picker.Find("fluent-text-field");

        // Assert
        Assert.Equal("MMMM yyyy", textfield.Attributes["placeholder"]!.Value);
    }

    [Fact]
    public void FluentDatePicker_YearsViewGetCorrectPlaceholder()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.View, CalendarViews.Years);
        });
        var textfield = picker.Find("fluent-text-field");

        // Assert
        Assert.Equal("yyyy", textfield.Attributes["placeholder"]!.Value);
    }

    [Fact]
    public void FluentCalendar_DisabledDate()
    {

        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);
        ctx.Services.AddSingleton(GlobalState);

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
            parameters.Add(p => p.DisabledDateFunc, (date) => date.Day == 14);
        });

        var textfield = picker.Find("fluent-text-field");

        // Click
        textfield.Click();

        // Assert
        var calendar = picker.FindComponent<FluentCalendar>();

        // Assert
        calendar.Verify();
    }

    [Fact]
    public void FluentDatePicker_ChangeValueWhenDefaultIsNull()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.Value, null);
        });

        var textField = picker.Find("fluent-text-field");

        // Set a new date value
        textField.Change("2022-03-12");

        // Assert
        Assert.Equal(System.DateTime.Parse("2022-03-12"), picker.Instance.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("2024-06-05")]
    public void FluentDatePicker_DoubleClickToSetDateInTextField(string plainDateTime)
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);
        System.DateTime? dt = string.IsNullOrEmpty(plainDateTime) ? null : System.DateTime.Parse(plainDateTime);

        // Act
        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.DoubleClickToDate, dt);
        });

        var textField = picker.Find("fluent-text-field");

        // Double-Click
        textField.DoubleClick();

        // Assert
        Assert.False(picker.Instance.Opened);

        if (dt.HasValue)
        {
            Assert.Equal(dt.Value, picker.Instance.Value);
        }
        else
        {
            Assert.Null(picker.Instance.Value);
        }
    }

    [Fact]
    public void FluentDatePicker_OnDoubleClickEventTriggers()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddSingleton(LibraryConfiguration);
        var expected = "EventWorks";

        // Act
        var actual = string.Empty;

        var picker = ctx.RenderComponent<FluentDatePicker>(parameters =>
        {
            parameters.Add(p => p.OnDoubleClick, e => actual = expected);
        });

        var textField = picker.Find("fluent-text-field");

        // Double-Click
        textField.DoubleClick();

        // Assert
        Assert.Equal(expected, actual);
    }
}
