// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the shape of the avatar.
/// </summary>
public enum AvatarShape
{
    /// <summary>
    /// The avatar is circular.
    /// </summary>
    [Description("circle")]
    Circle,
    /// <summary>
    /// The avatar is square.
    /// </summary>
    [Description("square")]
    Square,
}
