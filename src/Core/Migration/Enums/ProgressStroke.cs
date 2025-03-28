// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// List of stroke sizes for the progress component or the spinner component.
/// </summary>
public enum ProgressStroke
{
    /// <summary>
    /// Small stroke size.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ProgressThickness.Medium or SpinnerSize.Small value instead.")]
    Small,

    /// <summary>
    /// Normal stroke size.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ProgressThickness.Medium or SpinnerSize.Medium value instead.")]
    Normal,

    /// <summary>
    /// Large stroke size.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ProgressThickness.Large or SpinnerSize.Large value instead.")]
    Large,
}
