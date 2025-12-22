// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Migration;

/// <summary />
public static class FluentInputAppearanceExtensions
{
    /// <summary>
    /// Converts an obsolete <see cref="FluentInputAppearance"/> enum value to a <see cref="TextInputAppearance"/>.
    /// </summary>
    /// <param name="appearance"></param>
    /// <returns></returns>
    public static TextInputAppearance ToTextInputAppearance(this FluentInputAppearance appearance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return appearance switch
        {
            FluentInputAppearance.Outline => TextInputAppearance.Outline,
            FluentInputAppearance.Filled => TextInputAppearance.FilledDarker,
            _ => TextInputAppearance.Outline
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Converts an obsolete <see cref="FluentInputAppearance"/> enum value to a <see cref="TextAreaAppearance"/>.
    /// </summary>
    /// <param name="appearance"></param>
    /// <returns></returns>
    public static TextAreaAppearance ToTextAreaAppearance(this FluentInputAppearance appearance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return appearance switch
        {
            FluentInputAppearance.Outline => TextAreaAppearance.Outline,
            FluentInputAppearance.Filled => TextAreaAppearance.FilledDarker,
            _ => TextAreaAppearance.Outline
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
