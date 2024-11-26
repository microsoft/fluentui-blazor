// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Localization;

public class EmbeddedLocalizer : IFluentLocalizer
{
    private const string BaseName = "Microsoft.FluentUI.AspNetCore.Components.Tests.Localization.Resources.FluentLocalizer";   // .resx files
    private static readonly ResourceManager ResourceManager = new ResourceManager(BaseName, Assembly.GetExecutingAssembly());

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

            // Returns the localized version of the string
            // By default, returns the English version of the string
            var localizedString = ResourceManager.GetString(key, CultureInfo.CurrentUICulture)
                               ?? IFluentLocalizer.GetDefault(key);

            return arguments.Length > 0
                ? string.Format(CultureInfo.CurrentCulture, localizedString, arguments)
                : localizedString;
        }
    }
}
