namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Values to define available focus restore roles.
/// </summary>
public enum FocusRestoreRole
{
    /// <summary>
    /// When focus is lost by element with this role, it is restored to the most recent element with <see cref="Target"/> role.
    /// </summary>
    Source,

    /// <summary>
    /// When focus is lost by element with <see cref="Source" /> role, it is restored to the most recent element with this role.
    /// </summary>
    Target
}