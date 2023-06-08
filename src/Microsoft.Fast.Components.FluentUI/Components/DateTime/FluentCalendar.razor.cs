using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Fluent Calendar based on
/// https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/calendar/.
/// </summary>
public partial class FluentCalendar : FluentCalendarBase
{
    private DateTime? _pickerMonth = null;
    private CalendarExtended? _calendarExtended = null;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).AddClass("fluent-calendar").Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// The current month of the date picker (two-way bindable).
    /// This changes when the user browses through the calendar.
    /// The month is represented as a DateTime which is always the first day of that month.
    /// You can also set this to determine which month is displayed first.
    /// If not set, the current month is displayed.
    /// </summary>
    [Parameter]
    public virtual DateTime PickerMonth
    {
        get
        {
            return FirstDayOfMonth(_pickerMonth
                                  ?? (SelectedDate ?? DateTime.Today));
        }

        set
        {
            var month = FirstDayOfMonth(value);

            if (month == _pickerMonth)
            {
                return;
            }

            _pickerMonth = month;
            PickerMonthChanged.InvokeAsync(month);
        }
    }

    /// <summary>
    /// Fired when the display month changes.
    /// </summary>
    [Parameter]
    public virtual EventCallback<DateTime> PickerMonthChanged { get; set; }

    /// <summary>
    /// All days of this current month.
    /// </summary>
    private CalendarExtended CalendarExtended => _calendarExtended ?? new CalendarExtended(this.Culture, this.PickerMonth);

    /// <summary>
    /// Returns the class name to display a day (day, inactive, today).
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private DayProperties GetDayProperties(DateTime day)
    {
        var isDisabledDay = DisabledDateFunc?.Invoke(day) ?? false;
        var cssClasses = new List<string>
        {
            "day" // Default
        };

        if (isDisabledDay ||
            CalendarExtended.IsInCurrentMonth(day) == false)
        {
            cssClasses.Add("inactive");
        }

        if (day == DateTime.Today)
        {
            cssClasses.Add("today");
        }

        if (day == SelectedDate)
        {
            cssClasses.Add("selected");
        }

        return new DayProperties(CalendarExtended, day)
        {
            CssClasses = cssClasses.ToArray(),
            IsDisabled = isDisabledDay,
        };
    }

    private DateTime FirstDayOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }
}
