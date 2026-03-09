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

[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "Extension methods for ThemeCustom should be in the same file as the class.")]
public partial class ThemeCustom
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ThemeCustom"/> class
    /// </summary>
    public ThemeCustom()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeCustom"/> class with values from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string containing the theme custom values.</param>
    [RequiresUnreferencedCode("This constructor uses reflection which may be trimmed.")]
    public ThemeCustom(string json)
    {
        FromJson(json);
    }

    /// <summary>
    /// Converts the current theme custom object to a JSON string.
    /// </summary>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public string ToJson() => ThemeCustomExtensions.ToJson(this);

    /// <summary>
    /// Populates the theme custom object with values from a JSON string.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public ThemeCustom FromJson(string json) => ThemeCustomExtensions.FromJson(this, json);
}
