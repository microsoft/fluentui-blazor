// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the available patterns for displaying skeleton loading placeholders.
/// </summary>
public enum SkeletonPattern
{
    /// <summary>
    /// Represents a circular icon that can be displayed in the user interface.
    /// </summary>
    Icon,

    /// <summary>
    /// Represents a combination of a circular icon and a title, on the same line.
    /// </summary>
    IconTitle,

    /// <summary>
    /// Represents a combination of a circular icon and a title, on the same line.
    /// And a content area below the title.
    /// </summary>
    IconTitleContent,
}
