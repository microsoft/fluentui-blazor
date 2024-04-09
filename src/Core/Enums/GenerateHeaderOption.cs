namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The option for generating a header for the <see cref="FluentDataGrid{TGridItem}" />.
/// </summary>
public enum GenerateHeaderOption
{
    /// <summary>
    /// No header row.
    /// </summary>
    None,

    /// <summary>
    /// Generate a header row.
    /// </summary>
    Default,

    /// <summary>
    /// Generate a sticky header row.
    /// </summary>
    Sticky
}
