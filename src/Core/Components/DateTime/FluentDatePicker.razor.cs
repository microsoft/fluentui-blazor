// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a date picker control that enables users to select a date using a fluent user interface.
/// </summary>
public partial class FluentDatePicker : FluentCalendarBase
{
    private bool _popupOpenedByKeyboard;
    private FluentCalendar _calendar = default!;
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
                   && string.IsNullOrEmpty(CurrentValueAsString);
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
    public DatePickerRenderingMode RenderingMode { get; set; } = DatePickerRenderingMode.FluentUI;

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
    public DateTime? DoubleClickToDate { get; set; }

    /// <summary>
    /// Gets or sets the template used to render each day in the calendar.
    /// </summary>
    /// <remarks>Use this parameter to customize the appearance and content of individual days. The template
    /// receives a <see cref="FluentCalendarDay"/> parameter representing the day to render.
    /// </remarks>
    [Parameter]
    public RenderFragment<FluentCalendarDay>? DaysTemplate { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the selected month in the picker changes.
    /// </summary>
    [Parameter]
    public EventCallback<DateTime> PickerMonthChanged { get; set; }

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
        if (IsFluentUIMode && e.Detail == 1 && !ReadOnly)
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
            if (DoubleClickToDate.HasValue)
            {
                await OnSelectedDateAsync(DoubleClickToDate.Value);
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
    protected async Task OnSelectedDateAsync(DateTime? value)
    {
        var updatedValue = value;

        if (Value is not null && value is not null)
        {
            updatedValue = Value?.TimeOfDay != TimeSpan.Zero
            ? value?.Date + Value?.TimeOfDay
            : value;
        }

        Opened = false;

        if (IsFluentUIMode && _popupOpenedByKeyboard)
        {
            await _icon.Element.FocusAsync();
            _popupOpenedByKeyboard = false;
        }

        await OnSelectedDateHandlerAsync(updatedValue);
    }

    /// <summary />
    protected override string? FormatValueAsString(DateTime? value)
    {
        // FluentUI mode
        if (IsFluentUIMode)
        {
            return View switch
            {
                CalendarViews.Years => value?.ToString("yyyy", Culture),
                CalendarViews.Months => value?.ToString(Culture.DateTimeFormat.YearMonthPattern, Culture),
                _ => value?.ToString(Culture.DateTimeFormat.ShortDatePattern, Culture),
            };
        }

        // Browser mode
        return View switch
        {
            CalendarViews.Years => value?.ToString("yyyy", CultureInfo.InvariantCulture),
            CalendarViews.Months => value?.ToString("yyyy-MM", CultureInfo.InvariantCulture),
            _ => value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        };
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (View == CalendarViews.Years && int.TryParse(value, Culture, out var year))
        {
            value = new DateTime(year, 1, 1).ToString(Culture.DateTimeFormat.ShortDatePattern, Culture);
        }

        BindConverter.TryConvertTo(value, Culture, out result);

        validationErrorMessage = null;
        return true;
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
    /// Gets a value indicating whether the date picker is using the Fluent UI mode.
    /// </summary>
    private bool IsFluentUIMode => RenderingMode == DatePickerRenderingMode.FluentUI;

    /// <summary />
    internal string? GetInputType() => IsFluentUIMode ? null : View switch
    {
        CalendarViews.Days => "date",
        CalendarViews.Months => "month",
        CalendarViews.Years => "number",
        _ => null
    };

    /// <summary />
    internal TextInputMode? GetInputMode() => IsFluentUIMode ? null : View switch
    {
        CalendarViews.Years => TextInputMode.Numeric,
        _ => null
    };
}
