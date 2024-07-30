// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Bunit;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public partial class EnumExtensionsTests
{
    private enum MyEnum
    {
        [Description("Custom-Value")]
        MyDescription,

        [Display(Name = "Custom-Value")]
        MyDisplay,

        NoAttribute,
    }

    [Fact]
    public void EnumExtensions_Description_Default()
    {
        // Arrange
        var myDescription = MyEnum.MyDescription.GetDescription();

        // Assert
        Assert.Equal("custom-value", myDescription);
    }

    [Fact]
    public void EnumExtensions_Description_Original()
    {
        // Arrange
        var myDescription = MyEnum.MyDescription.GetDescription(lowercase: false);

        // Assert
        Assert.Equal("Custom-Value", myDescription);
    }

    [Fact]
    public void EnumExtensions_Description_NoAttribute()
    {
        // Arrange
        var myDescription = MyEnum.NoAttribute.GetDescription(lowercase: false);

        // Assert
        Assert.Equal("NoAttribute", myDescription);
    }

    [Fact]
    public void EnumExtensions_Display_Default()
    {
        // Arrange
        var myDispplay = MyEnum.MyDisplay.GetDisplay();

        // Assert
        Assert.Equal("custom-value", myDispplay);
    }

    [Fact]
    public void EnumExtensions_Display_Original()
    {
        // Arrange
        var myDispplay = MyEnum.MyDisplay.GetDisplay(lowercase: false);

        // Assert
        Assert.Equal("Custom-Value", myDispplay);
    }

    [Fact]
    public void EnumExtensions_Display_NoAttribute()
    {
        // Arrange
        var myDispplay = MyEnum.NoAttribute.GetDisplay(lowercase: false);

        // Assert
        Assert.Equal("NoAttribute", myDispplay);
    }

    [Fact]
    public void EnumExtensions_ToAttribute_Default()
    {
        // Arrange
        var myDescription = MyEnum.MyDescription.ToAttributeValue();

        // Assert
        Assert.Equal("custom-value", myDescription);
    }

    [Fact]
    public void EnumExtensions_ToAttribute_Original()
    {
        // Arrange
        var myDescription = MyEnum.MyDescription.ToAttributeValue(lowercase: false);

        // Assert
        Assert.Equal("Custom-Value", myDescription);
    }

    [Fact]
    public void EnumExtensions_ToAttribute_Null()
    {
        // Arrange
        MyEnum? myEnum = null;
        var myDescription = myEnum.ToAttributeValue();

        // Assert
        Assert.Null(myDescription);
    }

    [Fact]
    public void EnumExtensions_IsNullableEnum_True()
    {
        // Arrange
        var myEnum = typeof(MyEnum?);
        var isNullable = myEnum.IsNullableEnum();

        // Assert
        Assert.True(isNullable);
    }

    [Fact]
    public void EnumExtensions_IsNullableEnum_False()
    {
        // Arrange
        var myEnum = typeof(MyEnum);
        var isNullable = myEnum.IsNullableEnum();

        // Assert
        Assert.False(isNullable);
    }
}
