// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a customizable and interactive calendar component that supports various views, date selection modes,
/// and animations for period changes.
/// </summary>
public partial class FluentCalendar : FluentCalendarBase
{
    private ElementReference _calendarReference = default!;
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "DateTime/FluentCalendar.razor.js";

    internal static string ArrowUp = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M4.2 10.73a.75.75 0 001.1 1.04l5.95-6.25v14.73a.75.75 0 001.5 0V5.52l5.95 6.25a.75.75 0 001.1-1.04l-7.08-7.42a1 1 0 00-1.44 0L4.2 10.73z\"/></svg>";
    internal static string ArrowDown = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M19.8 13.27a.75.75 0 00-1.1-1.04l-5.95 6.25V3.75a.75.75 0 10-1.5 0v14.73L5.3 12.23a.75.75 0 10-1.1 1.04l7.08 7.42a1 1 0 001.44 0l7.07-7.42z\"/></svg>";

    private CalendarViews _pickerView = CalendarViews.Days;
    private bool _refreshAccessibilityPending;
    private AnimationRunning _animationRunning = AnimationRunning.None;
    private DateTime? _pickerMonth;
    private readonly RangeOfDates _rangeSelector = new();

    private readonly RangeOfDates _rangeSelectorMouseOver = new();
    private readonly List<DateTime> _selectedDatesMouseOver = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendar"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings used to initialize the calendar. Cannot be null.</param>
    public FluentCalendar(LibraryConfiguration configuration) : base(configuration)
    {
        // Default conditions for the message
        MessageCondition = (field) =>
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = Localizer[Localization.LanguageResource.Calendar_RequiredMessage];

            return FocusLost &&
                   (Required ?? false)
                   && !(Disabled ?? false)
                   && !ReadOnly
                   && CurrentValue is null;
        };
    }

    /// <summary />
    protected string? CalendarClass
    {
        get
        {
            return new CssBuilder()
                .AddClass("fluent-calendar")
                .AddClass("fluent-day-view", () => View == CalendarViews.Days)
                .AddClass("fluent-month-view", () => View == CalendarViews.Months)
                .AddClass("fluent-year-view", () => View == CalendarViews.Years)
                .Build();
        }
    }

    private CalendarViews PickerView
    {
        get
        {
            return _pickerView;
        }
        set
        {
            _pickerView = value;
            _refreshAccessibilityPending = true;
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
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "TO DO")]
    public virtual DateTime PickerMonth
    {
        get
        {
            return (_pickerMonth ?? Value ?? DateTimeProvider.Today).StartOfMonth(Culture);
        }

        set
        {
            var month = value.StartOfMonth(Culture);

            if (month == _pickerMonth)
            {
                return;
            }

            _pickerMonth = month;
            _ = PickerMonthChanged.InvokeAsync(month);
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
    /// Gets or sets a value indicating whether today's date should be highlighted in the calendar.
    /// </summary>
    [Parameter]
    public bool DisplayToday { get; set; } = true;

    /// <summary>
    /// Gets ot sets if the calendar items are animated during a period change.
    /// By default, the animation is enabled for Months views, but disabled for Days and Years view.
    /// </summary>
    [Parameter]
    public bool? AnimatePeriodChanges { get; set; }

    /// <summary>
    /// Gets or sets the way the user can select one or more dates
    /// </summary>
    [Parameter]
    public CalendarSelectMode SelectMode { get; set; } = CalendarSelectMode.Single;

    /// <summary>
    /// Gets or sets the list of all selected dates, only when <see cref="SelectMode"/> is set to <see cref="CalendarSelectMode.Range" /> or <see cref="CalendarSelectMode.Multiple" />.
    /// </summary>
    [Parameter]
    public IEnumerable<DateTime> SelectedDates { get; set; } = [];

    /// <summary>
    /// Fired when the selected dates change.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<DateTime>> SelectedDatesChanged { get; set; }

    /// <summary>
    /// Fired when the selected mouse over change, to display the future range of dates.
    /// </summary>
    [Parameter]
    public Func<DateTime, IEnumerable<DateTime>>? SelectDatesHover { get; set; }

    /// <summary />
    internal bool IsReadOnlyOrDisabled => ReadOnly || Disabled == true;

    /// <summary />
    private string GetAnimationClass(string existingClass) => CanBeAnimated ? _animationRunning switch
    {
        AnimationRunning.Up => $"{existingClass} animation-running-up",
        AnimationRunning.Down => $"{existingClass} animation-running-down",
        _ => $"{existingClass} animation-none"
    } : existingClass;

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => new(Culture, PickerMonth);

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
            await RefreshAccessibilityKeyboardAsync(firstRender);
        }
        else if (_refreshAccessibilityPending)
        {
            await RefreshAccessibilityKeyboardAsync(firstRender);
            _refreshAccessibilityPending = false;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task RefreshAccessibilityKeyboardAsync(bool firstRender)
    {
        var defaultSelector = _pickerView switch
        {
            CalendarViews.Days => ".day:not([disabled]):not([inactive])",
            CalendarViews.Months => ".month:not([disabled]):not([inactive])",
            CalendarViews.Years => ".year:not([disabled]):not([inactive])",
            _ => null,
        };

        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Calendar.SetAccessibilityKeyboard", _calendarReference, firstRender ? null : defaultSelector);
    }

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
        await StartNewAnimationAsync(AnimationRunning.Down);
        _refreshAccessibilityPending = true;

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
        await StartNewAnimationAsync(AnimationRunning.Up);
        _refreshAccessibilityPending = true;

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
    private async Task StartNewAnimationAsync(AnimationRunning position)
    {
        if (CanBeAnimated)
        {
            // Remove the current animation
            _animationRunning = AnimationRunning.None;
            await Task.Delay(1);
            StateHasChanged();

            // Start the new animation
            _animationRunning = position;
        }
    }

    /// <summary>
    /// Click on the Calendar Title to display the Month or Year selector
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
                PickerView = CalendarViews.Months;
                break;

            // Months -> Years
            case CalendarViews.Months:
                PickerView = CalendarViews.Years;
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
        PickerMonth = month ?? DateTimeProvider.Today;
        PickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Select a Year and come back to the Months view.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private async Task PickerYearSelectAsync(DateTime? year)
    {
        PickerMonth = year ?? DateTimeProvider.Today;
        PickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        BindConverter.TryConvertTo(value, Culture, out result);
        validationErrorMessage = null;
        return true;
    }

    /// <summary />
    private (bool IsMultiple, DateTime Min, DateTime Max, bool InProgress) GetMultipleSelection()
    {
        var inProgress = SelectDatesHover is not null;

        if (SelectedDates == null || !SelectedDates.Any())
        {
            return (false, DateTime.MinValue, DateTime.MinValue, inProgress);
        }

        if (SelectDatesHover is null)
        {
            inProgress = !_rangeSelector.IsValid();
        }
        else
        {
            inProgress = _rangeSelectorMouseOver.IsValid();
        }

        return
        (
            (SelectMode == CalendarSelectMode.Multiple || SelectMode == CalendarSelectMode.Range) && SelectedDates.Skip(1).Any(),
            SelectedDates.Min(),
            SelectedDates.Max(),
            inProgress
        );
    }

    /// <summary />
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "TO DO")]
    protected virtual async Task OnSelectDayHandlerAsync(DateTime value, bool dayDisabled)
    {
        if (!dayDisabled)
        {
            switch (SelectMode)
            {
                // Single selection
                case CalendarSelectMode.Single:
                    await OnSelectedDateHandlerAsync(value);
                    break;

                // Multiple selection
                case CalendarSelectMode.Multiple:

                    if (SelectDatesHover is null)
                    {
                        if (SelectedDates.Contains(value))
                        {
                            SelectedDates = SelectedDates.Where(i => i != value);
                        }
                        else
                        {
                            SelectedDates = SelectedDates.Append(value);
                        }

                        if (SelectedDatesChanged.HasDelegate)
                        {
                            await SelectedDatesChanged.InvokeAsync(SelectedDates);
                        }
                    }
                    else
                    {
                        var range = SelectDatesHover.Invoke(value);

                        SelectedDates = range.Where(day => DisabledDateFunc == null || !DisabledDateFunc(day));

                        if (SelectedDatesChanged.HasDelegate)
                        {
                            await SelectedDatesChanged.InvokeAsync(SelectedDates);
                        }
                    }

                    break;

                // Range of dates
                case CalendarSelectMode.Range:

                    var resetRange = (_rangeSelector.IsValid() || _rangeSelector.IsSingle()) && _rangeSelector.Includes(value);

                    // Reset the selection
                    if (resetRange)
                    {
                        _rangeSelector.Clear();
                        _rangeSelectorMouseOver.Clear();
                    }

                    // End the selection
                    else if (_rangeSelector.Start is not null && _rangeSelector.End is null)
                    {
                        _rangeSelector.End = value;
                    }

                    // Start and close a pre-selection
                    else if (SelectDatesHover is not null)
                    {
                        var range = SelectDatesHover.Invoke(value);

                        _rangeSelector.Start = range.Min();
                        _rangeSelector.End = range.Max();
                    }

                    // Start the selection
                    else
                    {
                        _rangeSelector.Start = value;
                        _rangeSelector.End = null;

                        await OnSelectDayMouseOverAsync(value, dayDisabled: false);
                    }

                    SelectedDates = _rangeSelector.GetAllDates().Where(day => DisabledDateFunc == null || !DisabledDateFunc(day));

                    if (SelectedDatesChanged.HasDelegate)
                    {
                        await SelectedDatesChanged.InvokeAsync(SelectedDates);
                    }

                    break;
            }
        }
    }

    /// <summary />
    private Task OnSelectDayMouseOverAsync(DateTime value, bool dayDisabled)
    {
        if (dayDisabled ||
            SelectMode == CalendarSelectMode.Single ||
            (_rangeSelector.IsSingle() && SelectDatesHover is null))
        {
            return Task.CompletedTask;
        }

        if (SelectDatesHover is null)
        {
            _rangeSelectorMouseOver.Start = _rangeSelector.Start ?? value;
            _rangeSelectorMouseOver.End = value;
        }
        else
        {
            var range = SelectDatesHover.Invoke(value);
            _rangeSelectorMouseOver.Start = range.Min();
            _rangeSelectorMouseOver.End = range.Max();
        }

        var days = DisabledDateFunc is null
                 ? _rangeSelectorMouseOver.GetAllDates()
                 : _rangeSelectorMouseOver.GetAllDates().Where(day => !DisabledDateFunc(day));

        _selectedDatesMouseOver.Clear();
        _selectedDatesMouseOver.AddRange(days);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <returns></returns>
    public virtual Task FocusOutHandlerAsync(FocusEventArgs? e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Check if all days between two dates are disabled.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    internal bool AllDaysAreDisabled(DateTime start, DateTime end)
    {
        if (DisabledDateFunc is null)
        {
            return false;
        }

        for (var day = start; day <= end; day = day.AddDays(1))
        {
            if (!DisabledDateFunc.Invoke(day))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary />
    private string GetFormValue()
    {
        switch (SelectMode)
        {
            case CalendarSelectMode.Single:
                return Value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty;

            case CalendarSelectMode.Range:
            case CalendarSelectMode.Multiple:
                return string.Join(",", SelectedDates.Select(d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

            default:
                return string.Empty;
        }
    }

    private enum AnimationRunning
    {
        None,
        Up,
        Down,
    }
}

