// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;
internal sealed class InternalAppBarContext(FluentAppBar appBar)
{
    public readonly Dictionary<string, IAppBarItem> Apps = [];
    public FluentAppBar AppBar { get; } = appBar;

    internal void Register(IAppBarItem app)
    {
        ArgumentNullException.ThrowIfNull(app.Id);

        Apps.Add(app.Id, app);
    }

    internal void Unregister(IAppBarItem app)
    {
        ArgumentNullException.ThrowIfNull(app.Id);

        if (Apps.Count > 0)
        {
            Apps.Remove(app.Id);
        }
    }
}
