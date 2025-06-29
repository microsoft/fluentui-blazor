// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class DisplayAttributeExtensionTests
{

    [Fact]
    public void Test_DisplayAttribute_Returns_Correct_Name()
    {
        var expected = "Test Name";
        var actual = typeof(TestModel).GetDisplayAttributeString(nameof(TestModel.TestProperty));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_DisplayAttribute_Returns_Null_When_No_Attribute()
    {
        var actual = typeof(TestModel).GetDisplayAttributeString(nameof(TestModel.PropertyWithoutAttr));

        Assert.Null(actual);
    }

    [Fact]
    public void Test_DisplayAttribute_Returns_Null_When_PropertyInfo_Is_Null()
    {
        PropertyInfo? nullProperty = null;
        var displayAttribute = nullProperty?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
        Assert.Null(displayAttribute);
    }

    [Fact]
    public void Test_DisplayAttribute_Returns_Null_When_MetadataType_Used()
    {
        var metadataType = typeof(TestModel).GetCustomAttributes(typeof(MetadataTypeAttribute), false).FirstOrDefault() as MetadataTypeAttribute;
        Assert.NotNull(metadataType);
        var property = metadataType?.MetadataClassType.GetProperty(nameof(TestModel.TestProperty));
        var displayAttribute = property?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
        Assert.Null(displayAttribute);
    }

    [Fact]
    public void Test_DisplayAttribute_Returns_Null_When_Property_Not_Found()
    {
        var actual = typeof(TestModel).GetDisplayAttributeString("NonExistentProperty");

        Assert.Null(actual);
    }

    [Fact]
    public void Test_DisplayAttribute_Returns_Name_From_MetadataType()
    {
        var expected = "Metadata Display Name";
        var actual = typeof(TestModelWithMetadata).GetDisplayAttributeString(nameof(TestModelWithMetadata.PropertyWithMetadataDisplay));

        Assert.Equal(expected, actual);
    }

    [MetadataType(typeof(TestModelMetadata))]
    private class TestModel
    {
        public static object PropertyWithoutAttr { get; internal set; } = default!;

        [Display(Name = "Test Name")]
        public string? TestProperty { get; set; }
    }

    private class TestModelMetadata
    {
        // This metadata class can contain attributes for TestModel properties
        // For the test, we intentionally don't add any DisplayAttribute here
        // so that the test can verify the behavior when no DisplayAttribute exists in metadata
        public string? TestProperty { get; set; }
    }

    [MetadataType(typeof(TestModelWithMetadataMetadata))]
    private class TestModelWithMetadata
    {
        public string? PropertyWithMetadataDisplay { get; set; }
    }

    private class TestModelWithMetadataMetadata
    {
        [Display(Name = "Metadata Display Name")]
        public string? PropertyWithMetadataDisplay { get; set; }
    }
}
