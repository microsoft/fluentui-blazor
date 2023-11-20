using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

// See https://react.fluentui.dev/?path=/docs/compat-components-datepicker--default
public partial class FluentMonth : FluentComponentBase
{
    public static string ArrowUp = FluentCalendar.ArrowUp;
    public static string ArrowDown = FluentCalendar.ArrowDown;

    private DateTime _selectedValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    private CalendarExtended? _calendarExtended = null;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).AddClass("fluent-month").Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets if the calendar is readonly 
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; } = false;

    /// <summary>
    /// The culture of the component.
    /// By default <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Selected date (two-way bindable).
    /// </summary>
    [Parameter]
    public virtual DateTime Value
    {
        get
        {
            return FirstDayOfMonth(_selectedValue);
        }

        set
        {
            var month = FirstDayOfMonth(value);

            if (month == _selectedValue)
            {
                return;
            }

            _selectedValue = month;
            ValueChanged.InvokeAsync(month);
        }
    }

    /// <summary>
    /// Fired when the display month changes.
    /// </summary>
    [Parameter]
    public virtual EventCallback<DateTime> ValueChanged { get; set; }

    /// <summary>
    /// All days of this current month.
    /// </summary>
    internal CalendarExtended CalendarExtended => _calendarExtended ?? new CalendarExtended(this.Culture, this.Value);

    private DateTime FirstDayOfMonth(DateTime date)
    {
        return date.Day == 1 ? date : new DateTime(date.Year, date.Month, 1);
    }

}
