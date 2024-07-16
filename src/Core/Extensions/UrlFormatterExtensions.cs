namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

internal static class UrlFormatterExtensions
{
    internal static string FormatCollocatedUrl(this string url, LibraryConfiguration configuration)
    {
        if (configuration.FormatCollocatedJavaScriptUrl == null)
        {
            return url;
        }

        return configuration.FormatCollocatedJavaScriptUrl(url);
    }

}
