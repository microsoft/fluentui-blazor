using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
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

    internal CalendarViews _pickerView = CalendarViews.Days;
    private VerticalPosition _animationRunning = VerticalPosition.Unset;
    private DateTime? _pickerMonth = null;
    private readonly CalendarExtended? _calendarExtended = null;

    /// <summary />
    protected override string? ClassValue
    {
        get
        {
            return new CssBuilder(base.ClassValue)
                .AddClass("fluent-calendar", () => View == CalendarViews.Days)
                .AddClass("fluent-month", () => View == CalendarViews.Months)
                .AddClass("fluent-year", () => View == CalendarViews.Years)
                .Build();
        }
    }

    /// <summary>
    /// Gets or sets the current month of the date picker (two-way bindable).
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
            return (_pickerMonth ?? Value ?? DateTime.Today).StartOfMonth(Culture);
        }

        set
        {
            var month = value.StartOfMonth(Culture);

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
    /// Gets ot sets if the calendar items are animated during a period change.
    /// By default, the animation is enabled for Months views, but disabled for Days and Years view.
    /// </summary>
    [Parameter]
    public bool? AnimatePeriodChanges { get; set; }

    /// <summary />
    private string GetAnimationClass(string existingClass) => CanBeAnimated ? _animationRunning switch
    {
        VerticalPosition.Top => $"{existingClass} animation-running-up",
        VerticalPosition.Bottom => $"{existingClass} animation-running-down",
        _ => $"{existingClass} animation-none"
    } : existingClass;

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => _calendarExtended ?? new CalendarExtended(Culture, PickerMonth);

    /// <summary>
    /// Gets titles to use in the calendar.
    /// </summary>
    /// <returns></returns>
    internal CalendarTitles GetTitles()
    {
        return new CalendarTitles(this);
    }

    /// <summary />
    private async Task OnPreviousButtonHandlerAsync(MouseEventArgs e)
    {
        await StartNewAnimationAsync(VerticalPosition.Bottom);

        switch (View)
        {
            case CalendarViews.Days:
                PickerMonth = PickerMonth.AddMonths(-1, Culture);
                break;

            case CalendarViews.Months:
                PickerMonth = PickerMonth.AddYears(-1, Culture);
                break;

            case CalendarViews.Years:
                PickerMonth = PickerMonth.AddYears(-12, Culture);
                break;
        }
    }

    /// <summary />
    private async Task OnNextButtonHandlerAsync(MouseEventArgs e)
    {
        await StartNewAnimationAsync(VerticalPosition.Top);

        switch (View)
        {
            case CalendarViews.Days:
                PickerMonth = PickerMonth.AddMonths(+1, Culture);
                break;

            case CalendarViews.Months:
                PickerMonth = PickerMonth.AddYears(+1, Culture);
                break;

            case CalendarViews.Years:
                PickerMonth = PickerMonth.AddYears(+12, Culture);
                break;
        }
    }

    /// <summary />
    private async Task OnSelectMonthHandlerAsync(int year, int month, bool isReadOnly)
    {
        if (!isReadOnly)
        {
            var value = Culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
            await OnSelectedDateHandlerAsync(value);
        }
    }

    /// <summary />
    private async Task OnSelectYearHandlerAsync(int year, bool isReadOnly)
    {
        if (!isReadOnly)
        {
            var value = Culture.Calendar.ToDateTime(year, 1, 1, 0, 0, 0, 0);
            await OnSelectedDateHandlerAsync(value);
        }
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
    private FluentCalendarMonth GetMonthProperties(int? year, int? month) => new(this, Culture.Calendar.ToDateTime(year ?? PickerMonth.GetYear(Culture), month ?? PickerMonth.GetMonth(Culture), 1, 0, 0, 0, 0));

    /// <summary>
    /// Returns the class name to display a year (year, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private FluentCalendarYear GetYearProperties(int? year) => new(this, Culture.Calendar.ToDateTime(year ?? PickerMonth.GetYear(Culture), 1, 1, 0, 0, 0, 0));

    /// <summary />
    private bool CanBeAnimated => AnimatePeriodChanges ?? (View != CalendarViews.Days && View != CalendarViews.Years);

    /// <summary />
    private async Task StartNewAnimationAsync(VerticalPosition position)
    {
        if (CanBeAnimated)
        {
            // Remove the current animation
            _animationRunning = VerticalPosition.Unset;
            await Task.Delay(1);
            StateHasChanged();

            // Start the new animation
            _animationRunning = position;
        }
    }

    /// <summary>
    /// Click on the Calendar Title to disply the Month or Year selector
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    private async Task TitleClickHandlerAsync(CalendarTitles title)
    {
        if (title.ReadOnly)
        {
            return;
        }

        switch (View)
        {
            // Days -> Months
            case CalendarViews.Days:
                _pickerView = CalendarViews.Months;
                break;

            // Months -> Years
            case CalendarViews.Months:
                _pickerView = CalendarViews.Years;
                break;
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Select a Month and come back to the Days view.
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    private async Task PickerMonthSelectAsync(DateTime? month)
    {
        PickerMonth = month ?? DateTime.Today;
        _pickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Select a Year and come back to the Months view.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private async Task PickerYearSelectAsync(DateTime? year)
    {
        PickerMonth = year ?? DateTime.Today;
        _pickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        BindConverter.TryConvertTo(value, Culture, out result);
        validationErrorMessage = null;
        return true;
    }
}
