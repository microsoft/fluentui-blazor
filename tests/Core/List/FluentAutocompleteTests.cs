using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.List;

public class FluentAutocompleteTests : TestBase
{
    public FluentAutocompleteTests()
    {
        TestContext.JSInterop.SetupModule(FluentAutocomplete<string>.JAVASCRIPT_FILE);
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

    private IEnumerable<Customer> GetCustomers()
    {
        yield return new Customer(1, "Denis Voituron");
        yield return new Customer(2, "Vincent Baaij");
        yield return new Customer(3, "Bill gates");
    }

    private record Customer(int Id, string Name);

}
