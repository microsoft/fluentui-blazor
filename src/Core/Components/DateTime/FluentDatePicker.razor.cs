// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Calendar;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a date picker control that enables users to select a date using a fluent user interface.
/// </summary>
/// <typeparam name="TValue">The type of value handled by the date picker. Must be one of: DateTime?, DateTime, DateOnly, or DateOnly?.</typeparam>
public partial class FluentDatePicker<TValue> : FluentCalendarBase<TValue>
{
    private bool _popupOpenedByKeyboard;
    private FluentCalendar<TValue> _calendar = default!;
    private FluentIcon<Icon> _icon = default!;

    /// <summary>
    /// Initializes a new instance of the FluentDatePicker class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to apply to the date picker. Cannot be null.</param>
    public FluentDatePicker(LibraryConfiguration configuration) : base(configuration)
    {
        // Default conditions for the message
        MessageCondition = (field) =>
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = Localizer[Localization.LanguageResource.TextInput_RequiredMessage];

            return FocusLost &&
                   (Required ?? false)
                   && !(Disabled ?? false)
                   && !ReadOnly
                   && CurrentValue.IsNull();
        };
    }

    /// <summary />
    protected override string? ClassValue => DefaultClassBuilder
        .AddClass(base.CssClass)
        .AddClass("fluent-datepicker")
        .Build();

    /// <summary />
    protected override string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, when: () => !string.IsNullOrEmpty(Width))
        .Build();

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Regular.Size20.Calendar().WithColor("currentColor");

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public TextInputAppearance Appearance { get; set; } = TextInputAppearance.Outline;

    /// <summary />
    [Parameter]
    public DatePickerRenderStyle RenderStyle { get; set; } = DatePickerRenderStyle.FluentUI;

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
    /// Gets or sets the callback that is invoked when a double-click event occurs on the component.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets a value which will be set when double-clicking on the text field of date picker.
    /// </summary>
    [Parameter]
    public TValue? DoubleClickToDate { get; set; }

    /// <summary>
    /// Gets or sets the template used to render each day in the calendar.
    /// </summary>
    /// <remarks>Use this parameter to customize the appearance and content of individual days. The template
    /// receives a <see cref="FluentCalendarDay{TValue}"/> parameter representing the day to render.
    /// </remarks>
    [Parameter]
    public RenderFragment<FluentCalendarDay<TValue>>? DaysTemplate { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the selected month in the picker changes.
    /// </summary>
    [Parameter]
    public EventCallback<TValue> PickerMonthChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the calendar is opened or closed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnCalendarOpen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the date picker is currently open.
    /// </summary>
    public bool Opened { get; set; }

    /// <summary />
    protected virtual async Task OnTextInputClickAsync(MouseEventArgs e)
    {
        // Simple click
        if (IsFluentUIStyle && e.Detail == 1 && !ReadOnly)
        {
            Opened = !Opened;

            if (OnCalendarOpen.HasDelegate)
            {
                await OnCalendarOpen.InvokeAsync(Opened);
            }
        }

        // Double click
        if (e.Detail >= 2 && !ReadOnly)
        {
            if (DoubleClickToDate.IsNotNull())
            {
                await OnSelectedDateAsync(DoubleClickToDate ?? default!);
            }

            if (OnDoubleClick.HasDelegate)
            {
                await OnDoubleClick.InvokeAsync(e);
            }
        }
    }

    /// <summary />
    protected virtual async Task OnIconKeydownAsync(KeyboardEventArgs e)
    {
        if (string.Equals(e.Code, "Enter", StringComparison.Ordinal) ||
            string.Equals(e.Code, "Space", StringComparison.Ordinal))
        {
            _popupOpenedByKeyboard = true;
            await OnTextInputClickAsync(new MouseEventArgs() { Detail = 1 });
            await _calendar.SetFirstFocusableAsync();
        }
    }

    /// <summary />
    protected async Task OnSelectedDateAsync(TValue value)
    {
        var dateTimeValue = value.ConvertToDateTime();
        var updatedValue = dateTimeValue;

        if (CurrentValue.IsNotNull() && dateTimeValue is not null)
        {
            var currentDateTime = CurrentValue.ConvertToDateTime();
            updatedValue = currentDateTime?.TimeOfDay != TimeSpan.Zero
                         ? dateTimeValue?.Date + currentDateTime?.TimeOfDay
                         : dateTimeValue;
        }

        Opened = false;

        if (IsFluentUIStyle && _popupOpenedByKeyboard)
        {
            await _icon.Element.FocusAsync();
            _popupOpenedByKeyboard = false;
        }

        await OnSelectedDateHandlerAsync(updatedValue is null ? default : updatedValue.Value.ConvertToTValue<TValue>()); 
    }

    /// <summary />
    protected override string? FormatValueAsString(TValue? value)
    {
        var dateValue = value.ConvertToDateTime();

        // FluentUI style
        if (IsFluentUIStyle)
        {
            return View switch
            {
                CalendarViews.Years => dateValue?.ToString("yyyy", Culture),
                CalendarViews.Months => dateValue?.ToString(Culture.DateTimeFormat.YearMonthPattern, Culture),
                _ => dateValue?.ToString(Culture.DateTimeFormat.ShortDatePattern, Culture),
            };
        }

        // Native style
        return View switch
        {
            CalendarViews.Years => dateValue?.ToString("yyyy", CultureInfo.InvariantCulture),
            CalendarViews.Months => dateValue?.ToString("yyyy-MM", CultureInfo.InvariantCulture),
            _ => dateValue?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        };
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (View == CalendarViews.Years && int.TryParse(value, Culture, out var year))
        {
            result = new DateTime(year, 1, 1).ConvertToTValue<TValue>();
            validationErrorMessage = null;
            return true;
        }

        return base.TryParseValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary />
    private string GetPlaceholderAccordingToView()
    {
        if (!string.IsNullOrEmpty(Placeholder))
        {
            return Placeholder;
        }

        return View switch
        {
            CalendarViews.Years => "yyyy",
            CalendarViews.Months => Culture.DateTimeFormat.YearMonthPattern,
            _ => Culture.DateTimeFormat.ShortDatePattern
        };
    }

    /// <summary>
    /// Gets a value indicating whether the date picker is using the Fluent UI style.
    /// </summary>
    private bool IsFluentUIStyle => RenderStyle == DatePickerRenderStyle.FluentUI;

    /// <summary />
    internal string? GetInputType() => IsFluentUIStyle ? null : View switch
    {
        CalendarViews.Days => "date",
        CalendarViews.Months => "month",
        CalendarViews.Years => "number",
        _ => null
    };

    /// <summary />
    internal TextInputMode? GetInputMode() => IsFluentUIStyle ? null : View switch
    {
        CalendarViews.Years => TextInputMode.Numeric,
        _ => null
    };

    /// <summary />
    private Task PickerMonthChangedHandlerAsync(TValue? month)
    {
        if (PickerMonthChanged.HasDelegate)
        {
            return PickerMonthChanged.InvokeAsync(month ?? default!);
        }

        return Task.CompletedTask;
    }
}
