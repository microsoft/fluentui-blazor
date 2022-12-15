using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

partial class NumberRangePicker
{

    #region Fields

    private double fromValue { get; set; }

    private double toValue { get; set; }

    #endregion


    #region Properties

    [Parameter]
	public Nullable<double> FromNumber { get; set; }

    [Parameter]
    public Nullable<double> ToNumber { get; set; }

    [Parameter]
    public EventCallback<(Nullable<double> FromNumber, Nullable<double> ToNumber)> RangeSelected { get; set; }

    #endregion

    private void DataEntered()
    {
        if (!double.IsNaN(fromValue))
            FromNumber = fromValue;
        if (!double.IsNaN(toValue))
            ToNumber = toValue;

        RangeSelected.InvokeAsync((FromNumber, ToNumber));
    }

    protected override void OnParametersSet()
    {
        if (FromNumber.HasValue)
            fromValue = FromNumber.Value;
        else
            fromValue = double.NaN;
        if (ToNumber.HasValue)
            toValue = ToNumber.Value;
        else
            toValue = double.NaN;
        base.OnParametersSet();
    }

}
