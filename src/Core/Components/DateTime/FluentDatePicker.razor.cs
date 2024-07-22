using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDatePicker : FluentCalendarBase
{
    public static string CalendarIcon = "<svg slot=\"end\" width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M17.75 3C19.55 3 21 4.46 21 6.25v11.5c0 1.8-1.46 3.25-3.25 3.25H6.25A3.25 3.25 0 013 17.75V6.25C3 4.45 4.46 3 6.25 3h11.5zm1.75 5.5h-15v9.25c0 .97.78 1.75 1.75 1.75h11.5c.97 0 1.75-.78 1.75-1.75V8.5zm-11.75 6a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm4.25 0a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm-4.25-4a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm4.25 0a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm4.25 0a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm1.5-6H6.25c-.97 0-1.75.78-1.75 1.75V7h15v-.75c0-.97-.78-1.75-1.75-1.75z\"/>";

    /// <summary />
    public FluentDatePicker()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override string? ClassValue
    {
        get
        {
            return new CssBuilder(base.ClassValue)
                .AddClass("fluent-datepicker")
                .Build();
        }
    }

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// raised when calendar popup opened
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnCalendarOpen { get; set; }

    /// <summary>
    /// Defines the appearance of a Day cell.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentCalendarDay>? DaysTemplate { get; set; }

    /// <summary>
    /// Fired when the display month changes.
    /// </summary>
    [Parameter]
    public virtual EventCallback<DateTime> PickerMonthChanged { get; set; }

    public bool Opened { get; set; } = false;

    protected override string? FormatValueAsString(DateTime? value)
    {
        return Value?.ToString(View switch
        {
            CalendarViews.Years => "yyyy",
            CalendarViews.Months => Culture.DateTimeFormat.YearMonthPattern,
            _ => Culture.DateTimeFormat.ShortDatePattern
        }, Culture);
    }

    protected Task OnCalendarOpenHandlerAsync(MouseEventArgs e)
    {
        if (!ReadOnly)
        {
            Opened = !Opened;

            if (OnCalendarOpen.HasDelegate)
            {
                return OnCalendarOpen.InvokeAsync(Opened);
            }
        }

        return Task.CompletedTask;
    }

    protected async Task OnSelectedDateAsync(DateTime? value)
    {
        DateTime? updatedValue = value;

        if (Value is not null && value is not null)
        {
            updatedValue = Value?.TimeOfDay != TimeSpan.Zero
            ? value?.Date + Value?.TimeOfDay
            : value;
        }
        Opened = false;
        await OnSelectedDateHandlerAsync(updatedValue);
    }

    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (View == CalendarViews.Years && int.TryParse(value, out var year))
        {
            value = new DateTime(year, 1, 1).ToString(Culture.DateTimeFormat.ShortDatePattern);
        }

        BindConverter.TryConvertTo(value, Culture, out result);

        validationErrorMessage = null;
        return true;
    }

    private string PlaceholderAccordingToView()
        => View switch
        {
            CalendarViews.Years => "yyyy",
            CalendarViews.Months => Culture.DateTimeFormat.YearMonthPattern,
            _ => Culture.DateTimeFormat.ShortDatePattern
        };
}
