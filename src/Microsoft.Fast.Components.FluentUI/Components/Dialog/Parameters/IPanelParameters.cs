namespace Microsoft.Fast.Components.FluentUI
{
    public interface IPanelParameters : IDialogParameters
    {
        HorizontalAlignment Alignment { get; set; }
        bool Modal { get; set; }
        bool ShowDismiss { get; set; }
        string PrimaryButton { get; set; }
        string SecondaryButton { get; set; }
        string? Width { set; get; }
    }
}