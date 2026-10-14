// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.Dialog.Examples.Dialog;

public class CustomSplashScreenState
{
    public string? Title { get; set; }

    public string? SubTitle { get; set; }

    public string? LoadingText { get; set; }

    public MarkupString? Message { get; set; }

    public string? Logo { get; set; }

    public int DisplayTime { get; set; } = 4000;

    public Action? RefreshProperties { get; set; }

    public void UpdateLabels(string? loadingText = null, MarkupString? message = null)
    {
        if (loadingText is not null)
        {
            LoadingText = loadingText;
        }

        if (message is not null)
        {
            Message = message;
        }

        RefreshProperties?.Invoke();
    }
}
