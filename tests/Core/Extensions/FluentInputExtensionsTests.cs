// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class FluentInputExtensionsTests
{

    [Theory]
    [InlineData("Custom-Value", true, "Custom-Value", null)]
    public void FluentInputExtensions_TryParseSelectableValueFromString_String(string value, bool expectedValid, string expectedResult, string? expectedValidationMessage)
    {
        // Arrange
        var input = new FluentSelect<string, string>(LibraryConfiguration.Empty);

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
        var input = new FluentSelect<int, int>(LibraryConfiguration.Empty);

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
        var input = new FluentSelect<bool, bool>(LibraryConfiguration.Empty);

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
        var input = new FluentSelect<bool?, bool?>(LibraryConfiguration.Empty);

        // Act
        var ok = FluentInputExtensions.TryParseSelectableValueFromString(input, value, out var result, out var validationErrorMessage);

        // Assert
        Assert.True(ok == expectedValid);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedValidationMessage, validationErrorMessage);
    }

    private const string attributeName = "Id";

    [Fact]
    public void FluentInputExtensions_ThrowNullableParameters_NullId_Throws()
    {
        // Arrange
        var component = new FluentSelect<string, string>(LibraryConfiguration.Empty);
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { attributeName, null },
        });

        // Act
        var ex = Assert.Throws<InvalidOperationException>(
            () => parameters.ThrowNullableParameters(component, attributeName));

        // Assert
        Assert.Equal("The 'Id' parameter of 'FluentSelect`2' cannot be null. Omit the parameter to use the component default value, or provide a non-null value.", ex.Message);
    }

    [Fact]
    public void FluentInputExtensions_ThrowNullableParameters_NonNullId_DoesNotThrow()
    {
        // Arrange
        var component = new FluentSelect<string, string>(LibraryConfiguration.Empty);
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { attributeName, "my-id" },
        });

        // Act & Assert
        parameters.ThrowNullableParameters(component, attributeName);
    }
}
