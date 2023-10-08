using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDatePicker : FluentCalendarBase
{
    public static string CalendarIcon = "<svg slot=\"end\" width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"var(--neutral-fill-strong-focus)\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M17.75 3C19.55 3 21 4.46 21 6.25v11.5c0 1.8-1.46 3.25-3.25 3.25H6.25A3.25 3.25 0 013 17.75V6.25C3 4.45 4.46 3 6.25 3h11.5zm1.75 5.5h-15v9.25c0 .97.78 1.75 1.75 1.75h11.5c.97 0 1.75-.78 1.75-1.75V8.5zm-11.75 6a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm4.25 0a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm-4.25-4a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm4.25 0a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm4.25 0a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5zm1.5-6H6.25c-.97 0-1.75.78-1.75 1.75V7h15v-.75c0-.97-.78-1.75-1.75-1.75z\"/>";

    /// <summary />
    public FluentDatePicker()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).AddClass("fluent-datepicker").Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Text displayed just above the component
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Content displayed just above the component
    /// </summary>
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Text used on aria-label attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The name of the element.Allows access by name from the associated form.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Whether the element needs to have a value
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Determines if the element should receive document focus on page load.
    /// </summary>
    [Parameter]
    public virtual bool Autofocus { get; set; } = false;

    /// <summary>
    /// The short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public virtual string? Placeholder { get; set; }

    [Parameter]
    public EventCallback<bool> OnCalendarOpen { get; set; }

    public bool Opened { get; set; } = false;

    private string? DateAsString
    {
        get
        {
            return Value?.ToString(Culture.DateTimeFormat.ShortDatePattern);
        }

        set
        {
            var datePattern = Culture.DateTimeFormat.ShortDatePattern;
            var isValid = DateTime.TryParseExact(
                Convert.ToString(value),
                datePattern,
                Culture,
                DateTimeStyles.None,
                out var newDate);

            if (isValid)
            {
                Value = newDate + (Value?.TimeOfDay ?? TimeSpan.Zero);
            }
            else
            {
                Value = null;
            }
        }
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

    protected Task OnSelectedDateAsync(DateTime? value)
    {
        Opened = false;

        if (Value != null && Value?.TimeOfDay != TimeSpan.Zero)
        {
            DateTime currentValue = value ?? DateTime.MinValue;
            Value = currentValue.Date + Value?.TimeOfDay;
        }
        else
        {
            Value = value;
        }

        return Task.CompletedTask;
    }
}
