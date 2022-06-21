using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentCalendar : FluentComponentBase
{
    private string? disabledDates = null;
    private string? selectedDates = null;

    /// <summary>
    /// Gets or sets if the calendar is readonly 
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }


    /// <summary>
    /// Gets or sets the month of the calendar to display
    /// </summary>
    [Parameter]
    public int Month { get; set; } = DateTime.Now.Month;

    /// <summary>
    /// Gets or sets the year of the calendar to display
    /// </summary>
    [Parameter]
    public int Year { get; set; } = DateTime.Now.Year;

    /// <summary>
    /// Locale information which can include market (language and country), calendar type and numbering type.
    /// </summary>
    [Parameter]
    public string? Locale { get; set; }

    /// <summary>
    ///  Gets or sets formatting for the numbered days.
    /// </summary>
    [Parameter]
    public DayFormat? DayFormat { get; set; } = FluentUI.DayFormat.Numeric;

    /// <summary>
    /// Gets or sets formatting for the weekday titles.
    /// </summary>
    [Parameter]
    public WeekdayFormat? WeekdayFormat { get; set; } = FluentUI.WeekdayFormat.Short;

    /// <summary>
    /// Gets or sets formatting for the month name in the title.
    /// </summary>
    [Parameter]
    public MonthFormat? MonthFormat { get; set; } = FluentUI.MonthFormat.Long;

    /// <summary>
    /// Gets or sets formatting for the year in the title.
    /// </summary>
    [Parameter]
    public YearFormat? YearFormat { get; set; } = FluentUI.YearFormat.Numeric;

    /// <summary>
    /// Gets or sets the minimum number of weeks to show. This allows for normalizing of calendars so that they take up the same vertical space.
    /// </summary>
    [Parameter]
    public int MinWeeks { get; set; } = 4;


    /// <summary>
    /// Gets or sets dates to be shown as disabled.
    /// </summary>
    [Parameter]
    public IEnumerable<DateOnly>? DisabledDates
    {
        get
        {
            return disabledDates!.Split(",").Select(x =>
            {
                _ = DateOnly.TryParse(x, out DateOnly d);
                return d;
            });
        }
        set
        {
            disabledDates = string.Join(",", value!.Select(x => x.ToString("M-d-yyyy")));
        }
    }

    /// <summary>
    /// Gets or sets dates to be shown as selected
    /// </summary>
    [Parameter]
    public IEnumerable<DateOnly>? SelectedDates
    {
        get
        {
            return selectedDates!.Split(",").Select(x =>
            {
                _ = DateOnly.TryParse(x, out DateOnly d);
                return d;
            });
        }
        set
        {
            selectedDates = string.Join(",", value!.Select(x => x.ToString("M-d-yyyy")));
        }
    }
}