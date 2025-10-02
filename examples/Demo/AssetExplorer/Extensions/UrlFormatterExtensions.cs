// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Extensions;

internal static class UrlFormatterExtensions
{
    internal static string FormatCollocatedUrl(this string url, LibraryConfiguration configuration)
    {
        if (configuration.CollocatedJavaScriptQueryString == null)
        {
            return url;
        }

        var queryString = configuration.CollocatedJavaScriptQueryString(url);

        return string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
    }

}
