// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Calendar;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentTimePicker<TValue> : FluentInputBase<TValue>
{
    /// <summary />
    public FluentTimePicker(LibraryConfiguration configuration) : base(configuration)
    {
        if (typeof(TValue).IsNotDateType())
        {
            throw new InvalidOperationException($"The type parameter {typeof(TValue)} is not supported. Supported types are DateTime, DateTime?, TimeOnly, and TimeOnly?.");
        }

        // Default conditions for the message
        MessageCondition = (field) =>
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = Localizer[Localization.LanguageResource.TextInput_RequiredMessage];

            return FocusLost &&
                   (Required ?? false)
                   && !(Disabled ?? false)
                   && !ReadOnly
                   && CurrentValue.IsNullOrDefault();
        };
    }

    /// <summary />
    protected override string? ClassValue => DefaultClassBuilder
        .AddClass(base.CssClass)
        .AddClass("fluent-timepicker")
        .Build();

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public ListAppearance Appearance { get; set; } = ListAppearance.Outline;

    /// <summary />
    [Parameter]
    public DatePickerRenderStyle RenderStyle { get; set; } = DatePickerRenderStyle.FluentUI;

    /// <summary>
    /// Gets or sets the format string used to display time values.
    /// If not <see cref="Placeholder"/> is set, the placeholder will be set to this time pattern.
    /// By default "HH:mm".
    /// </summary>
    [Parameter]
    public string TimePattern { get; set; } = "HH:mm";

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Function to know if a specific hour must be disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TValue, bool>? DisabledHourFunc { get; set; }

    /// <summary>
    /// Gets or sets the hour of the day at which the operation or schedule should start, in 24-hour format.
    /// </summary>
    [Parameter]
    public int StartHour { get; set; } = 8;

    /// <summary>
    /// Gets or sets the ending hour for the time range, in 24-hour format.
    /// </summary>
    [Parameter]
    public int EndHour { get; set; } = 18;

    /// <summary>
    /// Gets or sets the increment, in minutes, between each time option in the dropdown list.
    /// </summary>
    [Parameter]
    public int Increment { get; set; } = 15;

    /// <summary>
    /// Gets a collection of nullable <see cref="DateTime"/> values representing each hour within the configured range.
    /// </summary>
    private IEnumerable<DateTime?> Items
    {
        get
        {
            var count = EndHour - StartHour < 1 ? 1 : EndHour - StartHour + 1;

            return Enumerable.Range(0, count * (60 / Increment))
                             .Select(i => (DateTime?)new DateTime().AddHours(StartHour).AddMinutes(i * Increment));
        }
    }

    /// <summary>
    /// Gets or sets the selected time value as a <see cref="DateTime"/> object, or <see langword="null"/> if no value
    /// is selected.
    /// </summary>
    private DateTime? SelectedValue
    {
        get
        {
            return DateTime.TryParse(CurrentValueAsString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime) ? dateTime : null;
        }
        set
        {
            Console.WriteLine(value?.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
            CurrentValueAsString = value?.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the date picker is using the Fluent UI style.
    /// </summary>
    private bool IsFluentUIStyle => RenderStyle == DatePickerRenderStyle.FluentUI;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, out var dateTime))
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
    internal string? GetInputType() => IsFluentUIStyle ? null : "time";

    /// <summary />
    private string GetPlaceholderAccordingToView()
    {
        if (!string.IsNullOrEmpty(Placeholder))
        {
            return Placeholder;
        }

        return TimePattern;
    }

    /// <summary />
    private bool DisableHourHandler(DateTime? dateTime)
    {
        if (dateTime is null || DisabledHourFunc is null)
        {
            return false;
        }

        var value = CalendarTValue.ConvertToTValue<TValue>(dateTime ?? throw new ArgumentNullException(nameof(dateTime)));
        return DisabledHourFunc(value);
    }
}
