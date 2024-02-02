using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Highlighter;
public class FluentHighlighterTests : TestBase
{
    [Fact]
    public void FluentHighlighter_Default()
    {
        //Arrange
        bool caseSensitive = default!;
        string highlightedText = default!;
        string text = default!;
        string delimiters = default!;
        bool untilNextBoundary = default!;
        var cut = TestContext.RenderComponent<FluentHighlighter>(parameters => parameters
            .Add(p => p.CaseSensitive, caseSensitive)
            .Add(p => p.HighlightedText, highlightedText)
            .Add(p => p.Text, text)
            .Add(p => p.Delimiters, delimiters)
            .Add(p => p.UntilNextBoundary, untilNextBoundary)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

