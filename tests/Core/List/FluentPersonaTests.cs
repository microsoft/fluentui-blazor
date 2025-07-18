// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.List;

public class FluentPersonaTests : TestBase
{
    private const string SamplePicture = "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";

    public FluentPersonaTests()
    {
        TestContext.Services.AddScoped<GlobalState>();
    }

    [Fact]
    public void FluentPersona_Image()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentPersona>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Name, "Denis Voituron");
            parameters.Add(p => p.Image, $"data:image/png;base64, {SamplePicture}");
            parameters.Add(p => p.ImageSize, "32px");
            parameters.Add(p => p.DismissTitle, "Remove this people");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentPersona_Text_Position_Start()
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentPersona>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Name, "Denis Voituron");
            parameters.Add(p => p.Image, $"data:image/png;base64, {SamplePicture}");
            parameters.Add(p => p.ImageSize, "32px");
            parameters.Add(p => p.TextPosition, TextPosition.Start);
            parameters.Add(p => p.DismissTitle, "Remove this people");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData("OneSpace", "Denis Voituron")]
    [InlineData("MultipleSpaces", "Denis   Voituron")]
    [InlineData("MultipleWords", "Denis   Voituron  Senior")]
    [InlineData("OneName", "Denis")]
    [InlineData("NoName", "")]
    public void FluentPersona_Initials(string id, string name)
    {
        // Arrange
        var cut = TestContext.RenderComponent<FluentPersona>(parameters =>
        {
            parameters.Add(p => p.Id, "myComponent");
            parameters.Add(p => p.Name, name);
            parameters.Add(p => p.DismissTitle, "Remove this people");
        });

        // Assert
        cut.Verify(suffix: id);
    }
}
