// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

// ToDo: remove pragma after next PR
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
/// <summary>
/// The option for generating a header for the <see cref="FluentDataGrid{TGridItem}" />.
/// </summary>
public enum DataGridGeneratedHeaderType
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
    Sticky,
}
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
