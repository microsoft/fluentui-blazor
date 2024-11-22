// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Localizer that uses the FluentUI language resource to localize strings.
/// </summary>
public class FluentLocalizer : IFluentLocalizer
{
    /// <summary>
    /// Returns the localized string for the given key.
    /// By default, the English (InvariantCulture) resource is used.
    /// </summary>
    public virtual string? this[string key, params object[] arguments]
    {
        get
        {
            var resourceString = Localization.LanguageResource.ResourceManager.GetString(key, System.Globalization.CultureInfo.InvariantCulture);
            return resourceString;
        }
    }
}
