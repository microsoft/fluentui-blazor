using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public abstract class FluentCalendarBase : FluentComponentBase
{
    private static readonly CultureInfo DEFAULT_CULTURE = CultureInfo.GetCultureInfo("en-US");
    private DateTime? _selectedDate = null;

    /// <summary>
    /// Gets or sets if the calendar is readonly 
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; } = false;

    /// <summary>
    /// The culture of the component. By default "en-US".
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = DEFAULT_CULTURE;

    /// <summary>
    /// Function to know if a specific day must be disabled.
    /// </summary>
    [Parameter]
    public virtual Func<DateTime, bool>? DisabledDateFunc { get; set; }

    /// <summary>
    /// Apply the disabled style to the <see cref="DisabledDateFunc"/> days.
    /// If this is not the case, the days are displayed like the others, but cannot be selected.
    /// </summary>
    [Parameter] 
    public virtual bool DisabledSelectable { get; set; } = true;

    /// <summary>
    /// Format style for the day (numeric or 2-digits).
    /// </summary>
    [Parameter]
    public DayFormat? DayFormat { get; set; } = FluentUI.DayFormat.Numeric;

    /// <summary>
    /// Selected date (two-way bindable).
    /// </summary>
    [Parameter]
    public virtual DateTime? SelectedDate
    {
        get
        {
            return _selectedDate;
        }

        set
        {
            if (_selectedDate == value)
            {
                return;
            }

            _selectedDate = value;
            SelectedDateChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Fired when the display month changes.
    /// </summary>
    [Parameter]
    public virtual EventCallback<DateTime?> SelectedDateChanged { get; set; }

    /// <summary>
    /// Event raised when a date is selected (clicked).
    /// </summary>
    [Parameter]
    public virtual EventCallback<DateTime?> OnSelectedDate { get; set; }

    [Parameter]
    public EventCallback<DateOnly> OnDateClicked { get; set; }

    /// <summary />
    protected virtual async Task OnSelectedDateHandlerAsync(DateTime? value)
    {
        if (ReadOnly)
        {
            return;
        }

        SelectedDate = value;

        if (OnSelectedDate.HasDelegate)
        {
            await OnSelectedDate.InvokeAsync(value);
        }

        if (OnDateClicked.HasDelegate && value != null)
        {
            await OnDateClicked.InvokeAsync(DateOnly.FromDateTime(value.Value));
        }
    }

    /// <summary />
    protected virtual async Task OnSelectedDateHandlerAsync(DateTime? value, bool dayDisabled)
    {
        if (!dayDisabled)
        {
            await OnSelectedDateHandlerAsync(value);
        }
    }
}
