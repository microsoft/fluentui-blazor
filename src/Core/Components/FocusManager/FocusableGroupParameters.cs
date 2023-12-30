namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Focusable group parameters that can be applied to element using <see cref="IFocusManager"/>.
/// </summary>
public sealed class FocusableGroupParameters
{
    /// <summary>
    /// Gets or sets Tab behavior for the group. See <see cref="FocusableGroupMode"/>
    /// </summary>
    public FocusableGroupMode? Mode { get; set; }
}
