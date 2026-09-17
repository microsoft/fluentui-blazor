// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog;

public class DialogServiceInternalTests : Bunit.BunitContext
{
    [Fact]
    public async Task RemoveDialogFromProviderAsync_WhenDialogIdDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents(options => options.UseGlobalOverlay = false);

        var dialogService = (DialogService)Services.GetRequiredService<IDialogService>();

        // Render provider so provider Id is available and ProviderNotAvailable guard does not interfere.
        _ = Render<FluentDialogProvider>();

        // Create an instance that is intentionally NOT added to ServiceProvider.Items.
        var missing = new DialogInstance(dialogService, typeof(FluentDialogProvider), new DialogOptions { Id = "missing-id" });

        // Act + Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await dialogService.RemoveDialogFromProviderAsync(missing);
        });

        Assert.Contains("missing-id", ex.Message, StringComparison.Ordinal);
        Assert.Contains("doesn't exist", ex.Message, StringComparison.Ordinal);
    }
}
