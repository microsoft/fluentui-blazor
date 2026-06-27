// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoNewsBar
{
    private const string LocalStorageKey = "demo-newsbar-sha";

    private static readonly Uri NewsUri = new("https://raw.githubusercontent.com/microsoft/fluentui-blazor/refs/heads/dev-v5/LICENSE.TXT");

    private string NewsTitle { get; set; } = "News";

    private MessageBarIntent NewsIntent { get; set; } = MessageBarIntent.Info;

    private string? NewsContent { get; set; }

    private string? NewsSha { get; set; }

    private bool Visible { get; set; }

    [Inject]
    public required HttpClient HttpClient { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        // Read the news content.
        NewsContent = await HttpClient.GetStringAsync(NewsUri);

        if (string.IsNullOrWhiteSpace(NewsContent))
        {
            Visible = false;
            return;
        }

        // Compute a small SHA of this content.
        NewsSha = ComputeSha(NewsContent);

        // If the stored SHA matches the current content, the user already
        // read this message, so the message bar stays hidden.
        var storedSha = await JSRuntime.InvokeAsync<string?>("localStorage.getItem", LocalStorageKey);
        Visible = !string.Equals(storedSha, NewsSha, StringComparison.Ordinal);

        StateHasChanged();

        // When the content is set and the message bar is visible, apply the
        // notification style override defined in the razor script.
        if (Visible)
        {
            await JSRuntime.InvokeVoidAsync("applyDemoNotificationStyle");
        }
    }

    private async Task DismissClickAsync()
    {
        // Save the SHA of the read message into the user local storage.
        if (NewsSha is not null)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, NewsSha);
        }

        Visible = false;
    }

    private static string ComputeSha(string content)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(content));

        // Keep a small SHA: the first 8 bytes are enough to detect content changes.
        return Convert.ToHexString(hash, 0, 8).ToLowerInvariant();
    }
}