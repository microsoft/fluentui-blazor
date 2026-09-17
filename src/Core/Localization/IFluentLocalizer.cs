// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Localizer that uses the FluentUI language resource to localize strings.
/// </summary>
public interface IFluentLocalizer
{
    /// <summary>
    /// Gets the string resource with the given name.
    /// </summary>
    /// <param name="key">The name of the string resource.</param>
    /// <param name="arguments">The list of arguments to be passed to the string resource.</param>
    /// <returns>The string resource.</returns>
    string this[string key, params object[] arguments] { get; }

    /// <summary>
    /// Gets the default string resource (English) with the given name.
    /// </summary>
    /// <param name="key">The name of the string resource.</param>
    /// <param name="arguments">The list of arguments to be passed to the string resource.</param>
    /// <returns>The string resource.</returns>
    public static string GetDefault(string key, params object[] arguments)
    {
        var localizedString = Localization.LanguageResource.ResourceManager.GetString(key, System.Globalization.CultureInfo.InvariantCulture);

        if (localizedString == null)
        {
            throw new ArgumentException($"Key '{key}' not found in LanguageResource.", paramName: nameof(key));
        }

        return arguments.Length > 0
            ? string.Format(System.Globalization.CultureInfo.InvariantCulture, localizedString, arguments)
            : localizedString;
    }
}
