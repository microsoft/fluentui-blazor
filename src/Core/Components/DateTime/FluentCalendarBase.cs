using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public abstract class FluentCalendarBase : FluentInputBase<DateTime?>
{
    private DateTime? _selectedDate = null;

    /// <summary>
    /// Gets or sets the culture of the component.
    /// By default <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

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
    /// Gets or sets the Type style for the day (numeric or 2-digits).
    /// </summary>
    [Parameter]
    public DayFormat? DayFormat { get; set; } = AspNetCore.Components.DayFormat.Numeric;

    /// <summary>
    /// Gets or sets the selected date (two-way bindable).
    /// </summary>
    [Parameter]
    public override DateTime? Value
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

            if (ValueChanged.HasDelegate)
            {
                ValueChanged.InvokeAsync(value);
            }
        }
    }

    /// <summary />
    protected virtual Task OnSelectedDateHandlerAsync(DateTime? value)
    {
        if (!ReadOnly)
        {
            Value = value;
        }

        return Task.CompletedTask;
    }

    /// <summary />
    protected virtual async Task OnSelectDayHandlerAsync(DateTime? value, bool dayDisabled)
    {
        if (!dayDisabled)
        {
            await OnSelectedDateHandlerAsync(value);
        }
    }
}
