// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Calendar;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a customizable and interactive calendar component that supports various views, date selection modes,
/// and animations for period changes.
/// </summary>
/// <typeparam name="TValue">The type of value handled by the calendar. Must be one of: DateTime?, DateTime, DateOnly, or DateOnly?.</typeparam>
public partial class FluentCalendar<TValue> : FluentCalendarBase<TValue>
{
    private ElementReference _calendarReference = default!;
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "DateTime/FluentCalendar.razor.js";
    
    internal static string ArrowUp = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M4.2 10.73a.75.75 0 001.1 1.04l5.95-6.25v14.73a.75.75 0 001.5 0V5.52l5.95 6.25a.75.75 0 001.1-1.04l-7.08-7.42a1 1 0 00-1.44 0L4.2 10.73z\"/></svg>";
    internal static string ArrowDown = "<svg width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M19.8 13.27a.75.75 0 00-1.1-1.04l-5.95 6.25V3.75a.75.75 0 10-1.5 0v14.73L5.3 12.23a.75.75 0 10-1.1 1.04l7.08 7.42a1 1 0 001.44 0l7.07-7.42z\"/></svg>";

    private CalendarViews _pickerView = CalendarViews.Days;
    private bool _refreshAccessibilityPending;
    private AnimationRunning _animationRunning = AnimationRunning.None;
    private TValue? _pickerMonth;
    private readonly RangeOfDates _rangeSelector = new();

    private readonly RangeOfDates _rangeSelectorMouseOver = new();
    private readonly List<DateTime> _selectedDatesMouseOver = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCalendar{TValue}"/> class with the specified library configuration.
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
                   && CurrentValue.IsNullOrDefault();
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
    /// The month is represented as a TValue which is always the first day of that month.
    /// You can also set this to determine which month is displayed first.
    /// If not set, the current month is displayed.
    /// </summary>
    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Need to refactor in future release")]
    public virtual TValue? PickerMonth
    {
        get
        {
            var pickerMonthDateTime = _pickerMonth?.ConvertToDateTime() ?? ValueAsDateTime ?? DateTimeProvider.Today;
            return pickerMonthDateTime.StartOfMonth(Culture).ConvertToTValue<TValue>();
        }

        set
        {
            var monthDateTime = value.ConvertToDateTime()?.StartOfMonth(Culture);
            var currentPickerMonthDateTime = _pickerMonth?.ConvertToDateTime();

            if (monthDateTime == currentPickerMonthDateTime)
            {
                return;
            }

            _pickerMonth = monthDateTime.HasValue ? monthDateTime.Value.ConvertToTValue<TValue>() : default;
            _ = PickerMonthChanged.InvokeAsync(_pickerMonth);
        }
    }

    /// <summary>
    /// Fired when the display month changes.
    /// </summary>
    [Parameter]
    public virtual EventCallback<TValue?> PickerMonthChanged { get; set; }

    /// <summary>
    /// Defines the appearance of a Day cell.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentCalendarDay<TValue>>? DaysTemplate { get; set; }

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
    public IEnumerable<TValue> SelectedDates { get; set; } = [];

    /// <summary>
    /// Fired when the selected dates change.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TValue>> SelectedDatesChanged { get; set; }

    /// <summary>
    /// Fired when the selected mouse over change, to display the future range of dates.
    /// </summary>
    [Parameter]
    public Func<TValue, IEnumerable<TValue>>? SelectDatesHover { get; set; }

    /// <summary />
    internal bool IsReadOnlyOrDisabled => ReadOnly || Disabled == true;

    /// <summary />
    internal string GetAnimationClass(string existingClass) => CanBeAnimated ? _animationRunning switch
    {
        AnimationRunning.Up => $"{existingClass} animation-running-up",
        AnimationRunning.Down => $"{existingClass} animation-running-down",
        _ => $"{existingClass} animation-none"
    } : existingClass;

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => new(Culture, PickerMonth.ConvertToRequiredDateTime());

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

    /// <summary />
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
    /// Get the internal DateTime? value, synchronizing with CurrentValue if needed
    /// </summary>
    internal DateTime? ValueAsDateTime => CurrentValue.ConvertToDateTime();

    /// <summary>
    /// Implementation of the abstract method from FluentCalendarBase
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected Task OnSelectedDateHandlerAsync(DateTime value)
        => OnSelectedDateHandlerAsync(value.ConvertToTValue<TValue>());

    /// <summary />
    internal async Task SetFirstFocusableAsync()
    {
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Calendar.SetFirstFocusable", _calendarReference);
    }

    /// <summary>
    /// Gets titles to use in the calendar.
    /// </summary>
    /// <returns></returns>
    internal CalendarTitles<TValue> GetTitles()
    {
        return new CalendarTitles<TValue>(this);
    }

    /// <summary />
    internal async Task OnPreviousButtonHandlerAsync(MouseEventArgs _)
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
    internal async Task OnNextButtonHandlerAsync(MouseEventArgs _)
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
    private FluentCalendarDay<TValue> GetDayProperties(DateTime day) => new(this, day);

    /// <summary>
    /// Returns the class name to display a month (month, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    private FluentCalendarMonth<TValue> GetMonthProperties(int? year, int? month)
    {
        var pickerDateTime = PickerMonth.ConvertToRequiredDateTime();
        return new(this, Culture.Calendar.ToDateTime(year ?? pickerDateTime.GetYear(Culture), month ?? pickerDateTime.GetMonth(Culture), 1, 0, 0, 0, 0));
    }

