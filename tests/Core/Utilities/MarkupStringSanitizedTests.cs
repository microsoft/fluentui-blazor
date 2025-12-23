// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public void MarkupStringSanitized_Constructor_ThrowsException(string input, string xss)
    {
        const string exceptionMessage = "The provided CSS inline style contains potentially unsafe content";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => new MarkupStringSanitized(input.Replace("###", xss), null));

        Assert.Contains(exceptionMessage, exception.Message, StringComparison.OrdinalIgnoreCase);
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
}
