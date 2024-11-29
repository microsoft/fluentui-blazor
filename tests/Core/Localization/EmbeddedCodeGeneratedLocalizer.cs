// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Localization;

/// <summary>
/// This Localizer uses the embedded resources configured using the 'ResXFileCodeGenerator' tool.
/// If you are NOT using the 'ResXFileCodeGenerator' tool, you should use the <see cref="EmbeddedLocalizer"/> instead.
/// </summary>
public class EmbeddedCodeGeneratedLocalizer : IFluentLocalizer
{
    /// <summary>
    /// Gets the string resource with the given key, depending of the current UI culture.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public string this[string key, params object[] arguments]
    {
        get
        {
            // Need to add
            //  - builder.Services.AddLocalization();
            //  - app.UseRequestLocalization(new RequestLocalizationOptions().AddSupportedUICultures(["en", "fr", "nl"]));

            // Gets the localized version of the string
            var localizedString = Resources.FluentLocalizer.ResourceManager.GetString(key, CultureInfo.CurrentCulture);

            // Fallback to the Default/English if no translation is found
            return localizedString == null
                ? IFluentLocalizer.GetDefault(key, arguments)
                : string.Format(CultureInfo.CurrentCulture, localizedString, arguments);
        }
    }
}
