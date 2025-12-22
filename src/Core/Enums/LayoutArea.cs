// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// </summary>
public enum LayoutArea
{
    /// <summary />
    [Description("header")]
    Header,

    /// <summary />
    [Description("footer")]
    Footer,

    /// <summary />
    [Description("nav")]
    Navigation,

    /// <summary />
    [Description("content")]
    Content,

    /// <summary />
    [Description("aside")]
    Aside,
}
