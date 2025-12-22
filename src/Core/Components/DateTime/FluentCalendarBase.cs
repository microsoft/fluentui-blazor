// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Calendar;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides a base class for building calendar components.
/// </summary>
/// <typeparam name="TValue">The type of value handled by the calendar. Must be one of: DateTime?, DateTime, DateOnly, or DateOnly?.</typeparam>
public abstract class FluentCalendarBase<TValue> : FluentInputBase<TValue>
{
    /* ************************************************************************************
     * Dev Note: The TValue cannot be constrained to `where TValue : struct, IComparable`
     * because it can be either a nullable or non-nullable value type.
     * So, the CalendarTValue.IsNullOrDefault() extension method returns true if the value is null or equal to the default value (Date.Min).
     * ************************************************************************************/

    /// <summary />
    protected FluentCalendarBase(LibraryConfiguration configuration) : base(configuration)
    {
        if (typeof(TValue).IsNotDateType())
        {
            throw new InvalidOperationException($"The type parameter {typeof(TValue)} is not supported. Supported types are DateTime, DateTime?, DateOnly, and DateOnly?.");
        }
    }

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
    public virtual Func<TValue, bool>? DisabledDateFunc { get; set; }

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
    /// Defines the appearance of the <see cref="FluentCalendar{TValue}"/> component.
    /// </summary>
    [Parameter]
    public virtual CalendarViews View { get; set; } = CalendarViews.Days;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (DateTime.TryParse(value, Culture, out var dateTime))
        {
            result = dateTime.ConvertToTValue<TValue>();
            validationErrorMessage = null;
            return true;
        }

        result = default!;
        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, Localizer[Localization.LanguageResource.Calendar_FieldMustBeADate], DisplayName ?? FieldIdentifier.FieldName);
        return false;
    }

    /// <summary />
    protected virtual Task OnSelectedDateHandlerAsync(TValue? value)
    {
        if (ReadOnly || Disabled == true)
        {
            return Task.CompletedTask;
        }

        var dateTime = value.ConvertToDateTime();
        if ((CheckIfSelectedValueHasChanged ?? true) && CurrentValue.ConvertToDateTime() == dateTime)
        {
            return Task.CompletedTask;
        }

        CurrentValue = value;
        return Task.CompletedTask;
    }
}
