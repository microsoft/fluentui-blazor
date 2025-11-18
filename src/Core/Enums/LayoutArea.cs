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
    Header,

    /// <summary />
    Footer,

    /// <summary />
    [Description("nav")]
    Navigation,

    /// <summary />
    Content,

    /// <summary />
    Aside,
}
