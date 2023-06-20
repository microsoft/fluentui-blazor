namespace Microsoft.Fast.Components.FluentUI;

public class PanelParameters : DialogParameters, IPanelParameters
{
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Right;
    public bool Modal { get; set; } = true;
    public bool ShowDismiss { get; set; } = true;
    public string? PrimaryButton { get; set; } = "Ok"; //DialogResources.ButtonPrimary;
    public string? SecondaryButton { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;
    public string? Width { get; set; }
}
