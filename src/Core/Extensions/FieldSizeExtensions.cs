// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

/// <summary>
/// Contains extension methods to convert different size enumerations to <see cref="FluentField.Size"/>.
/// </summary>
public static class FieldSizeExtensions
{
    /// <summary>
    /// Converts a <see cref="CheckboxSize"/> to a <see cref="FieldSize"/>.
    /// </summary>
    public static FieldSize? ToFieldSize(this CheckboxSize? value) => value switch
    {
        CheckboxSize.Medium => FieldSize.Medium,
        CheckboxSize.Large => FieldSize.Large,
        _ => null
    };

    /// <summary>
    /// Converts a <see cref="TextAreaSize"/> to a <see cref="FieldSize"/>.
    /// </summary>
    public static FieldSize? ToFieldSize(this TextAreaSize? value) => value switch
    {
        TextAreaSize.Small => FieldSize.Small,
        TextAreaSize.Medium => FieldSize.Medium,
        TextAreaSize.Large => FieldSize.Large,
        _ => null
    };

    /// <summary>
    /// Converts a <see cref="TextInputSize"/> to a <see cref="FieldSize"/>.
    /// </summary>
    public static FieldSize? ToFieldSize(this TextInputSize? value) => value switch
    {
        TextInputSize.Small => FieldSize.Small,
        TextInputSize.Medium => FieldSize.Medium,
        TextInputSize.Large => FieldSize.Large,
        _ => null
    };

}
