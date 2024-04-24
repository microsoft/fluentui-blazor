namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// An <see cref="EventArgs"/> class that is used as arguments
/// whenever a user attempts to execute the default action on a <see cref="FluentNavMenuItemBase"/>.
/// </summary>
public class NavMenuActionArgs : EventArgs
{
    public bool Handled { get; private set; }
    public FluentNavMenuItemBase Target { get; }
    public bool ReNavigate { get; set; }

    public NavMenuActionArgs(FluentNavMenuItemBase target)
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }
    public NavMenuActionArgs(FluentNavMenuItemBase target, bool renavigate)
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
        ReNavigate = renavigate;
    }

    public void SetHandled()
    {
        Handled = true;
    }
}
