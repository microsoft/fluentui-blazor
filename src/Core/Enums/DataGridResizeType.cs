namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The type of <see cref="FluentDataGridRow{TGridItem}"/> in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public enum DataGridResizeType
{
    /// <summary>
    /// Resize datagrid columns by discreet steps of 10 pixels
    /// </summary>
    Discrete,

    /// <summary>
    /// Resize datagrid columns by exact pixel values
    /// </summary>
    Exact,
}
