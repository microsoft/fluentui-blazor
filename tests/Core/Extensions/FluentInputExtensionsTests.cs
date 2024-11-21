// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class FluentInputExtensionsTests
{

    [Theory]
    [InlineData("Custom-Value", true, "Custom-Value", null)]
    public void FluentInputExtensions_TryParseSelectableValueFromString_String(string value, bool expectedValid, string expectedResult, string? expectedValidationMessage)
    {
        // Arrange
        var input = new FluentSelect<string>();

        // Act
        var ok = FluentInputExtensions.TryParseSelectableValueFromString(input, value, out var result, out var validationErrorMessage);

        // Assert
        Assert.True(ok == expectedValid);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedValidationMessage, validationErrorMessage);
    }

    [Theory]
    [InlineData("10", true, 10, null)]
    [InlineData("-20", true, -20, null)]
    [InlineData("Invalid", false, 0, "The 'Unknown Bound Field' field is not valid.")]
    public void FluentInputExtensions_TryParseSelectableValueFromString_Number(string value, bool expectedValid, int expectedResult, string? expectedValidationMessage)
    {
        // Arrange
        var input = new FluentSelect<int>();

        // Act
        var ok = FluentInputExtensions.TryParseSelectableValueFromString(input, value, out var result, out var validationErrorMessage);

        // Assert
        Assert.True(ok == expectedValid);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedValidationMessage, validationErrorMessage);
    }

    [Theory]
    [InlineData("True", true, true, null)]
    [InlineData("False", true, false, null)]
    [InlineData("Invalid", false, false, "The 'Unknown Bound Field' field is not valid.")]
    public void FluentInputExtensions_TryParseSelectableValueFromString_Boolean(string value, bool expectedValid, bool expectedResult, string? expectedValidationMessage)
    {
        // Arrange
        var input = new FluentSelect<bool>();

        // Act
        var ok = FluentInputExtensions.TryParseSelectableValueFromString(input, value, out var result, out var validationErrorMessage);

        // Assert
        Assert.True(ok == expectedValid);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedValidationMessage, validationErrorMessage);
    }

    [Theory]
    [InlineData("True", true, true, null)]
    [InlineData("False", true, false, null)]
    [InlineData("", true, null, null)]
    [InlineData("Invalid", false, null, "The 'Unknown Bound Field' field is not valid.")]
    public void FluentInputExtensions_TryParseSelectableValueFromString_BooleanNullable(string value, bool expectedValid, bool? expectedResult, string? expectedValidationMessage)
    {
        // Arrange
        var input = new FluentSelect<bool?>();

        // Act
        var ok = FluentInputExtensions.TryParseSelectableValueFromString(input, value, out var result, out var validationErrorMessage);

        // Assert
        Assert.True(ok == expectedValid);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedValidationMessage, validationErrorMessage);
    }
}
