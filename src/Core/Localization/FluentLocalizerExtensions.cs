// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Extensions Localizer that uses the FluentUI language resource to localize strings.
/// </summary>
public static class FluentLocalizerExtensions
{
    /// <summary>
    /// Gets the default string resource (English) with the given name.
    /// </summary>
    /// <param name="localizer">The localizer.</param>
    /// <param name="key">The name of the string resource.</param>
    /// <param name="arguments">The list of arguments to be passed to the string resource.</param>
    /// <returns>The string resource.</returns>
    public static string GetDefault(this IFluentLocalizer localizer, string key, params object[] arguments)
    {
        return IFluentLocalizer.GetDefault(key, arguments);
    }
}
