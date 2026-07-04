// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout;

/// <summary>
/// A component that displays a news message bar in the demo application.
/// </summary>
public partial class DemoNewsBar
{
    /*
        --------------------------------------------------------------------
        NEWS-BANNER.md
        --------------------------------------------------------------------

        ---
        Title: New release available
        Intent: Success
        ---
        Version RC4 is now available with many new components.
        Check out the changelog for all the details and breaking changes.
    */

    /// <summary>
    /// The URI of the news content to display in the message bar.
    /// </summary>
    private static readonly Uri NewsUri = new("https://raw.githubusercontent.com/microsoft/fluentui-blazor/refs/heads/dev-v5/NEWS-BANNER.md");

    private const string LocalStorageKey = "fluentui-demo-newsbar-sha";

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
        var raw = "";

        try
        {
            raw = await HttpClient.GetStringAsync(NewsUri);
        }
        catch (HttpRequestException)
        {
            Console.WriteLine($"DemoNewsBar: Failed to read the news content from {NewsUri}");
        }

        if (string.IsNullOrWhiteSpace(raw))
        {
            Visible = false;
            return;
        }

        // Extract the Title, Intent and content from the front matter.
        ParseNewsBanner(raw);

        if (string.IsNullOrWhiteSpace(NewsContent))
        {
            Visible = false;
            return;
        }

        // Compute a small SHA of the whole file content.
        NewsSha = ComputeSha(raw);

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

        // Keep a small SHA: the first 16 bytes are enough to detect content changes.
        return Convert.ToHexString(hash, 0, 16).ToLowerInvariant();
    }

    /// <summary>
    /// Parses the news banner content using the front matter format:
    /// a metadata block delimited by "---" lines containing the Title and
    /// Intent, followed by the content body.
    /// </summary>
    /// <param name="raw">The raw file content to parse.</param>
    private void ParseNewsBanner(string raw)
    {
        var lines = raw.Replace("\r\n", "\n").Split('\n');
        var index = 0;

        // The front matter must start with a "---" delimiter line.
        if (index < lines.Length && lines[index].Trim() == "---")
        {
            index++;

            // Read the metadata until the closing "---" delimiter line.
            while (index < lines.Length && lines[index].Trim() != "---")
            {
                var line = lines[index];
                var separator = line.IndexOf(':');

                if (separator > 0)
                {
                    var key = line[..separator].Trim();
                    var value = line[(separator + 1)..].Trim();

                    if (string.Equals(key, "Title", StringComparison.OrdinalIgnoreCase))
                    {
                        NewsTitle = value;
                    }
                    else if (string.Equals(key, "Intent", StringComparison.OrdinalIgnoreCase)
                        && Enum.TryParse<MessageBarIntent>(value, ignoreCase: true, out var intent))
                    {
                        NewsIntent = intent;
                    }
                }

                index++;
            }

            // Skip the closing "---" delimiter line.
            if (index < lines.Length)
            {
                index++;
            }
        }

        // The remaining lines are the content body.
        NewsContent = string.Join(Environment.NewLine, lines[index..]).Trim();
    }
}