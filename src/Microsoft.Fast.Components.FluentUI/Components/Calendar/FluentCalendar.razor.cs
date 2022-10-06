using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;



public partial class FluentCalendar : FluentComponentBase
{
    private string? disabledDatesAsString = null;
    private string? selectedDatesAsString = null;

    private List<DateOnly> _selectedDates = new();

    private IEnumerable<DateOnly>? InternalDisabledDates
    {
        get
        {
            if (string.IsNullOrEmpty(disabledDatesAsString))
                return null;

            return disabledDatesAsString.Split(",").Select(x =>
            {
                _ = DateOnly.TryParse(x, out DateOnly d);
                return d;
            });
        }
        set
        {
            disabledDatesAsString = string.Join(",", value!.OrderBy(d => d.DayNumber).Select(x => x.ToString("M-d-yyyy")));
        }
    }

    private List<DateOnly> InternalSelectedDates
    {
        get => _selectedDates;
        set
        {
            _selectedDates = value.OrderBy(d => d.DayNumber).ToList();
            selectedDatesAsString = string.Join(",", _selectedDates.Select(x => x.ToString("M-d-yyyy")));
        }
    }

    /// <summary>
    /// Gets or sets if the calendar is readonly 
    /// </summary>
    [Parameter]
    public bool Readonly { get; set; } = false;

    /// <summary>
    /// String repesentation of the full locale including market, calendar type and numbering system
    /// </summary>
    [Parameter]
    public string? Locale { get; set; }

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
    /// Format style for the day
    /// </summary>
    [Parameter]
    public DayFormat? DayFormat { get; set; } = FluentUI.DayFormat.Numeric;

    /// <summary>
    /// Format style for the week day labels
    /// </summary>
    [Parameter]
    public WeekdayFormat? WeekdayFormat { get; set; } = FluentUI.WeekdayFormat.Short;

    /// <summary>
    /// Format style for the month label
    /// </summary>
    [Parameter]
    public MonthFormat? MonthFormat { get; set; } = FluentUI.MonthFormat.Long;

    /// <summary>
    /// Format style for the year used in the title
    /// </summary>
    [Parameter]
    public YearFormat? YearFormat { get; set; } = FluentUI.YearFormat.Numeric;

    /// <summary>
    /// Minimum number of weeks to show for the month
    /// This can be used to normalize the calendar view
    /// when changing or across multiple calendars
    /// </summary>
    [Parameter]
    public int MinWeeks { get; set; } = 4;

    /// <summary>
    /// A list of dates that should be shown as disabled
    /// </summary>
    [Parameter]
    public bool DisabledSelectable { get; set; } = true;

    /// <summary>
    /// Gets or sets whether dates outside of shown month are selectable
    /// </summary>
    [Parameter]
    public bool OutOfMonthSelectable { get; set; } = true;

    /// <summary>
    /// Gets or sets dates to be shown as disabled.
    /// </summary>
    [Parameter]
    public IEnumerable<DateOnly>? DisabledDates { get; set; }

    /// <summary>
    /// A list of dates that should be shown as highlighted
    /// </summary>
    [Parameter]
    public List<DateOnly> SelectedDates { get; set; } = new();

    [Parameter]
    public EventCallback<List<DateOnly>> SelectedDatesChanged { get; set; }

    [Parameter]
    public EventCallback<DateOnly> OnDateClicked { get; set; }

    protected override void OnParametersSet()
    {
        if (InternalSelectedDates != SelectedDates)
        {
            InternalSelectedDates = SelectedDates;
        }
        if (InternalDisabledDates != DisabledDates)
        {
            InternalDisabledDates = DisabledDates;
        }
    }

    private async Task OnDateSelected(CalendarSelectEventArgs args)
    {
        CalendarDateInfo di = args.CalendarDateInfo;

        DateOnly date = DateOnly.FromDateTime(new(di.Year, di.Month, di.Day));

        if (!SelectedDates.Contains(date) && (!di.Disabled || DisabledSelectable) && (di.Month == Month || OutOfMonthSelectable))
        {
            SelectedDates.Add(date);
        }

        if (di.Selected && SelectedDates.Contains(date))
        {
            SelectedDates.Remove(date);
        }

        await OnDateClicked.InvokeAsync(date);
        await SelectedDatesChanged.InvokeAsync(SelectedDates);

    }
}