// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Localizer that uses the FluentUI language resource to localize strings.
/// </summary>
public class FluentLocalizer
{
    /// <summary />
    public virtual string this[string key, params object[] arguments]
    {
        get
        {
            return GetDefault(key, arguments);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    protected string GetDefault(string key, params object[] arguments)
    {
        return Localization.LanguageResource.ResourceManager.GetString(key, System.Globalization.CultureInfo.InvariantCulture)
            ?? throw new ArgumentException($"Key '{key}' not found in LanguageResource.", paramName: nameof(key));
    }
}
