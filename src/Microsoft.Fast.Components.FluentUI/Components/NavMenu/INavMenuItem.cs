namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuItem
{
    string? Id { get; }
    string? Href { get; }
    bool HasIcon { get; }
}
