namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The set of parameters for a panel.
/// </summary>
public class PanelParameters : DialogParameters, IPanelParameters
{
    /// <summary>
    /// Determines the alignment of the panel.
    /// </summary>
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Right;

    /// <summary>
    /// Determines if the panel is modal. Defaults to true.
    /// When true, clicking outside the panel will dismiss the panel.
    /// </summary>
    public bool Modal { get; set; } = true;

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Text to display on the primary button.
    /// </summary>
    public string? PrimaryButton { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// Text to display on the secondary button.
    /// </summary>
    public string? SecondaryButton { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    /// <summary>
    /// Width of the panel.
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Data to pass to and from the panel.
    /// </summary>
    public object? Data { get; set; }
}
