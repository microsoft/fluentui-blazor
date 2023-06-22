namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The set of parameters for a panel.
/// </summary>
public class PanelParameters<TData> : DialogParameters<TData>
{
    public override HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Right;
}
