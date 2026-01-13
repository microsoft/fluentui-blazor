// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Forms;

public class FluentValidationSummaryTests : Verify.FluentUITestContext
{
    [Fact]
    public async Task FluentValidationSummary_Renders_Validation_Messages()
    {
        // Arrange
        var model = new TestModel();
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);

        messageStore.Add(editContext.Field(nameof(TestModel.Name)), "Name is required");

        // Act
        var cut = Render<FluentValidationSummary>(parameters => parameters
            .AddCascadingValue(editContext)
        );

        // Assert
        var messages = cut.FindAll(".fluent-validation-message");
        Assert.Single(messages);
        Assert.Contains("Name is required", messages[0].InnerHtml);
    }

    [Fact]
    public async Task FluentValidationSummary_Filters_By_Model()
    {
        // Arrange
        var model1 = new TestModel();
        var model2 = new TestModel();
        var editContext = new EditContext(model1);
        var messageStore = new ValidationMessageStore(editContext);

        messageStore.Add(new FieldIdentifier(model1, string.Empty), "Model1 error");
        messageStore.Add(new FieldIdentifier(model2, string.Empty), "Model2 error");

        // Act
        var cut = Render<FluentValidationSummary>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.Model, model1)
        );

        // Assert
        var messages = cut.FindAll(".fluent-validation-message");
        Assert.Single(messages);
        Assert.Contains("Model1 error", messages[0].InnerHtml);
    }

    [Fact]
    public async Task FluentValidationSummary_Updates_When_Validation_State_Changes()
    {
        // Arrange
        var model = new TestModel();
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);

        var cut = Render<FluentValidationSummary>(parameters => parameters
            .AddCascadingValue(editContext)
        );

        // Act
        var messages = cut.FindAll(".fluent-validation-message");
        Assert.Empty(messages);

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(editContext.Field(nameof(TestModel.Name)), "Name is invalid");
            editContext.NotifyValidationStateChanged();
        });

        // Assert
        messages = cut.FindAll(".fluent-validation-message");
        Assert.Single(messages);
        Assert.Contains("Name is invalid", messages[0].InnerHtml);
    }

    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
