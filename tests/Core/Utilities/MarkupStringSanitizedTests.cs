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
}
