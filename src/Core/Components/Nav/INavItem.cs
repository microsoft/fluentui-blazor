// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Marker interface for components that can be direct children of <see cref="FluentNav"/>.
/// </summary>
/// <remarks>
/// Implement this interface to indicate that a component is allowed to be rendered as a direct child
/// of <see cref="FluentNav"/>. Only components implementing this interface should be used as children
/// of the navigation drawer to ensure proper structure and styling.
/// </remarks>
public interface INavItem
{
}
