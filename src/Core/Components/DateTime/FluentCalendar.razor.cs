using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Fluent Calendar based on
/// https://github.com/microsoft/fluentui/blob/master/packages/web-components/src/calendar/.
/// </summary>
public partial class FluentCalendar : FluentCalendarBase
{
    public static string ArrowUp = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M4.2 10.73a.75.75 0 001.1 1.04l5.95-6.25v14.73a.75.75 0 001.5 0V5.52l5.95 6.25a.75.75 0 001.1-1.04l-7.08-7.42a1 1 0 00-1.44 0L4.2 10.73z\"/></svg>";
    public static string ArrowDown = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M19.8 13.27a.75.75 0 00-1.1-1.04l-5.95 6.25V3.75a.75.75 0 10-1.5 0v14.73L5.3 12.23a.75.75 0 10-1.1 1.04l7.08 7.42a1 1 0 001.44 0l7.07-7.42z\"/></svg>";

    private VerticalPosition _animationRunning = VerticalPosition.Unset;
    private DateTime? _pickerMonth = null;
    private CalendarExtended? _calendarExtended = null;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-calendar", () => View == CalendarViews.Days)
        .AddClass("fluent-month", () => View == CalendarViews.Months)
        .AddClass("fluent-year", () => View == CalendarViews.Years)
        .Build();

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
    /// Defines the appearance of the <see cref="FluentCalendar"/> component.
    /// </summary>
    [Parameter]
    public CalendarViews View { get; set; } = CalendarViews.Days;

    /// <summary>
    /// Gets ot sets if the calendar items are animated during a period change.
    /// By default, the animation is enabled for Months and Years views,
    /// but disabled for Days view.
    /// </summary>
    [Parameter]
    public bool? AnimatePeriodChanges { get; set; }

    /// <summary />
    private string AnimationClass => _animationRunning switch
    {
        VerticalPosition.Top => "animation-running-up",
        VerticalPosition.Bottom => "animation-running-down",
        _ => "animation-none"
    };

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => _calendarExtended ?? new CalendarExtended(this.Culture, this.PickerMonth);

    /// <summary>
    /// Gets titles to use in the calendar.
    /// </summary>
    /// <returns></returns>
    internal CalendarTitles GetTitles()
    { 
        return new CalendarTitles(this);
    }

    /// <summary />
    private async Task OnPreviousButtonHandler(MouseEventArgs e)
    {
        bool animate = AnimatePeriodChanges ?? (View != CalendarViews.Days);

        if (animate)
        {
            await CleanAnimation();
            _animationRunning = VerticalPosition.Bottom;
        }

        switch (View)
        {
            case CalendarViews.Days:
                PickerMonth = PickerMonth.AddMonths(-1);
                break;

            case CalendarViews.Months:
                _pickerMonth = PickerMonth.AddYears(-1);
                break;

            case CalendarViews.Years:
                _pickerMonth = PickerMonth.AddYears(-12);
                break;
        }
    }

    /// <summary />
    private async Task OnNextButtonHandler(MouseEventArgs e)
    {
        bool animate = AnimatePeriodChanges ?? (View != CalendarViews.Days);

        if (animate)
        {
            await CleanAnimation();
            _animationRunning = VerticalPosition.Top;
        }

        switch (View)
        {
            case CalendarViews.Days:
                PickerMonth = PickerMonth.AddMonths(+1);
                break;

            case CalendarViews.Months:
                _pickerMonth = PickerMonth.AddYears(+1);
                break;

            case CalendarViews.Years:
                _pickerMonth = PickerMonth.AddYears(+12);
                break;
        }
    }

    /// <summary />
    private bool MonthSelected(int? year, int? month)
    {
        if (Value == null || year == null || month == null)
        {
            return false;
        }

        if (Value.Value.Year == year && Value.Value.Month == month)
        {
            return true;
        }

        return false;
    }

    /// <summary />
    private Task OnSelectMonthHandlerAsync(int? year, int? month)
    {
        if (year != null && month != null)
        {
            Value = new DateTime(year.Value, month.Value, 1);
        }

        return Task.CompletedTask;
    }

    /// <summary />
    private Task OnSelectYearHandlerAsync(int? year)
    {
        if (year != null)
        {
            Value = new DateTime(year.Value, 1, 1);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns the class name to display a day (day, inactive, today).
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private FluentCalendarDay GetDayProperties(DateTime day) => new(this, day);

    /// <summary>
    /// Returns the class name to display a month (month, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    private FluentCalendarMonth GetMonthProperties(int? year, int? month) => new(this, new DateTime(year ?? PickerMonth.Year, month ?? PickerMonth.Month, 1));

    /// <summary>
    /// Returns the class name to display a year (year, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private FluentCalendarYear GetYearProperties(int? year) => new(this, new DateTime(year ?? PickerMonth.Year, 1, 1));

    /// <summary />
    private DateTime FirstDayOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    /// <summary />
    private async Task CleanAnimation()
    {
        // Remove the current animation
        _animationRunning = VerticalPosition.Unset;
        await Task.Delay(1);
        StateHasChanged();
    }
}
