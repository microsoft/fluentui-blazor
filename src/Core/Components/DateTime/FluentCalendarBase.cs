using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public abstract class FluentCalendarBase : FluentInputBase<DateTime?>
{
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
    /// Gets or sets the verification to do when the selected value has changed.
    /// By default, ValueChanged is called only if the selected value has changed.
    /// </summary>
    [Parameter]
    public bool CheckIfSelectedValueHasChanged { get; set; } = true;

    /// <summary>
    /// Defines the appearance of the <see cref="FluentCalendar"/> component.
    /// </summary>
    [Parameter]
    public virtual CalendarViews View { get; set; } = CalendarViews.Days;

    /// <summary />
    protected virtual async Task OnSelectedDateHandlerAsync(DateTime? value)
    {
        if (CheckIfSelectedValueHasChanged && Value == value)
        {
            return;
        }

        if (!ReadOnly)
        {
            Value = value;
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(value);
            }
            if (FieldBound)
            {
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }
    }
}
