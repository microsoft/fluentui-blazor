namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

internal interface IThemeStorageService : IAsyncDisposable
{
    ValueTask<bool> IsStorageEnabledAndSupported();
    ValueTask<string?> GetThemeAsync();
    ValueTask SetThemeAsync(StandardLuminance theme);
    ValueTask SetHighlightAsync(StandardLuminance highlight);
    ValueTask<string?> GetAccentColorAsync();
    ValueTask SetAccentColorAsync(string color);
    ValueTask<bool?> GetDirectionAsync();
    ValueTask ChangeDirection(bool isRTL);
    ValueTask SetDirectionAsync(bool isRTL);
}
