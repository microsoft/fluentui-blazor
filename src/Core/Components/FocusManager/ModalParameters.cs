namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Parameters that help to create modal dialog like experience. Applied to dialog content element using <see cref="IFocusManager"/>.
/// </summary>
public class ModalParameters
{
    /// <summary>
    /// Gets or sets modal group name.
    /// Elements with same modal group name could be accessible when one of them is activated.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Indicates if element is always reachable in Tab order.
    /// </summary>
    public bool AlwaysAccessible { get; set; }

    /// <summary>
    /// Traps focus inside the target element - it forbids users to tab out of the focus trap into the actual browser.
    /// </summary>
    public bool TrapFocus { get; set; }
}
