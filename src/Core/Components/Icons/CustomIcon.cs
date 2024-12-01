// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Custom icon loaded from <see cref="IconsExtensions.GetInstance(IconInfo)"/>
/// </summary>
public class CustomIcon : Icon
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomIcon"/> class.
    /// </summary>
    public CustomIcon()
        : base(string.Empty, IconVariant.Regular, IconSize.Size24, string.Empty)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomIcon"/> class.
    /// </summary>
    /// <param name="icon"></param>
    public CustomIcon(Icon icon)
        : base(icon.Name, icon.Variant, icon.Size, icon.Content)
    { }
}
