// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Calendar;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentTimePicker<TValue> : FluentInputBase<TValue>
{
    private DateTime DefaultTime => Culture.Calendar.MinSupportedDateTime;
    private FluentCombobox<DateTime?, DateTime?> _fluentCombobox = default!;

    /// <summary />
    public FluentTimePicker(LibraryConfiguration configuration) : base(configuration)
    {
        if (typeof(TValue).IsNotTimeType())
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
    /// Gets or sets the culture of the component.
    /// By default <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

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
    /// Function to know if a specific time must be disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TValue, bool>? DisabledTimeFunc { get; set; }

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
    /// Gets the short time pattern used by the current culture for formatting time values.
    /// </summary>
    private string TimePattern => Culture.DateTimeFormat.ShortTimePattern;

    /// <summary>
    /// Gets a collection of nullable <see cref="DateTime"/> values representing each hour within the configured range.
    /// </summary>
    private IEnumerable<DateTime?> Items
    {
        get
        {
            var count = EndHour - StartHour < 1 ? 1 : EndHour - StartHour + 1;

            return Enumerable.Range(0, count * (60 / Increment) - 1)
                             .Select(i => (DateTime?)DefaultTime.AddHours(StartHour).AddMinutes(i * Increment));
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
            CurrentValueAsString = value?.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the date picker is using the Fluent UI style.
    /// </summary>
    private bool IsFluentUIStyle => RenderStyle == DatePickerRenderStyle.FluentUI;

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !IsFluentUIStyle)
        {
            // Set the attribute min/max/step on the shadow "control" element.
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.copyToShadow",
                Id,
                "[part='control']",
                "min", DefaultTime.AddHours(StartHour).ToString("HH:mm", CultureInfo.InvariantCulture));

            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.copyToShadow",
                Id,
                "[part='control']",
                "max", DefaultTime.AddHours(EndHour).ToString("HH:mm", CultureInfo.InvariantCulture));

            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.copyToShadow",
                Id,
                "[part='control']",
                "step", Increment);
        }
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        var acceptedFormats = new string[] { "HH:mm", "HH:mm:ss", "HH:mm:ss.fff" };
        var currentValue = Value.ConvertToDateTime()?.Date ?? DefaultTime;

        if (DateTime.TryParseExact(value, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            result = (currentValue.Date + dateTime.TimeOfDay).ConvertToTValue<TValue>();
            validationErrorMessage = null;
            return true;
        }

        result = default!;
        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, Localizer[Localization.LanguageResource.Calendar_FieldMustBeADate], DisplayName ?? FieldIdentifier.FieldName);
        return false;
    }

    /// <summary />
    protected override string? FormatValueAsString(TValue? value)
    {
        return value switch
        {
            DateTime dt => dt.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
            TimeOnly to => to.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
            _ => null
        };
    }

    /// <summary />
    private static string? FormatValueAsString(DateTime? value)
    {
        return value?.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
    }

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
        if (dateTime is null || DisabledTimeFunc is null)
        {
            return false;
        }

        var value = CalendarTValue.ConvertToTValue<TValue>(dateTime ?? throw new ArgumentNullException(nameof(dateTime)));
        return DisabledTimeFunc(value);
    }

    /// <summary />
    private async Task KeyDownHandlerAsync(KeyboardEventArgs args)
    {
        var isNullable = Nullable.GetUnderlyingType(typeof(TValue)) != null || !typeof(TValue).IsValueType;

        if (isNullable && string.Equals(args.Key, "Delete", StringComparison.OrdinalIgnoreCase))
        {
            await _fluentCombobox.ClearAsync();
        }
    }

    /// <summary />
    private TextInputAppearance TextInputAppearance
    {
        get
        {
            return Appearance switch
            {
                ListAppearance.FilledLighter => TextInputAppearance.FilledLighter,
                ListAppearance.FilledDarker => TextInputAppearance.FilledDarker,
                ListAppearance.Outline => TextInputAppearance.Outline,
                ListAppearance.Transparent => TextInputAppearance.Underline,
                _ => TextInputAppearance.Outline
            };
        }
    }
}
