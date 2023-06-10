using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDatePicker : FluentCalendarBase
{
    private const string CalendarIcon = "<svg slot=\"end\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"red\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M21 17.75c0 1.8-1.46 3.25-3.25 3.25H6.25A3.25 3.25 0 013 17.75V6.25C3 4.45 4.46 3 6.25 3h11.5C19.55 3 21 4.46 21 6.25v11.5zm-1.5 0V6.25c0-.97-.78-1.75-1.75-1.75H6.25c-.97 0-1.75.78-1.75 1.75v11.5c0 .97.78 1.75 1.75 1.75h11.5c.97 0 1.75-.78 1.75-1.75zm-2.5-10c0 .38-.28.7-.65.74l-.1.01h-8.5a.75.75 0 01-.1-1.5h8.6c.41 0 .75.34.75.75zm0 8.5c0 .38-.28.7-.65.74l-.1.01h-8.5a.75.75 0 01-.1-1.5h8.6c.41 0 .75.34.75.75zM17 12c0 .38-.28.7-.65.74l-.1.01h-8.5a.75.75 0 01-.1-1.5h8.6c.41 0 .75.34.75.75z\"/>";

    /// <summary />
    public FluentDatePicker()
    {
        Placeholder = Culture.DateTimeFormat.ShortDatePattern;
    }

    /// <summary />
    private string AnchorId { get; } = Identifier.NewId();

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).AddClass("fluent-datepicker").Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder().AddStyle(Style).Build();

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual Appearance Appearance { get; set; } = Appearance.Outline;

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
            return SelectedDate?.ToString(Culture.DateTimeFormat.ShortDatePattern);
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
                SelectedDate = newDate;
            }
            else
            {
                SelectedDate = null;
            }
        }
    }

    protected Task OnCalendarOpenHandlerAsync(MouseEventArgs e)
    {
        Opened = !Opened;

        if (OnCalendarOpen.HasDelegate)
        {
            return OnCalendarOpen.InvokeAsync(Opened);
        }

        return Task.CompletedTask;
    }

    protected Task OnSelectedDateAsync(DateTime? value)
    {
        Opened = false;
        SelectedDate = value;

        return Task.CompletedTask;
    }
}
