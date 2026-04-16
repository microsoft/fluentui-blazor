// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class MarkupStringSanitizedTests
{
    [Theory]
    [InlineData("<div />", "div", null, null, null)]
    [InlineData("<div>my-content</div>", "div", null, null, "my-content")]
    [InlineData("<div style='my-attribute'>my-content</div>", "div", "style", "my-attribute", "my-content")]
    [InlineData("<div style=\"my-attribute\">my-content</div>", "div", "style", "my-attribute", "my-content")]
    [InlineData("<div style=\"my: 'attribute'\">my-content</div>", "div", "style", "my: 'attribute'", "my-content")]
    [InlineData("<div style='my-attribute'></div>", "div", "style", "my-attribute", null)]
    [InlineData("<div style=\"my-attribute\"></div>", "div", "style", "my-attribute", null)]
    [InlineData("<div style=\"my-attribute\" />", "div", "style", "my-attribute", null)]
    [InlineData("<div style='my-attribute' />", "div", "style", "my-attribute", null)]
    public void MarkupStringSanitized_ParseTagAndContent(string input, string tag, string? attribute, string? value, string? content)
    {
        // Act
        var result = MarkupStringSanitized.ParseTagAndContent(input);

        // Assert
        Assert.Equal(tag, result.Tag);
        Assert.Equal(attribute, result.Attribute);
        Assert.Equal(value, result.AttributeValue);
        Assert.Equal(content, result.Content);
    }

    [Theory]
    [InlineData("<div")]
    [InlineData("div>")]
    [InlineData("<>")]
    [InlineData("")]
    [InlineData("<div style= >")]
    public void MarkupStringSanitized_ParseTagAndContent_ThrowsException(string input)
    {
        const string exceptionMessage = "Value must be in the format '<tag>content</tag>' or '<tag attribute=\"attr-value\">content</tag>'. (Parameter 'value')";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => MarkupStringSanitized.ParseTagAndContent(input));
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Theory]
    [InlineData("<div />", "<div />")]
    [InlineData("<div>my-content</div>", "<div>my-content</div>")]
    [InlineData("<div style='my-attribute'>my-content</div>", "<div style='my-attribute'>my-content</div>")]
    [InlineData("<div style=\"my-attribute\">my-content</div>", "<div style=\"my-attribute\">my-content</div>")]
    [InlineData("<div style=\"my: 'attribute'\">my-content</div>", "<div style=\"my: 'attribute'\">my-content</div>")]
    [InlineData("<div style='my-attribute'></div>", "<div style='my-attribute' />")]
    [InlineData("<div style=\"my-attribute\"></div>", "<div style=\"my-attribute\" />")]
    [InlineData("<div style=\"my-attribute\" />", "<div style=\"my-attribute\" />")]
    [InlineData("<div style='my-attribute' />", "<div style='my-attribute' />")]
    public void MarkupStringSanitized_Default(string input, string expected)
    {
        // Act
        var result = new MarkupStringSanitized(input, null);

        // Assert
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData("<style> .my-class { color: ### } </style>", "red; } </style> <script>alert('XSS')</script> <style>")]
    [InlineData("<style> .my-class { color: ### } </style>", "red; } </style> <script>alert('XSS')</script> <style>", false)]
    public void MarkupStringSanitized_Constructor_ThrowsException(string input, string xss, bool throwOnUnsafe = true)
    {
        const string exceptionMessage = "The provided CSS inline style contains potentially unsafe content";

        // Act & Assert
        if (throwOnUnsafe)
        {
            var exception = Assert.Throws<InvalidOperationException>(() => StartMarkupStringSanitized());
            Assert.Contains(exceptionMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            var result = StartMarkupStringSanitized();
            Assert.Equal("<style />", result.Value);
        }

        MarkupStringSanitized StartMarkupStringSanitized()
        {
            MarkupSanitizedOptions.ThrowOnUnsafe = throwOnUnsafe;
            var result = new MarkupStringSanitized(input.Replace("###", xss), null);
            MarkupSanitizedOptions.ThrowOnUnsafe = true;    // Reset to default after test
            return result;
        }
    }

    [Theory]
    [InlineData("<p>Simple paragraph</p>", "<p>Simple paragraph</p>")]
    [InlineData("<div>Hello World</div>", "<div>Hello World</div>")]
    [InlineData("<span class=\"my-class\">Text</span>", "<span class=\"my-class\">Text</span>")]
    [InlineData("<strong>Bold text</strong>", "<strong>Bold text</strong>")]
    [InlineData("<em>Italic text</em>", "<em>Italic text</em>")]
    [InlineData("<ul><li>Item 1</li><li>Item 2</li></ul>", "<ul><li>Item 1</li><li>Item 2</li></ul>")]
    [InlineData("<h1>Heading</h1>", "<h1>Heading</h1>")]
    [InlineData("<div id=\"test-id\" title=\"Test Title\">Content</div>", "<div id=\"test-id\" title=\"Test Title\">Content</div>")]
    [InlineData("Plain text without tags", "Plain text without tags")]
    public void MarkupStringSanitized_Html_ValidContent(string input, string expected)
    {
        // Act
        var result = new MarkupStringSanitized(input, MarkupStringSanitized.Formats.Html, configuration: null);

        // Assert
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData("<script>alert('XSS')</script>")]
    [InlineData("<div onclick=\"alert('XSS')\">Click me</div>")]
    [InlineData("<iframe src=\"javascript:alert('XSS')\"></iframe>")]
    [InlineData("<img src=x onerror=\"alert('XSS')\">")]
    [InlineData("<a href=\"javascript:void(0)\">Link</a>")]
    public void MarkupStringSanitized_Html_UnsafeContent_ThrowsException(string input)
    {
        const string exceptionMessage = "The provided HTML content contains potentially unsafe content";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => new MarkupStringSanitized(input, MarkupStringSanitized.Formats.Html, configuration: null));

        Assert.Contains(exceptionMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("<script>alert('XSS')</script>", "")]
    public void MarkupStringSanitized_Html_UnsafeContent_NoException(string input, string expected)
    {
        // Act & Assert
        MarkupSanitizedOptions.ThrowOnUnsafe = false;
        var result = new MarkupStringSanitized(input, MarkupStringSanitized.Formats.Html, configuration: null).Value;
        MarkupSanitizedOptions.ThrowOnUnsafe = true;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void MarkupStringSanitized_Constructor_Invalid()
    {
        const string exceptionMessage = "Cannot create MarkupStringSanitized with explicitly un-sanitized value.";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => new MarkupStringSanitized("Any data", (MarkupStringSanitized.Formats)999, configuration: null));

        Assert.Contains(exceptionMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MarkupStringSanitized_ToString()
    {
        // Act & Assert
        var input = "<p>Simple paragraph</p>";
        var result = new MarkupStringSanitized(input, MarkupStringSanitized.Formats.Html, configuration: null);

        Assert.Equal(input, result.ToString());
    }

    [Fact]
    public void MarkupStringSanitized_MarkupString()
    {
        // Act & Assert
        var input = "<p>Simple paragraph</p>";
        var result = new MarkupStringSanitized(input, MarkupStringSanitized.Formats.Html, configuration: null);
        var markupString = result.Markup;

        Assert.Equal(input, markupString.Value);
    }

    [Theory]
    [InlineData("color: red")]
    [InlineData("font-size: 14px")]
    [InlineData("margin: 10px 20px 30px 40px")]
    [InlineData("width: 100%")]
    [InlineData("transition: 300ms")]
    [InlineData("transform: rotate(45deg)")]
    [InlineData("height: 50vh")]
    [InlineData("padding: 2em")]
    [InlineData("font-size: 1.5rem")]
    [InlineData("width: 80vw")]
    [InlineData("animation-duration: 0.5s")]
    [InlineData("rotate(0.25turn)")]
    [InlineData("#f635a67c::part(dialog) { width: 400px; max-width: calc(-48px + 100vw); }")]
    public void DefaultSanitizeInlineStyle_ValidStyles_ReturnsValue(string style)
    {
        // Act
        var result = MarkupStringSanitized.DefaultSanitizeInlineStyle(style);

        // Assert
        Assert.Equal(style, result);
    }

    [Theory]
    [InlineData("behavior: url(script.htc)")]
    [InlineData("expression(alert('XSS'))")]
    [InlineData("-moz-binding: url(xss.xml#xss)")]
    [InlineData("javascript: alert('XSS')")]
    [InlineData("url(javascript: alert('XSS'))")]
    [InlineData("color: red; background: url(javascript:alert('XSS'))")]
    public void DefaultSanitizeInlineStyle_UnsafeStyles_ThrowsException(string style)
    {
        const string exceptionMessage = "The provided CSS inline style contains potentially unsafe content";

        // Act & Assert
        MarkupSanitizedOptions.ThrowOnUnsafe = true;
        try
        {
            var exception = Assert.Throws<InvalidOperationException>(() => MarkupStringSanitized.DefaultSanitizeInlineStyle(style));
            Assert.Contains(exceptionMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            MarkupSanitizedOptions.ThrowOnUnsafe = true;
        }
    }

    [Theory]
    [InlineData("behavior: url(script.htc)")]
    [InlineData("expression(alert('XSS'))")]
    [InlineData("javascript: alert('XSS')")]
    public void DefaultSanitizeInlineStyle_UnsafeStyles_NoThrow_ReturnsEmpty(string style)
    {
        // Act
        MarkupSanitizedOptions.ThrowOnUnsafe = false;
        try
        {
            var result = MarkupStringSanitized.DefaultSanitizeInlineStyle(style);

            // Assert
            Assert.Equal(string.Empty, result);
        }
        finally
        {
            MarkupSanitizedOptions.ThrowOnUnsafe = true;
        }
    }

    [Fact]
    public void SanitizeInlineStyle_CustomFunction_IsUsedDuringSanitization()
    {
        // Arrange - custom sanitizer that uppercases all values
        var config = new LibraryConfiguration();
        config.MarkupSanitized.SanitizeInlineStyle = value => value.ToUpperInvariant();

        // Act
        var result = new MarkupStringSanitized("<div style='color: red'>bold</div>", config);

        // Assert
        Assert.Equal("<div style='COLOR: RED'>BOLD</div>", result.Value);
    }

    [Theory]
    [InlineData("Plain text without tags")]
    [InlineData("<p>Simple paragraph</p>")]
    [InlineData("<div>Hello World</div>")]
    [InlineData("<span>Inline text</span>")]
    [InlineData("<strong>Bold</strong>")]
    [InlineData("<em>Italic</em>")]
    [InlineData("<b>Bold</b>")]
    [InlineData("<i>Italic</i>")]
    [InlineData("<u>Underline</u>")]
    [InlineData("<ul><li>Item 1</li><li>Item 2</li></ul>")]
    [InlineData("<ol><li>First</li><li>Second</li></ol>")]
    [InlineData("<h1>Heading 1</h1>")]
    [InlineData("<h6>Heading 6</h6>")]
    [InlineData("<br>")]
    [InlineData("<br/>")]
    [InlineData("<hr>")]
    [InlineData("<hr/>")]
    [InlineData("<div class=\"my-class\">Content</div>")]
    [InlineData("<span id=\"my-id\">Content</span>")]
    [InlineData("<p title=\"tooltip\">Content</p>")]
    [InlineData("<div data-custom=\"value\">Content</div>")]
    public void DefaultSanitizeHtml_ValidContent_ReturnsValue(string html)
    {
        // Act
        var result = MarkupStringSanitized.DefaultSanitizeHtml(html);

        // Assert
        Assert.Equal(html, result);
    }

    [Theory]
    [InlineData("<script>alert('XSS')</script>")]
    [InlineData("<div onclick=\"alert('XSS')\">Click me</div>")]
    [InlineData("<iframe src=\"evil.html\"></iframe>")]
    [InlineData("<img src=x onerror=\"alert('XSS')\">")]
    [InlineData("<a href=\"javascript:void(0)\">Link</a>")]
    [InlineData("<object data=\"evil.swf\"></object>")]
    [InlineData("<embed src=\"evil.swf\">")]
    public void DefaultSanitizeHtml_UnsafeContent_ThrowsException(string html)
    {
        const string exceptionMessage = "The provided HTML content contains potentially unsafe content";

        // Act & Assert
        MarkupSanitizedOptions.ThrowOnUnsafe = true;
        try
        {
            var exception = Assert.Throws<InvalidOperationException>(() => MarkupStringSanitized.DefaultSanitizeHtml(html));
            Assert.Contains(exceptionMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            MarkupSanitizedOptions.ThrowOnUnsafe = true;
        }
    }

    [Theory]
    [InlineData("<script>alert('XSS')</script>")]
    [InlineData("<iframe src=\"evil.html\"></iframe>")]
    [InlineData("<img src=x onerror=\"alert('XSS')\">")]
    public void DefaultSanitizeHtml_UnsafeContent_NoThrow_ReturnsEmpty(string html)
    {
        // Act
        MarkupSanitizedOptions.ThrowOnUnsafe = false;
        try
        {
            var result = MarkupStringSanitized.DefaultSanitizeHtml(html);

            // Assert
            Assert.Equal(string.Empty, result);
        }
        finally
        {
            MarkupSanitizedOptions.ThrowOnUnsafe = true;
        }
    }

    [Fact]
    public void DefaultSanitizeHtml_CustomFunction_IsUsedDuringSanitization()
    {
        // Arrange - custom sanitizer that uppercases all values
        var config = new LibraryConfiguration();
        config.MarkupSanitized.DefaultSanitizeHtml = value => value.ToUpperInvariant();

        // Act
        var result = new MarkupStringSanitized("<p>hello world</p>", MarkupStringSanitized.Formats.Html, config);

        // Assert
        Assert.Equal("<P>HELLO WORLD</P>", result.Value);
    }
}
