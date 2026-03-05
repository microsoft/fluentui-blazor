// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The position of the toast on the screen.
/// </summary>
public enum ToastPosition
{
    /// <summary />
    [Description("top-right")]
    TopRight,

    /// <summary />
    [Description("top-left")]
    TopLeft,

    /// <summary />
    [Description("top-center")]
    TopCenter,

    /// <summary />
    [Description("bottom-right")]
    BottomRight,

    /// <summary />
    [Description("bottom-left")]
    BottomLeft,

    /// <summary />
    [Description("bottom-center")]
    BottomCenter,
}
