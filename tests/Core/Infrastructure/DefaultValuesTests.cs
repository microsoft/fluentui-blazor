// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Infrastructure;

public class DefaultValuesTests
{
    private static FluentDivider CreateDivider() => new(new LibraryConfiguration());

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
}
