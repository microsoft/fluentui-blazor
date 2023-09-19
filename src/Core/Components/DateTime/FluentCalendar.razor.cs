using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Fluent Calendar based on
/// https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/calendar/.
/// </summary>
public partial class FluentCalendar : FluentCalendarBase
{
    public static string ArrowUp = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M4.2 10.73a.75.75 0 001.1 1.04l5.95-6.25v14.73a.75.75 0 001.5 0V5.52l5.95 6.25a.75.75 0 001.1-1.04l-7.08-7.42a1 1 0 00-1.44 0L4.2 10.73z\"/></svg>";
    public static string ArrowDown = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M19.8 13.27a.75.75 0 00-1.1-1.04l-5.95 6.25V3.75a.75.75 0 10-1.5 0v14.73L5.3 12.23a.75.75 0 10-1.1 1.04l7.08 7.42a1 1 0 001.44 0l7.07-7.42z\"/></svg>";

    private DateTime? _pickerMonth = null;
    private CalendarExtended? _calendarExtended = null;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).AddClass("fluent-calendar").Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();

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
                                  ?? (Value ?? DateTime.Today));
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
    /// Defines the appearance of a Day cell.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentCalendarDay>? DaysTemplate { get; set; }

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => _calendarExtended ?? new CalendarExtended(this.Culture, this.PickerMonth);

    /// <summary>
    /// Returns the class name to display a day (day, inactive, today).
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private FluentCalendarDay GetDayProperties(DateTime day) => new(this, day);

    private DateTime FirstDayOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }
}
