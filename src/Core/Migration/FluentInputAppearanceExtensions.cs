// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
}
