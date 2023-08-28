namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// An <see cref="EventArgs"/> class that is used as arguments
/// whenever a user attempts to execute the default action on a <see cref="FluentNavMenuItemBase"/>.
/// </summary>
public class NavMenuActionArgs : EventArgs
{
    public bool Handled { get; private set; }
    public FluentNavMenuItemBase Target { get; }

    public NavMenuActionArgs(FluentNavMenuItemBase target)
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    public void SetHandled()
    {
        Handled = true;
    }
}
