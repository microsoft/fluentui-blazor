// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A spinner alerts a user that content is being loaded or processed and they should wait for the activity to complete.
/// This component is renamed to <see cref="FluentSpinner"/> and will be removed in a future release.
/// </summary>
[Obsolete("This component is renamed to FluentSpinner and will be removed in a future release.")]
public class FluentProgressRing : FluentSpinner
{
    /// <summary />
    public FluentProgressRing(LibraryConfiguration configuration) : base(configuration) { }
}
