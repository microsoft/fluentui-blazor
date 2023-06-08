using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public abstract class FluentCalendarBase : FluentComponentBase
{
    private static readonly CultureInfo DEFAULT_CULTURE = CultureInfo.GetCultureInfo("en-US");
    private DateTime? _selectedDate = null;

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

    /// <summary />
    protected virtual void OnSelectedDateHandler(DateTime? value)
    {
        this.SelectedDate = value;
        OnSelectedDate.InvokeAsync(value);
    }

    /// <summary />
    protected virtual void OnSelectedDateHandler(DateTime? value, bool dayDisabled)
    {
        if (!dayDisabled)
        {
            OnSelectedDateHandler(value);
        }
    }
}
