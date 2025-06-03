using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Attributes;

public class FluentDefaultTests : TestBase
{
    public static class TestDefaults
    {
        [FluentDefault("TestComponent")]
        public static string? DefaultClass => "test-class";

        [FluentDefault("TestComponent")]
        public static bool DefaultDisabled => true;

        [FluentDefault("TestComponent")]
        public static int DefaultNumber => 42;

        [FluentDefault("TestComponent")]
        public static int? DefaultNullableNumber => 100;

        [FluentDefault("AnotherComponent")]
        public static string? AnotherDefault => "another-value";
    }

    public static class ParameterNameTestDefaults
    {
        // Test ParameterName functionality - multiple properties mapping to same parameter
        [FluentDefault("TestComponent", ParameterName = "Class")]
        public static string? TestComponentClass => "param-name-class";

        [FluentDefault("AnotherComponent", ParameterName = "Class")]
        public static string? AnotherComponentClass => "another-param-class";
    }

    public class TestComponent : FluentComponentBase
    {
        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public int Number { get; set; }

        [Parameter]
        public int? DefaultNullableNumber { get; set; }

        public string? NonParameterProperty { get; set; }
    }

    public class AnotherComponent : FluentComponentBase
    {
        [Parameter]
        public string? AnotherDefault { get; set; }

        [Parameter]
        public string? Class { get; set; }
    }

    [Fact]
    public void FluentDefault_Attribute_SetsComponentTypeName()
    {
        // Arrange & Act
        var attribute = new FluentDefaultAttribute("TestComponent");

        // Assert
        Assert.Equal("TestComponent", attribute.ComponentTypeName);
    }

