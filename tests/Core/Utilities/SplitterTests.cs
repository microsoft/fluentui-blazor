using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class SplitterTests
{
    [Fact]
    public void GetFragments_ReturnsEmpty_WhenTextIsNull()
    {
        // Arrange
        string? text = null;
        IEnumerable<string> highlightedTexts = new List<string>();

        // Act
        var result = AspNetCore.Components.Utilities.Splitter.GetFragments(text!, highlightedTexts, out _, false, false);

        // Assert
        Assert.True(result.IsEmpty);
    }

    [Fact]
    public void GetFragments_ReturnsEmpty_WhenTextIsEmpty()
    {
        // Arrange
        var text = string.Empty;
        IEnumerable<string> highlightedTexts = new List<string>();

        // Act
        var result = AspNetCore.Components.Utilities.Splitter.GetFragments(text, highlightedTexts, out _, false, false);

        // Assert
        Assert.True(result.IsEmpty);
    }

    [Fact]
    public void GetFragments_ReturnsFragments_WhenTextAndHighlightedTextsAreValid()
    {
        // Arrange
        var text = "This is a test string.";
        IEnumerable<string> highlightedTexts = new List<string> { "test" };

        // Act
        var result = AspNetCore.Components.Utilities.Splitter.GetFragments(text, highlightedTexts, out _, false, false);

        // Assert
        Assert.Equal(new[] { "This is a ", "test", " string." }, result.ToArray());
    }
}
