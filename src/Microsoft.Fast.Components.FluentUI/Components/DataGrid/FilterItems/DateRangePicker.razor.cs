using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

partial class DateRangePicker
{

    #region Properties

    [Parameter]
	public Nullable<DateTime> FromDate { get; set; }

    [Parameter]
    public Nullable<DateTime> ToDate { get; set; }

    [Parameter]
    public EventCallback<(Nullable<DateTime> FromDate, Nullable<DateTime> ToDate)> RangeSelected { get; set; }

    #endregion

    private void DataEntered()
    {
        RangeSelected.InvokeAsync((FromDate, ToDate));
    }

}
