namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Instance specific parameters for a <see cref="FluentDialog"/> component
/// </summary>
public class DialogSettings
{

    /// <summary>
    /// Label of the primary (OK) button. If <value>null</value> button will not be shown.
    /// </summary>
    public string? PrimaryButton { get; set; } = "Ok"; //FluentPanelResources.ButtonOK;

    /// <summary>
    /// Label of the secondary (Cancel) button. If <value>null</value> button will not be shown.
    /// </summary>
    public string? SecondaryButton { get; set; } = "Cancel"; //FluentPanelResources.ButtonCancel;





}
