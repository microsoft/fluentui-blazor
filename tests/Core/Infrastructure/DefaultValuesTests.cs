// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Infrastructure;

public class DefaultValuesTests
{
    private static FluentDivider CreateDivider() => new(new LibraryConfiguration());

    // Plain POCO (no IFluentComponentBase constraint required by DefaultValuesComponentBuilder<T>) used to
    // exercise Set() edge cases that are not easily reproducible using real Fluent components.
    private sealed class SamplePoco
    {
        public string? NullableName { get; set; }
        public string RequiredName { get; set; } = string.Empty;
        public int? OptionalAge { get; set; }
        public int Age { get; set; }
        public string ReadOnlyValue { get; } = "fixed";
        public string PublicField = string.Empty;
    }

    // Property inherited from a parent interface is not found by TComponentType.GetProperty(name, type)
    // when TComponentType is an interface, unlike class hierarchies.
    private interface IBaseInterface
    {
        string? Name { get; set; }
    }

    private interface IDerivedInterface : IBaseInterface
    {
        int Age { get; set; }
    }

    [Fact]
    public void ApplyDefaults_RegisteredDefaultValue_SetsPropertyValue()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentDivider>().Set(x => x.Inset, true);
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal((bool?)true, component.Inset);
    }

    [Fact]
    public void ApplyDefaults_NoRegisteredDefaults_PropertyRemainsUnset()
    {
        var defaultValues = new DefaultValues();
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Null(component.Inset);
    }

    [Fact]
    public void ApplyDefaults_UnregisteredComponentType_DoesNotThrow()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentOption<string>>().Set(x => x.Name, "option-name");
        var component = CreateDivider();

        var exception = Record.Exception(() => defaultValues.ApplyDefaults(component));

        Assert.Null(exception);
        Assert.Null(component.Inset);
    }

    [Fact]
    public void ApplyDefaults_MultipleRegisteredProperties_SetsAllPropertyValues()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<FluentDivider>();
        builder.Set(x => x.Inset, true);
        builder.Set(x => x.Vertical, true);
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal((bool?)true, component.Inset);
        Assert.Equal((bool?)true, component.Vertical);
    }

    [Fact]
    public void ApplyDefaults_ForAnyGenericComponent_AppliesToAnyClosedGenericType()
    {
        var defaultValues = new DefaultValues();
        defaultValues.ForAny<FluentOption<object>>().Set(x => x.Name, "generic-default");
        var component = new FluentOption<string>(new LibraryConfiguration());

        defaultValues.ApplyDefaults(component);

        Assert.Equal("generic-default", component.Name);
    }

    [Fact]
    public void SetInitialValues_PropertyWithoutRegisteredDefault_SetsInitialValue()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentDivider>(); // Marks the type as initialized without registering a default value
        var component = CreateDivider();

        defaultValues.SetInitialValues(component, [(nameof(FluentDivider.Inset), true)]);

        Assert.Equal((bool?)true, component.Inset);
    }

    [Fact]
    public void SetInitialValues_PropertyWithRegisteredDefault_KeepsRegisteredValueOverInitialValue()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentDivider>().Set(x => x.Inset, false);
        var component = CreateDivider();
        defaultValues.ApplyDefaults(component);

        defaultValues.SetInitialValues(component, [(nameof(FluentDivider.Inset), true)]);

        Assert.Equal((bool?)false, component.Inset);
    }

    [Fact]
    public void SetInitialValues_ComponentTypeNotInitialized_DoesNotSetValue()
    {
        var defaultValues = new DefaultValues();
        var component = CreateDivider();

        defaultValues.SetInitialValues(component, [(nameof(FluentDivider.Inset), true)]);

        Assert.Null(component.Inset);
    }

    [Fact]
    public void ApplyDefaults_RegisteredBaseClass()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentComponentBase>().Set(x => x.Margin, "10px");
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal("10px", component.Margin);
    }

    [Fact]
    public void ApplyDefaults_RegisteredBaseClass_DifferentValues()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentComponentBase>().Set(x => x.Margin, "10px");
        defaultValues.For<FluentDivider>().Set(x => x.Padding, "20px");
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal("10px", component.Margin);
        Assert.Equal("20px", component.Padding);
    }

    [Fact]
    public void ApplyDefaults_RegisteredBaseClass_OverridesBaseClassValues()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentComponentBase>().Set(x => x.Margin, "10px");
        defaultValues.For<FluentDivider>().Set(x => x.Margin, "50px");
        defaultValues.For<FluentDivider>().Set(x => x.Padding, "20px");
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal("50px", component.Margin);
        Assert.Equal("20px", component.Padding);
    }

    [Fact]
    public void ApplyDefaults_ForAnyNonGenericComponent_RegistersUsingConcreteType()
    {
        var defaultValues = new DefaultValues();
        defaultValues.ForAny<FluentDivider>().Set(x => x.Inset, true);
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal((bool?)true, component.Inset);
    }

    [Fact]
    public void Set_CalledTwiceWithSameValue_KeepsRegisteredValue()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<FluentDivider>();
        builder.Set(x => x.Inset, true);
        builder.Set(x => x.Inset, true);
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal((bool?)true, component.Inset);
    }

    [Fact]
    public void Set_CalledTwiceWithDifferentValue_UpdatesRegisteredValue()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<FluentDivider>();
        builder.Set(x => x.Inset, true);
        builder.Set(x => x.Inset, false);
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal((bool?)false, component.Inset);
    }

    [Fact]
    public void Set_PropertyDeclaredOnBaseType_RegistersUsingBaseComponentPropertyName()
    {
        var defaultValues = new DefaultValues();
        defaultValues.For<FluentDivider>().Set(x => x.Margin, "5px");
        var component = CreateDivider();

        defaultValues.ApplyDefaults(component);

        Assert.Equal("5px", component.Margin);
    }

    [Fact]
    public void Set_NullParameterSelector_ThrowsArgumentNullException()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<SamplePoco>();

        Assert.Throws<ArgumentNullException>(() => builder.Set<string?>(null!, "value"));
    }

    [Fact]
    public void Set_ParameterSelectorTargetsField_ThrowsArgumentException()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<SamplePoco>();

        var exception = Assert.Throws<ArgumentException>(() => builder.Set(x => x.PublicField, "value"));

        Assert.Contains("does not resolve to a public property", exception.Message);
    }

    [Fact]
    public void Set_ReadOnlyProperty_ThrowsArgumentException()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<SamplePoco>();

        var exception = Assert.Throws<ArgumentException>(() => builder.Set(x => x.ReadOnlyValue, "value"));

        Assert.Contains("is read-only", exception.Message);
    }

    [Fact]
    public void Set_NullValueForNonNullableReferenceTypeProperty_ThrowsArgumentException()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<SamplePoco>();

        var exception = Assert.Throws<ArgumentException>(() => builder.Set(x => x.RequiredName, null));

        Assert.Contains("cannot be null", exception.Message);
    }

    [Fact]
    public void Set_NullValueForNullableReferenceTypeProperty_DoesNotThrow()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<SamplePoco>();

        var exception = Record.Exception(() => builder.Set(x => x.NullableName, null));

        Assert.Null(exception);
    }

    [Fact]
    public void Set_NullValueForNullableValueTypeProperty_DoesNotThrow()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<SamplePoco>();

        var exception = Record.Exception(() => builder.Set(x => x.OptionalAge, null));

        Assert.Null(exception);
    }

    [Fact]
    public void Set_InterfacePropertyDeclaredOnParentInterface_ThrowsArgumentException()
    {
        var defaultValues = new DefaultValues();
        var builder = defaultValues.For<IDerivedInterface>();

        var exception = Assert.Throws<ArgumentException>(() => builder.Set(x => x.Name, "value"));

        Assert.Contains("does not resolve to a public property", exception.Message);
    }

    [Fact]
    public void ValidateValueCompatibility_NullForNonNullableValueTypeProperty_ThrowsArgumentException()
    {
        var propertyInfo = typeof(SamplePoco).GetProperty(nameof(SamplePoco.Age))!;

        var exception = Assert.Throws<ArgumentException>(() =>
            DefaultValuesComponentBuilder<SamplePoco>.ValidateValueCompatibility(propertyInfo, null, propertyInfo.Name));

        Assert.Contains("cannot be null", exception.Message);
    }

    [Fact]
    public void ValidateValueCompatibility_ValueNotAssignableToPropertyType_ThrowsArgumentException()
    {
        var propertyInfo = typeof(SamplePoco).GetProperty(nameof(SamplePoco.RequiredName))!;

        var exception = Assert.Throws<ArgumentException>(() =>
            DefaultValuesComponentBuilder<SamplePoco>.ValidateValueCompatibility(propertyInfo, 42, propertyInfo.Name));

        Assert.Contains("must be assignable", exception.Message);
    }
}
