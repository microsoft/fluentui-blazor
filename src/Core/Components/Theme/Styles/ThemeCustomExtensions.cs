// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static class ThemeCustomExtensions
{
    /// <summary>
    /// Converts the current theme custom object to a JSON string.
    /// </summary>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public static string ToJson(this ThemeCustom themeCustom)
    {
        var dico = themeCustom.ToDictionary().Where(i => !string.IsNullOrEmpty(i.Value));
        var data = System.Linq.Enumerable.ToDictionary(dico, x => x.Key, x => x.Value, StringComparer.Ordinal);
        var options = new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
        };
        return System.Text.Json.JsonSerializer.Serialize(data, options);
    }

    /// <summary>
    /// Populates the theme custom object with values from a JSON string.
    /// </summary>
    /// <param name="themeCustom"></param>
    /// <param name="json"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public static ThemeCustom FromJson(this ThemeCustom themeCustom, string json)
    {
        var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string?>>(json);
        if (data is not null)
        {
            themeCustom.FromDictionary(data);
        }
        
        return themeCustom;
    }
}