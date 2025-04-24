// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides utility methods for converting file sizes between different units.
/// </summary>
public static class FileSizeConverter
{
    /// <summary>
    /// Converts a size in bytes to its equivalent size in megabytes.
    /// </summary>
    /// <param name="bytes">The size in bytes to convert.</param>
    /// <returns>The equivalent size in megabytes</returns>
    public static decimal ToMegaBytes(long bytes)
    {
        return Decimal.Divide(bytes, 1024 * 1024);
    }

    /// <summary>
    /// Converts a size in bytes to its equivalent size in kilobytes.
    /// </summary>
    /// <param name="bytes">The size in bytes to convert.</param>
    /// <returns>The equivalent size in kilobytes</returns>
    public static decimal ToKiloBytes(long bytes)
    {
        return Decimal.Divide(bytes, 1024);
    }

    /// <summary>
    /// Converts a value in megabytes to its equivalent in bytes.
    /// </summary>
    /// <remarks>This method multiplies the input value by 1,048,576 (1024 * 1024) to calculate the equivalent
    /// size in bytes.</remarks>
    /// <param name="megabytes">The size in megabytes to be converted.</param>
    /// <returns>The equivalent size in bytes as an integer.</returns>
    public static long FromMegaBytes(long megabytes)
    {
        return megabytes * 1024 * 1024;
    }

    /// <summary>
    /// Converts a value from kilobytes to bytes.
    /// </summary>
    /// <param name="kilobytes">The size in kilobytes to be converted.</param>
    /// <returns>The equivalent size in bytes.</returns>
    public static long FromKiloBytes(long kilobytes)
    {
        return kilobytes * 1024;
    }
}
