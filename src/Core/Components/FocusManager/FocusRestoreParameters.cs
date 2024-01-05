namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Focus restore parameters that can be applied to element using <see cref="IFocusManager"/>.
/// </summary>
public class FocusRestoreParameters
{
    /// <summary>
    /// Gets or sets role of element in focus restore. See <see cref="FocusRestoreRole"/>.
    /// </summary>
    public FocusRestoreRole Role { get; set; }
}
