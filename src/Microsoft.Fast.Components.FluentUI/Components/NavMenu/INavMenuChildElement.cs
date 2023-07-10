namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuChildElement
{
    string? Id { get; }
    bool HasIcon { get; }

    Task SetSelectedAsync(bool selected, bool forceChangedEvent);
}
