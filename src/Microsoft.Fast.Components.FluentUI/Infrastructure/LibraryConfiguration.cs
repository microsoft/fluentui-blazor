namespace Microsoft.Fast.Components.FluentUI;

// If needed, additional services configuration objects can be added here

/// <summary>
/// Defines the global Fluent UI Web Components for Blazor library services configuration
/// </summary>
public class LibraryConfiguration
{
    public BlazorHostingModel HostingModel { get; set; } = BlazorHostingModel.NotSpecified;
    public StaticAssetServiceConfiguration StaticAssetServiceConfiguration { get; set; } = new();

    public IconConfiguration IconConfiguration { get; set; }
    public EmojiConfiguration EmojiConfiguration { get; set; }

    public LibraryConfiguration()
    {
        IconConfiguration = new(true);
        EmojiConfiguration = new(false);
    }
    
    public LibraryConfiguration(IconConfiguration iconConfiguration, EmojiConfiguration emojiConfiguration)
    {
        IconConfiguration = iconConfiguration;
        EmojiConfiguration = emojiConfiguration;
    }
    
}
