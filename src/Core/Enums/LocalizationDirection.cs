namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The direction of the text.
/// </summary>
public enum LocalizationDirection
{
    /// <summary>
    /// Left to right.
    /// </summary>
    ltr,

    /// <summary>
    /// Right to left.
    /// </summary>
    rtl
}

public static class LocalizationDirectionExtension
{
    public static bool IsRTL(this LocalizationDirection direction)
    {
        return direction is LocalizationDirection.rtl;
    }

    public static LocalizationDirection GetDirectionFromBoolean(this bool boolValue)
    {
        return boolValue ? LocalizationDirection.rtl : LocalizationDirection.ltr;
    }
}