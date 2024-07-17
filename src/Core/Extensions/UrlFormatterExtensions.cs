namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

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
