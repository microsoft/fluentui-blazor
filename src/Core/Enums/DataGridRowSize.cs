namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The height of each <see cref="FluentDataGridRow{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// Values are in pixels.
/// </summary>
public enum DataGridRowSize
{

    /// <summary>
    /// Medium row height (default)
    /// </summary>
    Medium = 44,

    /// <summary>
    /// Small row height
    /// </summary>
    Small = 32,

    /// <summary>
    /// Smaller row height
    /// </summary>
    Smaller = 24,

    /// <summary>
    /// Large row height
    /// </summary>
    Large = 58,

    /// <summary>
    /// Dynamic row height
    /// </summary>
    Dynamic
}
