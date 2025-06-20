// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;
using System.Globalization;

namespace FluentUI.Demo;

/// <summary>
/// Sample of a Custom Localizer,
/// used with `builder.Services.AddFluentUIComponents`.
/// </summary>
internal class MyLocalizer : IFluentLocalizer
{
    public string this[string key, params object[] arguments]
    {
        get
        {
            // Need to add
            //  - builder.Services.AddLocalization();
            //  - app.UseRequestLocalization(new RequestLocalizationOptions().AddSupportedUICultures(["fr"]));
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            // Returns the French version of the string
            if (language == "fr")
            {
                return key switch
                {
                    "FluentSample_Hello" => "Bonjour",
                    _ => IFluentLocalizer.GetDefault(key, arguments),
                };
            }
            // Provide custom translations based on the key
            return key switch
            {
                "DataGrid_ResizeDiscrete" => "Width (+/- 10px)",

                // Fallback to the Default/English if no translation is found
                _ => IFluentLocalizer.GetDefault(key, arguments),
            };
        }
    }
}
