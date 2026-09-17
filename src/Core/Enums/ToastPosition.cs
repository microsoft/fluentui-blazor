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
    [Description("top-end")]
    TopEnd,

    /// <summary />
    [Description("top-start")]
    TopStart,

    /// <summary />
    [Description("top-center")]
    TopCenter,

    /// <summary />
    [Description("bottom-end")]
    BottomEnd,

    /// <summary />
    [Description("bottom-start")]
    BottomStart,

    /// <summary />
    [Description("bottom-center")]
    BottomCenter,
}
