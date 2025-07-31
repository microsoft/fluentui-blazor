// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides a base class for building calendar components.
/// </summary>
public abstract class FluentCalendarBase : FluentInputBase<DateTime?>
{
    /// <summary />
    protected FluentCalendarBase(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary>
    /// Gets or sets the verification to do when the selected value has changed.
    /// By default, ValueChanged is called only if the selected value has changed.
    /// </summary>
    [CascadingParameter(Name = "CheckIfSelectedValueHasChanged")]
    internal bool? CheckIfSelectedValueHasChanged { get; set; }

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
    /// By default, the <see cref="DisabledDateFunc" /> check only the first day of the month and the first day of the year for the Month and Year views.
    /// Set this property to `true` to check if all days of the month and year are disabled (more time consuming).
    /// </summary>
    [Parameter]
    public virtual bool DisabledCheckAllDaysOfMonthYear { get; set; }

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
    public CalendarDayFormat? DayFormat { get; set; } = CalendarDayFormat.Numeric;

    /// <summary>
    /// Defines the appearance of the <see cref="FluentCalendar"/> component.
    /// </summary>
    [Parameter]
    public virtual CalendarViews View { get; set; } = CalendarViews.Days;

    /// <summary />
    protected virtual Task OnSelectedDateHandlerAsync(DateTime? value)
    {
        if (ReadOnly)
        {
            return Task.CompletedTask;
        }

        if ((CheckIfSelectedValueHasChanged ?? true) && CurrentValue == value)
        {
            return Task.CompletedTask;
        }

        CurrentValue = value;
        return Task.CompletedTask;
    }
}
