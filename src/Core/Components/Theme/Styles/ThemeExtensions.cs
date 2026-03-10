// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static class ThemeExtensions
{
    /// <summary>
    /// Converts the current theme custom object to a JSON string.
    /// </summary>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public static string ToJson(this Theme theme)
    {
        var dico = theme.ToDictionary().Where(i => !string.IsNullOrEmpty(i.Value));
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
    /// <param name="Theme"></param>
    /// <param name="json"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public static Theme FromJson(this Theme Theme, string json)
    {
        var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string?>>(json);
        if (data is not null)
        {
            Theme.FromDictionary(data);
        }

        return Theme;
    }
}

[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "Extension methods for Theme should be in the same file as the class.")]
public partial class Theme
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="Theme"/> class
    /// </summary>
    public Theme()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Theme"/> class with values from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string containing the theme custom values.</param>
    [RequiresUnreferencedCode("This constructor uses reflection which may be trimmed.")]
    public Theme(string json)
    {
        FromJson(json);
    }

    /// <summary>
    /// Converts the current theme custom object to a JSON string.
    /// </summary>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public string ToJson() => ThemeExtensions.ToJson(this);

    /// <summary>
    /// Populates the theme custom object with values from a JSON string.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("This method uses reflection which may be trimmed.")]
    public Theme FromJson(string json) => ThemeExtensions.FromJson(this, json);
}
