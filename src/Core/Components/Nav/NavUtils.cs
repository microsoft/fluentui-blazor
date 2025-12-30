// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Contains utility methods for navigation components.
/// </summary>
public class NavUtils
{
    /// <summary>
    /// Gets the active (filled) variant of the provided icon. If the filled icon is not available, the original icon is returned.
    /// </summary>
    /// <param name="icon">The name of the icon.</param>
    /// <param name="active">Whether the icon is active.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage(Justification = "We can't test the Icon.* DLLs here")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public static Icon? GetActiveIcon(Icon icon, bool active)
    {
        if (active)
        {
            var iconInfo = new IconInfo
            {

                Name = icon.Name,
                Size = IconSize.Size20,
                Variant = IconVariant.Filled,
            };

            //This cannot be tested as the Icons assembly is not available in bUnit tests
            if (iconInfo.TryGetInstance(out var customIcon))
            {
                return customIcon;
            }
        }

        return icon;
    }
}
