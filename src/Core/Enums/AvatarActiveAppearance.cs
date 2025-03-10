// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the styled appearance of the avatar when active is set to Active.
/// </summary>
public enum AvatarActiveAppearance
{
    /// <summary>
    /// Default value, the avatar will be decorated with a ring.
    /// </summary>
    [Description("ring")]
    Ring,

    /// <summary>
    /// The avatar will be decorated with a shadow.
    /// </summary>
    [Description("shadow")]
    Shadow,

    /// <summary>
    /// The avatar will be decorated with a ring and shadow.
    /// </summary>
    [Description("ring-shadow")]
    RingShadow,

}
