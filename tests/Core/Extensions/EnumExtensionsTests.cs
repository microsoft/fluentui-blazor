// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
        Assert.Equal("Custom-Value", myDescription);
    }

    [Fact]
    public void EnumExtensions_Description_NoAttribute()
    {
        // Arrange
        var myDescription = MyEnum.NoAttribute.GetDescription();

        // Assert
        Assert.Equal("noattribute", myDescription);
    }

    [Fact]
    public void EnumExtensions_Display_Default()
    {
        // Arrange
        var myDispplay = MyEnum.MyDisplay.GetDisplay();

        // Assert
        Assert.Equal("Custom-Value", myDispplay);
    }

    [Fact]
    public void EnumExtensions_Display_NoAttribute()
    {
        // Arrange
        var myDispplay = MyEnum.NoAttribute.GetDisplay();

        // Assert
        Assert.Equal("noattribute", myDispplay);
    }

    [Fact]
    public void EnumExtensions_ToAttribute_Default()
    {
        // Arrange
        var myDescription = MyEnum.MyDescription.ToAttributeValue();

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

    public static IEnumerable<object[]> RegisteredEnumValues()
    {
        return typeof(GeneratedEnumExtensions).GetCustomAttributesData()
            .Where(attribute => attribute.AttributeType.Name == "EnumAttributeValuesAttribute")
            .Select(attribute => (Type)attribute.ConstructorArguments[0].Value!)
            .SelectMany(type => Enum.GetValues(type).Cast<Enum>())
            .Select(value => new object[] { value });
    }

    [Theory]
    [MemberData(nameof(RegisteredEnumValues))]
    public void GetDescription_RegisteredValue_MatchesReflection(Enum value)
    {
        var method = typeof(GeneratedEnumExtensions).GetMethod("GetDescription", BindingFlags.Static | BindingFlags.NonPublic, [value.GetType()]);

        Assert.NotNull(method);
        Assert.False(method.IsGenericMethod);
        Assert.Equal(EnumExtensions.GetDescription(value), method.Invoke(null, [value]));
    }

    [Fact]
    public void ToAttributeValue_UnregisteredEnum_UsesReflectionFallback()
    {
        var method = typeof(GeneratedEnumExtensions).GetMethod("GetDescription", BindingFlags.Static | BindingFlags.NonPublic, [typeof(MyEnum)]);

        Assert.Null(method);
        Assert.Equal("Custom-Value", MyEnum.MyDescription.ToAttributeValue());
    }

    [Theory]
    [InlineData(Color.Default, "var(--colorNeutralForeground1)")]
    [InlineData(Color.Primary, "var(--colorBrandForeground1)")]
    [InlineData((Color)(-1), "")]
    public void ToAttributeValue_GeneratedColor_ReturnsDescription(Color value, string expected)
    {
        var actual = GeneratedEnumExtensions.ToAttributeValue(value);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(Color.Primary, "var(--colorBrandForeground1)")]
    [InlineData((Color)(-1), "")]
    public void ToAttributeValue_NullableGeneratedColor_ReturnsDescription(Color? value, string? expected)
    {
        var actual = GeneratedEnumExtensions.ToAttributeValue(value);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(Color.Default, null)]
    [InlineData(Color.Primary, "var(--colorBrandForeground1)")]
    public void ToAttributeValue_GeneratedSentinel_OmitsMatchingValue(Color value, string? expected)
    {
        var actual = GeneratedEnumExtensions.ToAttributeValue(value, Color.Default);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(Color.Default, null)]
    [InlineData(Color.Primary, "var(--colorBrandForeground1)")]
    public void ToAttributeValue_NullableGeneratedSentinel_OmitsMatchingValue(Color? value, string? expected)
    {
        var actual = GeneratedEnumExtensions.ToAttributeValue(value, Color.Default);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, "")]
    [InlineData(true, null)]
    [InlineData(null, "")]
    public void ToAttributeValue_UnknownGeneratedValue_RespectsEmptyOption(bool? returnEmptyAsNull, string? expected)
    {
        var actual = GeneratedEnumExtensions.ToAttributeValue((Color)(-1), returnEmptyAsNull);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToAttributeValue_TypedExtensionCall_ReusesGeneratedString()
    {
        var first = Orientation.Horizontal.ToAttributeValue();
        var second = Orientation.Horizontal.ToAttributeValue();

        Assert.Equal("horizontal", first);
        Assert.Same(first, second);
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

#pragma warning disable CS0618 // Type or member is obsolete

    [Fact]
    public void EnumExtensions_IsObsoleteEnum_True()
    {
        // Arrange
        var isObsolete = Color.Fill.IsObsolete();

        // Assert
        Assert.True(isObsolete);
    }

    [Fact]
    public void EnumExtensions_IsObsoleteEnum_False()
    {
        // Arrange
        var isObsolete = Color.Primary.IsObsolete();

        // Assert
        Assert.False(isObsolete);
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
