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
        if (app == null || app.Id == null)
        {
            return;
        }

        Apps.TryAdd(app.Id, app);
    }

    internal void Unregister(IAppBarItem app)
    {
        if (app == null || app.Id == null)
        {
            return;
        }

        if (Apps.TryGetValue(app.Id, out var registered) && registered == app)
        {
            Apps.Remove(app.Id);
        }
    }
}