    /// <summary>
    /// Returns the class name to display a year (year, inactive, disable).
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private FluentCalendarYear<TValue> GetYearProperties(int? year)
    {
        var pickerDateTime = PickerMonth.ConvertToRequiredDateTime();
        return new(this, Culture.Calendar.ToDateTime(year ?? pickerDateTime.GetYear(Culture), 1, 1, 0, 0, 0, 0));
    }

    /// <summary />
    private bool CanBeAnimated => AnimatePeriodChanges ?? (View != CalendarViews.Days && View != CalendarViews.Years);

    /// <summary />
    internal async Task StartNewAnimationAsync(AnimationRunning position)
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
    private async Task TitleClickHandlerAsync(CalendarTitles<TValue> title)
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
    internal async Task PickerMonthSelectAsync(DateTime month)
    {
        PickerMonth = month.ConvertToTValue<TValue>();
        PickerView = CalendarViews.Days;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Select a Year and come back to the Months view.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private async Task PickerYearSelectAsync(DateTime year)
    {
        PickerMonth = year.ConvertToTValue<TValue>();
        PickerView = CalendarViews.Days;
        await Task.CompletedTask;
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
            SelectedDates.MinDateTime(),
            SelectedDates.MaxDateTime(),
            inProgress
        );
    }

    /// <summary />
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
                    await OnSelectMultipleDatesAsync(value);
                    break;

                // Range of dates
                case CalendarSelectMode.Range:
                    await OnSelectRangeDatesAsync(value);
                    break;
            }
        }
    }

    /// <summary />
    private async Task OnSelectMultipleDatesAsync(DateTime value)
    {
        var tValue = value.ConvertToTValue<TValue>();

        if (SelectDatesHover is null)
        {
            if (SelectedDates.Any(d => d.ConvertToDateTime() == value))
            {
                SelectedDates = SelectedDates.Where(i => i.ConvertToDateTime() != value);
            }
            else
            {
                SelectedDates = SelectedDates.Concat([tValue]);
            }

            if (SelectedDatesChanged.HasDelegate)
            {
                await SelectedDatesChanged.InvokeAsync(SelectedDates);
            }
        }
        else
        {
            var range = SelectDatesHover.Invoke(tValue);

            SelectedDates = range.Where(day =>
            {
                var dateTime = day.ConvertToDateTime();
                return dateTime.HasValue && (DisabledDateFunc == null || !DisabledDateFunc(day));
            });

            if (SelectedDatesChanged.HasDelegate)
            {
                await SelectedDatesChanged.InvokeAsync(SelectedDates);
            }
        }
    }

    /// <summary />
    private async Task OnSelectRangeDatesAsync(DateTime value)
    {
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
            var range = SelectDatesHover.Invoke(value.ConvertToTValue<TValue>());
            _rangeSelector.Start = range.MinDateTime();
            _rangeSelector.End = range.MaxDateTime();
        }

        // Start the selection
        else
        {
            _rangeSelector.Start = value;
            _rangeSelector.End = null;

            await OnSelectDayMouseOverAsync(value, dayDisabled: false);
        }

        SelectedDates = _rangeSelector.GetAllDates()
            .Where(day => DisabledDateFunc == null || !DisabledDateFunc(day.ConvertToTValue<TValue>()))
            .Select(day => day.ConvertToTValue<TValue>());

        if (SelectedDatesChanged.HasDelegate)
        {
            await SelectedDatesChanged.InvokeAsync(SelectedDates);
        }
    }

    /// <summary />
    internal Task OnSelectDayMouseOverAsync(DateTime value, bool dayDisabled)
    {
        if (dayDisabled ||
            SelectMode == CalendarSelectMode.Single ||
            (_rangeSelector.IsSingle() && SelectDatesHover is null))
        {
            return Task.CompletedTask;
        }

        var tValue = value.ConvertToTValue<TValue>();

        if (SelectDatesHover is null)
        {
            _rangeSelectorMouseOver.Start = _rangeSelector.Start ?? value;
            _rangeSelectorMouseOver.End = value;
        }
        else
        {
            var range = SelectDatesHover.Invoke(tValue);
            _rangeSelectorMouseOver.Start = range.MinDateTime();
            _rangeSelectorMouseOver.End = range.MaxDateTime();
        }

        var days = DisabledDateFunc is null
                 ? _rangeSelectorMouseOver.GetAllDates()
                 : _rangeSelectorMouseOver.GetAllDates().Where(day => !DisabledDateFunc(day.ConvertToTValue<TValue>()));

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
            if (!DisabledDateFunc.Invoke(day.ConvertToTValue<TValue>()))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary />
    private string GetFormValue()
    {
        return SelectMode switch
        {
            CalendarSelectMode.Single
                => ValueAsDateTime?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty,

            CalendarSelectMode.Range or CalendarSelectMode.Multiple
                => string.Join(',', SelectedDates.Select(d =>
                            {
                                var dateTime = d.ConvertToDateTime();
                                return dateTime?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty;
                            }).Where(s => !string.IsNullOrEmpty(s))),

            _ => string.Empty,
        };
    }

    internal enum AnimationRunning
    {
        None,
        Up,
        Down,
    }
}