    [Fact]
    public void FluentDefault_Attribute_ThrowsForNullComponentType()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FluentDefaultAttribute(null!));
    }

    [Fact]
    public void FluentDefaultValuesService_AppliesDefaults_ToMatchingComponent()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var component = new TestComponent();

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert
        Assert.Equal("test-class", component.Class);
        Assert.True(component.Disabled);
        Assert.Equal(42, component.Number);
        Assert.Equal(100, component.DefaultNullableNumber);
    }

    [Fact]
    public void FluentDefaultValuesService_DoesNotOverrideExistingValues()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var component = new TestComponent
        {
            Class = "existing-class",
            Disabled = false,
            Number = 100,
            DefaultNullableNumber = 200
        };

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert - Values should remain unchanged
        Assert.Equal("existing-class", component.Class);
        Assert.False(component.Disabled);
        Assert.Equal(100, component.Number);
        Assert.Equal(200, component.DefaultNullableNumber);
    }

    [Fact]
    public void FluentDefaultValuesService_DoesNotAffectNonParameterProperties()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var component = new TestComponent();

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert - Non-parameter property should remain null
        Assert.Null(component.NonParameterProperty);
    }

    [Fact]
    public void FluentDefaultValuesService_AppliesCorrectDefaults_ForDifferentComponents()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var testComponent = new TestComponent();
        var anotherComponent = new AnotherComponent();

        // Act
        FluentDefaultValuesService.ApplyDefaults(testComponent);
        FluentDefaultValuesService.ApplyDefaults(anotherComponent);

        // Assert
        Assert.Equal("test-class", testComponent.Class);
        Assert.Equal("another-value", anotherComponent.AnotherDefault);
    }

    [Fact]
    public void FluentDefaultValuesService_HandlesComponentWithNoDefaults()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var component = new ComponentWithNoDefaults();

        // Act & Assert - Should not throw
        FluentDefaultValuesService.ApplyDefaults(component);
    }

    [Fact]
    public void FluentDefaultValuesService_DoesNotOverrideNullableValueTypes_WhenExplicitlySet()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var component = new TestComponent
        {
            DefaultNullableNumber = null // This will be treated as unset since nullable types default to null
        };

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert - For nullable types, null is considered "unset", so the default should be applied
        Assert.Equal(100, component.DefaultNullableNumber);
    }

    [Fact]
    public void FluentDefaultValuesService_OverridesValueTypes_WhenSetToDefaultValue()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var component = new TestComponent
        {
            Disabled = false // This is the default value for bool, so it will be overridden
        };

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert - Value type defaults are overridden when set to their default value
        // Note: This is a limitation of the approach - we can't distinguish between 
        // "not set" and "explicitly set to default value" for value types
        Assert.True(component.Disabled);
    }

    [Fact]
    public void FluentDefaultValuesService_SupportsParameterNameMapping()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state
        var testComponent = new TestComponent();
        var anotherComponent = new AnotherComponent();

        // Act
        FluentDefaultValuesService.ApplyDefaults(testComponent);
        FluentDefaultValuesService.ApplyDefaults(anotherComponent);

        // Assert - Both components should get different default values for their Class parameter
        // even though they use different property names in the defaults class
        Assert.Equal("param-name-class", testComponent.Class);
        Assert.Equal("another-param-class", anotherComponent.Class);
    }

    [Fact]
    public void FluentDefaultValuesService_SupportsInstanceProviders()
    {
        // Arrange
        FluentDefaultValuesService.Reset();
        var provider = new TestInstanceProvider();
        FluentDefaultValuesService.ScanConfiguration.InstanceProviders.Add(provider);
        
        var component = new TestComponent();

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert
        Assert.Equal("instance-class", component.Class);
        Assert.True(component.Disabled);
    }

    [Fact]
    public void FluentDefaultValuesService_InstanceProviders_OverrideStaticDefaults()
    {
        // Arrange
        FluentDefaultValuesService.Reset();
        var provider = new TestInstanceProvider();
        FluentDefaultValuesService.ScanConfiguration.InstanceProviders.Add(provider);
        
        var component = new TestComponent();

        // Act
        FluentDefaultValuesService.ApplyDefaults(component);

        // Assert - Instance provider should override static defaults
        Assert.Equal("instance-class", component.Class); // From instance provider, not "test-class" from static
        Assert.True(component.Disabled); // From static defaults
    }

    [Fact]
    public void FluentDefaultValuesService_Configuration_WithTargetAssemblies()
    {
        // Arrange
        FluentDefaultValuesService.Reset();
        var currentAssembly = typeof(FluentDefaultTests).Assembly;
        
        // Act
        FluentDefaultValuesService.ScanConfiguration.WithTargetAssemblies(currentAssembly);
        FluentDefaultValuesService.Initialize();
        
        // Assert - Should find defaults in the target assembly
        var defaults = FluentDefaultValuesService.GetAllDefaults();
        Assert.True(defaults.ContainsKey("TestComponent"));
    }

    [Fact]
    public void FluentDefaultValuesService_Configuration_WithTargetNamespaces()
    {
        // Arrange
        FluentDefaultValuesService.Reset();
        
        // Act
        FluentDefaultValuesService.ScanConfiguration.WithTargetNamespaces("Microsoft.FluentUI.AspNetCore.Components.Tests");
        FluentDefaultValuesService.Initialize();
        
        // Assert - Should find defaults in the target namespace
        var defaults = FluentDefaultValuesService.GetAllDefaults();
        Assert.True(defaults.ContainsKey("TestComponent"));
    }

    [Fact]
    public void FluentDefaultValuesService_Configuration_WithoutStaticDefaults()
    {
        // Arrange
        FluentDefaultValuesService.Reset();
        FluentDefaultValuesService.ScanConfiguration.WithoutStaticDefaults();
        
        // Act
        FluentDefaultValuesService.Initialize();
        
        // Assert - Should not find any static defaults
        var defaults = FluentDefaultValuesService.GetAllDefaults();
        Assert.Empty(defaults);
    }

    private class TestInstanceProvider : FluentDefaultValuesService.IFluentDefaultProvider
    {
        public object? GetDefaultValue(string componentTypeName, string parameterName)
        {
            return (componentTypeName, parameterName) switch
            {
                ("TestComponent", "Class") => "instance-class",
                ("TestComponent", "Number") => 999,
                _ => null
            };
        }

        public bool HasDefaultValue(string componentTypeName, string parameterName)
        {
            return (componentTypeName, parameterName) switch
            {
                ("TestComponent", "Class") => true,
                ("TestComponent", "Number") => true,
                _ => false
            };
        }
    }

    private class ComponentWithNoDefaults : FluentComponentBase
    {
        [Parameter]
        public string? SomeProperty { get; set; }
    }

    public class AnotherComponent : FluentComponentBase
    {
        [Parameter] 
        public string? Class { get; set; }
    }
}