using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.List;

public class FluentAutocompleteTests : TestBase
{
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = new LibraryConfiguration();

    public FluentAutocompleteTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.Services.AddSingleton(LibraryConfiguration);
    }

    [Fact]
    public void FluentAutocomplete_Empty()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Items, GetCustomers());
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAutocomplete_Width_Empty()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Items, GetCustomers());
            parameters.Add(p => p.Width, string.Empty);
        });

        // Assert
        Assert.Contains("width: 250px", cut.Markup);
        cut.Verify();
    }

    [Fact]
    public void FluentAutocomplete_Opened()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Items, GetCustomers());
        });

        // Act
        cut.Find("fluent-text-field").Click();

        // Assert
        cut.Verify();
    }

    [Theory()]
    [InlineData("Escape")]
    [InlineData("Backspace")]
    [InlineData("ArrowDown")]
    [InlineData("ArrowUp")]
    public void FluentAutocomplete_Keyboard(string keyCode)
    {
        KeyCode code = keyCode switch
        {
            "Escape" => KeyCode.Escape,
            "Backspace" => KeyCode.Backspace,
            "ArrowDown" => KeyCode.Down,
            "ArrowUp" => KeyCode.Up,
            _ => KeyCode.Unknown
        };

        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Items, GetCustomers());
            parameters.Add(p => p.SelectedOptions, GetCustomers().Take(2));
        });

        // Act
        var input = cut.Find("fluent-text-field");
        input.Click();
        cut.FindComponent<FluentKeyCode>().Instance.OnKeyDownRaisedAsync((int)code, "", false, false, false, false, 0, string.Empty);

        // Assert
        cut.Verify(suffix: keyCode);
    }

    [Fact]
    public void FluentAutocomplete_MultipleFalse_Exception()
    {
        // Arrange & Act
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
            {
                parameters.Add(p => p.Id, "myComponent");
                parameters.Add(p => p.Multiple, false);
            });
        });

        // Assert
        Assert.Equal("For FluentAutocomplete, this property must be True. Set the MaximumSelectedOptions property to 1 to select just one item.", ex.InnerException?.Message);
    }

    [Fact]
    public void FluentAutocomplete_Templates()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.OptionValue, context => context.Id.ToString());

            // Add a HeaderContent template
            parameters.Add(p => p.HeaderContent, context =>
            {
                return $"<header>Please, select an item</header>";
            });

            // Add an Item template
            parameters.Add(p => p.OptionTemplate, context =>
            {
                return $"<div>{context?.Id} {context?.Name}</div>";
            });

            // Add a FooterContent template
            parameters.Add(p => p.FooterContent, context =>
            {
                return $"<footer>{context.Count()} items found</footer>";
            });

            parameters.Add(p => p.Items, GetCustomers());
        });

        // Act
        cut.Find("fluent-text-field").Click();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAutocomplete_SelectedOptions()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.OptionValue, context => context.Id.ToString());
            parameters.Add(p => p.OptionText, context => context.Name);
            parameters.Add(p => p.Items, GetCustomers());
            parameters.Add(p => p.SelectedOptions, GetCustomers().Take(2));
        });

        // Act
        cut.Find("fluent-text-field").Click();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAutocomplete_SelectedOptions_Template()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.OptionValue, context => context.Id.ToString());
            parameters.Add(p => p.OptionText, context => context.Name);
            parameters.Add(p => p.Items, GetCustomers());
            parameters.Add(p => p.SelectedOptions, GetCustomers().Take(2));

            // Add an Item template
            parameters.Add(p => p.OptionTemplate, context =>
            {
                return $"<div>{context?.Id} {context?.Name}</div>";
            });
        });

        // Act
        cut.Find("fluent-text-field").Click();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAutocomplete_SelectedOptions_OnDismissClick()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentAutocomplete<Customer>>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.OptionValue, context => context.Id.ToString());
            parameters.Add(p => p.OptionText, context => context.Name);
            parameters.Add(p => p.Items, GetCustomers());
            parameters.Add(p => p.SelectedOptions, GetCustomers().Take(2));
        });

        // Act (click on the Dismiss button)
        // The first SelectedOption is removed
        cut.Find("fluent-badge svg").Click();

        // Assert
        cut.Verify();
    }

    // Sample data...
    private static IEnumerable<Customer> GetCustomers()
    {
        yield return new Customer(1, "Denis Voituron");
        yield return new Customer(2, "Vincent Baaij");
        yield return new Customer(3, "Bill Gates");
    }

    private record Customer(int Id, string Name);

}
