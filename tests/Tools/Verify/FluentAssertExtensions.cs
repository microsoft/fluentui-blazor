// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Verify;

/// <summary />
public static class FluentAssertExtensions
{
    /// <summary>
    /// Removes randomized Blazor attributes from the content.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string ScrubBlazorAttributes(this string content)
    {
        return new FluentAssertOptions().ScrubLinesWithReplace(content);
    }
}
