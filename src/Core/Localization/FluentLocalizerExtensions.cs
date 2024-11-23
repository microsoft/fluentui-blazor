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
    /// 
    /// </summary>
    /// <param name="localizer"></param>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GetDefault(this IFluentLocalizer localizer, string key, params object[] arguments)
    {
        return IFluentLocalizer.GetDefault(key, arguments);
    }
}
