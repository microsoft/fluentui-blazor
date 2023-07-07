namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuChildElement
{
    string? Id { get; }
    string? Href { get; }
    bool HasIcon { get; }
}
