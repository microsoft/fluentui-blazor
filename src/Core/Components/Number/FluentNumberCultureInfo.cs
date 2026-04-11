// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A custom CultureInfo class for FluentNumber component to allow dynamic culture settings without affecting the global culture.
/// </summary>
public class FluentNumberCultureInfo : CultureInfo
{
    /// <summary>
    /// A static instance of FluentNumberCultureInfo with default settings (2 decimal digits, 
    /// "." as decimal separator, and no group separator).
    /// </summary>
    public static readonly FluentNumberCultureInfo Invariant = new(2, ".", "");

    /// <summary>
    /// Initializes a new instance of the FluentNumberCultureInfo class with default settings 
    /// (2 decimal digits, CurrentCulture.NumberFormat.NumberDecimalSeparator as decimal separator, 
    /// and no group separator).
    /// </summary>
    public FluentNumberCultureInfo() : this(2, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "")
    {
    }

    /// <summary>
    /// Initializes a new instance of the FluentNumberCultureInfo class
    /// with specified decimal digits and default decimal separator 
    /// (CurrentCulture.NumberFormat.NumberDecimalSeparator) and no group separator.
    /// </summary>
    /// <param name="decimalDigits">The number of decimal digits to use.</param>
    public FluentNumberCultureInfo(int decimalDigits) : this(decimalDigits, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "")
    {
    }

    /// <summary>
    /// Initializes a new instance of the FluentNumberCultureInfo class with specified decimal digits and decimal separator, and default group separator.
    /// </summary>
    /// <param name="decimalDigits">The number of decimal digits to use.</param>
    /// <param name="decimalSeparator">The character to use as the decimal separator.</param>
    public FluentNumberCultureInfo(int decimalDigits, string decimalSeparator) : this(decimalDigits, decimalSeparator, "")
    {
    }

    /// <summary>
    /// Initializes a new instance of the FluentNumberCultureInfo class with specified decimal digits, decimal separator, and group separator.
    /// </summary>
    /// <param name="decimalDigits">The number of decimal digits to use.</param>
    /// <param name="decimalSeparator">The character to use as the decimal separator.</param>
    /// <param name="groupSeparator">The character to use as the group separator.</param>
    public FluentNumberCultureInfo(int decimalDigits, string decimalSeparator, string groupSeparator) : base("")
    {
        Name = "fuib";
        DisplayName = "FluentUI";
        NumberFormat.NumberDecimalDigits = decimalDigits;
        NumberFormat.NumberDecimalSeparator = decimalSeparator;
        NumberFormat.NumberGroupSeparator = groupSeparator;
    }

    /// <summary>
    /// Returns the full name of the CultureInfo.
    /// </summary>
    public override string Name { get; }

    /// <summary>
    /// Returns the full name of the CultureInfo in the localized language.
    /// </summary>
    public override string DisplayName { get; }
}