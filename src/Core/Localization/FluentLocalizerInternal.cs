// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Localizer that uses the FluentUI language resource to localize strings.
/// </summary>
internal class FluentLocalizerInternal : IFluentLocalizer
{

    /// <summary>
    /// Gets the default IFluentLocalizer.
    /// </summary>
    public static readonly IFluentLocalizer Default = new FluentLocalizerInternal();

    /// <summary>
    /// Gets the string resource with the given name.
    /// </summary>
    /// <param name="key">The name of the string resource.</param>
    /// <param name="arguments">The list of arguments to be passed to the string resource.</param>
    /// <returns>The string resource.</returns>
    public virtual string this[string key, params object[] arguments]
    {
        get
        {
            return IFluentLocalizer.GetDefault(key, arguments);
        }
    }
}
