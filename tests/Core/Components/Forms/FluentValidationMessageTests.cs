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
        void cut() => Render<FluentValidationMessage<string>>(parameters => parameters
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
        void cut() => Render<FluentValidationMessage<string>>(parameters => parameters
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
        var message = cut.Find(".fluent-validation-message");
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
        Assert.Empty(cut.FindAll(".fluent-validation-message"));
    }

    [Fact]
    public async Task FluentValidationMessage_Renders_No_Icon_When_Null()
    {
        // Arrange
        var model = new TestModel { Name = "" };
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);

        // Act
        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.For, () => model.Name)
            .Add(p => p.Icon, null)
        );

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(editContext.Field(nameof(TestModel.Name)), "Error");
            editContext.NotifyValidationStateChanged();
        });

        // Assert
        Assert.Contains("Error", cut.Markup);
        Assert.Empty(cut.FindAll("svg"));
    }

    [Fact]
    public async Task FluentValidationMessage_Uses_Field_Parameter()
    {
        // Arrange
        var model = new TestModel { Description = "" };
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);
        var fieldIdentifier = new FieldIdentifier(model, nameof(TestModel.Description));

        // Act
        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.Field, fieldIdentifier)
        );

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(fieldIdentifier, "Description is required");
            editContext.NotifyValidationStateChanged();
        });

        // Assert
        Assert.Contains("Description is required", cut.Markup);
    }

    [Fact]
    public async Task FluentValidationMessage_Field_Takes_Precedence_Over_For()
    {
        // Arrange
        var model = new TestModel { Name = "", Description = "" };
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);
        var nameField = new FieldIdentifier(model, nameof(TestModel.Name));
        var descriptionField = new FieldIdentifier(model, nameof(TestModel.Description));

        // Act
        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.For, () => model.Name)
            .Add(p => p.Field, descriptionField)
        );

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(nameField, "Name error");
            messageStore.Add(descriptionField, "Description error");
            editContext.NotifyValidationStateChanged();
        });

        // Assert
        Assert.Contains("Description error", cut.Markup);
        Assert.DoesNotContain("Name error", cut.Markup);
    }

    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
