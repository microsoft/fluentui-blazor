// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Forms;

public class FluentValidationMessageTests : Verify.FluentUITestContext
{
    [Fact]
    public void FluentValidationMessage_Throws_When_No_EditContext()
    {
        void RenderMessage() => Render<FluentValidationMessage<string>>(parameters => parameters
            .Add(p => p.For, () => "value"));

        Assert.Throws<InvalidOperationException>(RenderMessage);
    }

    [Fact]
    public void FluentValidationMessage_Throws_When_No_Field_Or_For()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        void RenderMessage() => Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext));

        Assert.Throws<InvalidOperationException>(RenderMessage);
    }

    [Fact]
    public async Task FluentValidationMessage_Renders_Validation_Messages()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);

        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.For, () => model.Name));

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(editContext.Field(nameof(TestModel.Name)), "Name is required");
            editContext.NotifyValidationStateChanged();
        });

        var message = cut.Find(".fluent-validation-message");
        Assert.NotNull(message);
        Assert.Contains("Name is required", message.InnerHtml);
        Assert.Contains("slot=\"message\"", cut.Markup);
    }

    [Fact]
    public async Task FluentValidationMessage_Uses_Field_Parameter()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);
        var messageStore = new ValidationMessageStore(editContext);
        var fieldIdentifier = new FieldIdentifier(model, nameof(TestModel.Description));

        var cut = Render<FluentValidationMessage<string>>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.Field, fieldIdentifier));

        await cut.InvokeAsync(() =>
        {
            messageStore.Add(fieldIdentifier, "Description is required");
            editContext.NotifyValidationStateChanged();
        });

        Assert.Contains("Description is required", cut.Markup);
    }

    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
