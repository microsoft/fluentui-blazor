// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class IFluentComponentBaseTests
{
    [Fact]
    public void IsDisposed_Accessed_ReturnsFalse()
    {
        // Arrange
        var component = new TestComponent
        {
            Id = "test-id",
            Class = "test-class",
            Style = "color: red;",
            Margin = "1px",
            Padding = "2px",
            Data = new object(),
            AdditionalAttributes = new Dictionary<string, object> { { "data-test", "value" } }
        };

        // Act
        var isDisposed = ((IFluentComponentBase)component).IsDisposed;

        // Assert
        Assert.False(isDisposed);
    }

    // Helper test class placed inside the test class per project constraints.
    private class TestComponent : IFluentComponentBase
    {
        public string? Id { get; set; }
        public string? Class { get; set; }
        public string? Style { get; set; }
        public string? Margin { get; set; }
        public string? Padding { get; set; }
        public object? Data { get; set; }
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}
