using Bunit;
using FluentAssertions;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Calendar
{
    public class CalendarRenderShould : TestBase
    {
        [Fact]
        public void RenderProperly_Default()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(12)]
        [InlineData(13)]
        public void RenderProperly_MonthAttribute(int month)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = null;
            Action action = () =>
            {
                cut = TestContext.RenderComponent<FluentCalendar>(
                    parameters => parameters
                        .Add(p => p.Month, month)
                        .AddChildContent(childContent));
            };

            // Assert
            if (month > 12 || month < 1)
            {
                action.Should().Throw<ArgumentException>();
            }
            else
            {
                action.Should().NotThrow();
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"month=\"{month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  "selected-dates=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
        }

        [Fact]
        public void RenderProperly_YearAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            int year = 2022;
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.Year, year)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Theory]
        [InlineData("en-GB")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RenderProperly_LocaleAttribute(string? locale)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.Locale, locale)
                    .AddChildContent(childContent));

            // Assert
            if (locale is not null)
            {
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"locale=\"{locale}\" " +
                                  $"month=\"{DateTime.Now.Month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  "selected-dates=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
            else
            {
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"month=\"{DateTime.Now.Month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  "selected-dates=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
        }

        [Theory]
        [InlineData(DayFormat.Numeric)]
        [InlineData(DayFormat.TwoDigit)]
        public void RenderProperly_DayFormatAttribute(DayFormat dayFormat)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.DayFormat, dayFormat)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              $"day-format=\"{dayFormat.ToAttributeValue()}\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Theory]
        [InlineData(WeekdayFormat.Long)]
        [InlineData(WeekdayFormat.Narrow)]
        [InlineData(WeekdayFormat.Short)]
        public void RenderProperly_WeekDayFormatAttribute(WeekdayFormat weekdayFormat)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.WeekdayFormat, weekdayFormat)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              $"weekday-format=\"{weekdayFormat.ToAttributeValue()}\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Theory]
        [InlineData(MonthFormat.Long)]
        [InlineData(MonthFormat.Narrow)]
        [InlineData(MonthFormat.Numeric)]
        [InlineData(MonthFormat.Short)]
        [InlineData(MonthFormat.TwoDigit)]
        public void RenderProperly_MonthFormatAttribute(MonthFormat monthFormat)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.MonthFormat, monthFormat)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              $"month-format=\"{monthFormat.ToAttributeValue()}\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Theory]
        [InlineData(YearFormat.Numeric)]
        [InlineData(YearFormat.TwoDigit)]
        public void RenderProperly_YearFormatAttribute(YearFormat yearFormat)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.YearFormat, yearFormat)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              $"year-format=\"{yearFormat.ToAttributeValue()}\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Fact]
        public void RenderProperly_MinWeeksAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            int minWeeks = 6;
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.MinWeeks, minWeeks)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              $"min-weeks=\"{minWeeks}\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        public static IEnumerable<object[]> RenderProperly_DisabledDatesAttribute_TestData = new List<object[]>
        {
            new object[] { new List<DateOnly>() },
            new object[] { new List<DateOnly> { new DateOnly(2022, 12, 13) } },
            new object[] { new List<DateOnly> { new DateOnly(2022, 12, 13), new DateOnly(2022, 12, 23) } }
        };

        [Theory]
        [MemberData(nameof(RenderProperly_DisabledDatesAttribute_TestData))]
        public void RenderProperly_DisabledDatesAttribute(IEnumerable<DateOnly>? disabledDates)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.DisabledDates, disabledDates)
                    .AddChildContent(childContent));

            // Assert
            if (disabledDates is not null)
            {
                string disabled = string.Join(",",
                    disabledDates.OrderBy(d => d.DayNumber).Select(s => s.ToString("M-d-yyyy")));
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"month=\"{DateTime.Now.Month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  "selected-dates=\"\" " +
                                  $"disabled-dates=\"{disabled}\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
            else
            {
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"month=\"{DateTime.Now.Month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  "selected-dates=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
        }

        public static IEnumerable<object[]> RenderProperly_SelectedDatesAttribute_TestData = new List<object[]>
        {
            new object[] { new List<DateOnly>() },
            new object[] { new List<DateOnly> { new DateOnly(2022, 12, 13) } },
            new object[] { new List<DateOnly> { new DateOnly(2022, 12, 13), new DateOnly(2022, 12, 23) } }
        };

        [Theory]
        [MemberData(nameof(RenderProperly_SelectedDatesAttribute_TestData))]
        public void RenderProperly_SelectedDatesAttribute(IEnumerable<DateOnly>? selectedDates)
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.SelectedDates, selectedDates)
                    .AddChildContent(childContent));

            // Assert
            if (selectedDates is not null)
            {
                string selected = string.Join(",",
                    selectedDates.OrderBy(d => d.DayNumber).Select(s => s.ToString("M-d-yyyy")));
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"month=\"{DateTime.Now.Month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  $"selected-dates=\"{selected}\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
            else
            {
                cut.MarkupMatches("<fluent-calendar " +
                                  "readonly=\"false\" " +
                                  $"month=\"{DateTime.Now.Month}\" " +
                                  $"year=\"{DateTime.Now.Year}\" " +
                                  "day-format=\"numeric\" " +
                                  "weekday-format=\"short\" " +
                                  "month-format=\"long\" " +
                                  "year-format=\"numeric\" " +
                                  "min-weeks=\"4\" " +
                                  "selected-dates=\"\">" +
                                  $"{childContent}" +
                                  "</fluent-calendar>");
            }
        }

        [Fact]
        public void RenderProperly_ReadonlyAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.Readonly, true)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              "readonly=\"true\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Fact]
        public void RenderProperly_ClassAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            string className = "class-name";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.Class, className)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              $"class=\"{className}\" " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Fact]
        public void RenderProperly_StyleAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            string style = "background-color:yellow;";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .Add(p => p.Style, style)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              $"style=\"{style}\" " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Fact]
        public void RenderProperly_AdditionalAttribute()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            string additionalAttributeName = "additional-attribute-name";
            string additionalAttributeValue = "additional-attribute-value";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .AddUnmatched(additionalAttributeName, additionalAttributeValue)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              $"{additionalAttributeName}=\"{additionalAttributeValue}\" " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }

        [Fact]
        public void RenderProperly_AdditionalAttributes()
        {
            // Arrange && Act
            string childContent = "fluent-calendar";
            string additionalAttribute1Name = "additional-attribute1-name";
            string additionalAttribute1Value = "additional-attribute1-value";
            string additionalAttribute2Name = "additional-attribute2-name";
            string additionalAttribute2Value = "additional-attribute2-value";
            IRenderedComponent<FluentCalendar> cut = TestContext.RenderComponent<FluentCalendar>(
                parameters => parameters
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent(childContent));

            // Assert
            cut.MarkupMatches("<fluent-calendar " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\" " +
                              "readonly=\"false\" " +
                              $"month=\"{DateTime.Now.Month}\" " +
                              $"year=\"{DateTime.Now.Year}\" " +
                              "day-format=\"numeric\" " +
                              "weekday-format=\"short\" " +
                              "month-format=\"long\" " +
                              "year-format=\"numeric\" " +
                              "min-weeks=\"4\" " +
                              "selected-dates=\"\">" +
                              $"{childContent}" +
                              "</fluent-calendar>");
        }
    }
}