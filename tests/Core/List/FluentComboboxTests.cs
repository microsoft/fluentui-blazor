// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.List;

public class FluentComboboxTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    public FluentComboboxTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.Services.AddSingleton(LibraryConfiguration);
    }

    [Fact]
    public void FluentCombobox_AutofocusAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCombobox<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Autofocus, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCombobox_NameAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentCombobox<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Name, "myComponentName");
            parameters.Add(p => p.Autofocus, true);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCombobox_ClearSelection()
    {
        // Arrange
        var customer = new Customer(1, "Contoso");
        var cut = TestContext.RenderComponent<FluentCombobox<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Items, [customer]);
            parameters.Add(p => p.OptionText, customer => customer.Name);
            parameters.Add(p => p.SelectedOption, customer);
        });

        //Act
        cut.SetParametersAndRender(parameters => parameters.Add(p => p.SelectedOption, null));

        // Assert
        cut.Verify();
    }

    private record Customer(int Id, string Name);
}
