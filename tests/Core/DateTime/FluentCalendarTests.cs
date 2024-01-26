using System.Globalization;
using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DateTime;

public partial class FluentCalendarTests : TestContext
{
    [Fact]
    public void FluentCalendar_Default()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
            parameters.Add(p => p.DisabledDateFunc, (date) => date.Day == 14);
        });

        // Assert
        calendar.Verify();
    }

    [Fact]
    public void FluentCalendar_Title()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
        });
        var title = calendar.Find(".title");

        // Assert
        title.MarkupMatches(@$"<div class=""title"" part=""title"" aria-label=""February 2022"">
                                 <div part=""label"" class=""label"">February 2022</div>
                                 <div part=""move"" class=""change-period"">
                                   <div class=""previous"" title=""January"">
                                     {FluentCalendar.ArrowUp}
                                   </div>
                                   <div class=""next"" title=""March"">
                                     {FluentCalendar.ArrowDown}
                                   </div>
                                 </div>
                               </div>");
    }

    [Fact]
    public void FluentCalendar_WeekDaysTitle()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
        });
        var title = calendar.Find(".week-days");

        // Assert
        title.MarkupMatches(@"<div class=""week-days"" part=""week-days"">
                              <div class=""week-day"" part=""week-day"" title=""Sunday"" abbr=""Sunday"">S</div>
                              <div class=""week-day"" part=""week-day"" title=""Monday"" abbr=""Monday"">M</div>
                              <div class=""week-day"" part=""week-day"" title=""Tuesday"" abbr=""Tuesday"">T</div>
                              <div class=""week-day"" part=""week-day"" title=""Wednesday"" abbr=""Wednesday"">W</div>
                              <div class=""week-day"" part=""week-day"" title=""Thursday"" abbr=""Thursday"">T</div>
                              <div class=""week-day"" part=""week-day"" title=""Friday"" abbr=""Friday"">F</div>
                              <div class=""week-day"" part=""week-day"" title=""Saturday"" abbr=""Saturday"">S</div>
                          </div>");
    }

    [Fact]
    public void FluentCalendar_WeekDays_French()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("fr-FR"));
        });
        var weekDays = calendar.Find(".week-days");

        // Assert
        weekDays.MarkupMatches(@"<div class=""week-days"" part=""week-days"">
                              <div class=""week-day"" part=""week-day"" title=""Lundi"" abbr=""Lundi"">L</div>
                              <div class=""week-day"" part=""week-day"" title=""Mardi"" abbr=""Mardi"">M</div>
                              <div class=""week-day"" part=""week-day"" title=""Mercredi"" abbr=""Mercredi"">M</div>
                              <div class=""week-day"" part=""week-day"" title=""Jeudi"" abbr=""Jeudi"">J</div>
                              <div class=""week-day"" part=""week-day"" title=""Vendredi"" abbr=""Vendredi"">V</div>
                              <div class=""week-day"" part=""week-day"" title=""Samedi"" abbr=""Samedi"">S</div>
                              <div class=""week-day"" part=""week-day"" title=""Dimanche"" abbr=""Dimanche"">D</div>
                          </div>");
    }

    [Fact]
    public void FluentCalendar_OneDayFormat()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
        });
        var day = calendar.Find("[value='2022-02-20']");

        // Assert
        day.MarkupMatches(@"<div part=""day""
                                 class=""day""
                                 aria-label=""February 20""
                                 value=""2022-02-20"">20</div>");
    }

    [Fact]
    public void FluentCalendar_DayValues()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
        });
        var allDays = calendar.FindAll(".date");

        // Assert
        var firstDate = System.DateTime.Parse("2022-01-30");

        for (var i = 0; i < allDays.Count; i++)
        {
            var expectedDay = firstDate.AddDays(i).Day;
            var actualDay = Convert.ToInt32(allDays[i].InnerHtml);

            Assert.Equal(expectedDay, actualDay);
        }
    }

    [Fact]
    public void FluentCalendar_SetPickerMonth()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.Value, new System.DateTime(2022, 02, 15));
            parameters.Add(p => p.PickerMonth, new System.DateTime(2022, 06, 15));
        });
        var monthName = calendar.Find(".label").InnerHtml;
        var juneFirst = System.DateTime.Parse("2022-06-01");

        // Assert
        Assert.Equal("June 2022", monthName);
        Assert.Equal(juneFirst, calendar.Instance.PickerMonth);
    }

    [Fact]
    public void FluentCalendar_ClickAndSelectDate()
    {
        const string DAY = "2022-06-15";

        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.PickerMonth, System.DateTime.Parse(DAY));
        });

        // Click to select 2022-06-24
        var day = calendar.Find($"div[value='{DAY}']");
        day?.Click();

        // Assert
        var selectedDay = calendar.Find($"div[value='{DAY}']");
        selectedDay.HasAttribute("selected");
        Assert.True(selectedDay.HasAttribute("selected"));
    }

    [Fact]
    public void FluentCalendar_ClickPreviousMonth()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.PickerMonth, System.DateTime.Parse("2022-06-15"));
        });

        // Click on Previous Month button
        var buttonMove = calendar.Find(".previous");
        buttonMove.Click();

        // Assert
        var monthName = calendar.Find(".label").InnerHtml;
        Assert.Equal("May 2022", monthName);
    }

    [Fact]
    public void FluentCalendar_ClickNextMonth()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.Culture, CultureInfo.GetCultureInfo("en-US"));
            parameters.Add(p => p.PickerMonth, System.DateTime.Parse("2022-06-15"));
        });

        // Click on Previous Month button
        var buttonMove = calendar.Find(".next");
        buttonMove.Click();

        // Assert
        var monthName = calendar.Find(".label").InnerHtml;
        Assert.Equal("July 2022", monthName);
    }

    [Fact]
    public void FluentCalendar_GetDayOfMonthTwoDigit()
    {
        const string DAY = "2022-06-01";

        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.PickerMonth, System.DateTime.Parse(DAY));
            parameters.Add(p => p.DayFormat, DayFormat.TwoDigit);
        });

        // Click to select 2022-06-24
        var day = calendar.Find($"div[value='{DAY}']");
        day?.Click();

        // Assert
        var selectedDay = calendar.Find($"div[value='{DAY}']");

        Assert.Equal("01", selectedDay.TextContent);
    }

    [Fact]
    public void FluentCalendar_GetDayOfMonthNumeric()
    {
        const string DAY = "2022-06-01";

        // Arrange
        using var ctx = new TestContext();

        // Act
        var calendar = ctx.RenderComponent<FluentCalendar>(parameters =>
        {
            parameters.Add(p => p.PickerMonth, System.DateTime.Parse(DAY));
            parameters.Add(p => p.DayFormat, DayFormat.Numeric);
        });

        // Click to select 2022-06-01
        var day = calendar.Find($"div[value='{DAY}']");
        day?.Click();

        // Assert
        var selectedDay = calendar.Find($"div[value='{DAY}']");

        Assert.Equal("1", selectedDay.TextContent);
    }
}
