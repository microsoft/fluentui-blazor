// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Forms;

public class FluentValidationMessageTests : Verify.FluentUITestContext
{
    [Fact]
    public void FluentValidationMessage_Throws_When_No_EditContext()
    {
        // Arrange & Act
        Action cut = () => Render<FluentValidationMessage<string>>(parameters => parameters
            .Add(p => p.For, () => "test")
        );

        // Assert
        Assert.Throws<InvalidOperationException>(cut);
    }

    [Fact]
    public void FluentValidationMessage_Throws_When_No_Field_Or_For()
    {
        // Arrange
        var model = new TestModel();
        var editContext = new EditContext(model);

        // Act
        Action cut = () => Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
        );

        // Assert
        Assert.Throws<InvalidOperationException>(cut);
    }

    [Fact]
    public async Task FluentValidationMessage_Renders_Validation_Messages()
    {
        // Arrange
        var model = new TestModel { Name = "" };
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);

        // Act
        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.For, () => model.Name)
        );

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(editContext.Field(nameof(TestModel.Name)), "Name is required");
            editContext.NotifyValidationStateChanged();
        });

        // Assert
        var message = cut.Find(".validation-message");
        Assert.NotNull(message);
        Assert.Contains("Name is required", message.InnerHtml);
        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public async Task FluentValidationMessage_Updates_When_Validation_State_Changes()
    {
        // Arrange
        var model = new TestModel { Name = "Initial" };
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);

        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.For, () => model.Name)
        );

        // Act
        await cut.InvokeAsync(() =>
        {
            messageStore.Add(editContext.Field(nameof(TestModel.Name)), "Name is invalid");
            editContext.NotifyValidationStateChanged();
        });

        // Assert
        Assert.Contains("Name is invalid", cut.Markup);

        // Act: Clear messages
        await cut.InvokeAsync(() =>
        {
            messageStore.Clear();
            editContext.NotifyValidationStateChanged();
        });

        // Assert: No messages should be rendered
        Assert.DoesNotContain("Name is invalid", cut.Markup);
        Assert.Empty(cut.FindAll(".validation-message"));
    }

    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
