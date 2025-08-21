// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a date picker control that enables users to select a date using a fluent user interface.
/// </summary>
public partial class FluentDatePicker
{
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
    /// Gets or sets a value indicating whether the date picker is currently open.
    /// </summary>
    public bool Opened { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the calendar is opened or closed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnCalendarOpen { get; set; }

    /// <summary />
    protected async Task OnCalendarOpenHandlerAsync(MouseEventArgs e)
    {
        if (!ReadOnly)
        {
            Opened = !Opened;

            if (OnCalendarOpen.HasDelegate)
            {
                await OnCalendarOpen.InvokeAsync(Opened);
            }
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
        await OnSelectedDateHandlerAsync(updatedValue);
    }

    /// <summary />
    protected async Task OnDoubleClickHandlerAsync(MouseEventArgs e)
    {
        if (!ReadOnly)
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
    private string PlaceholderAccordingToView()
    {
        return View switch
        {
            CalendarViews.Years => "yyyy",
            CalendarViews.Months => Culture.DateTimeFormat.YearMonthPattern,
            _ => Culture.DateTimeFormat.ShortDatePattern
        };
    }
}
