namespace Microsoft.Fast.Components.FluentUI;

// If needed, additional services configuration objects can be added here

/// <summary>
/// Defines the global Fluent UI Blazor component library services configuration
/// </summary>
public class LibraryConfiguration
{
    public BlazorHostingModel HostingModel { get; set; } = BlazorHostingModel.NotSpecified;
    public StaticAssetServiceConfiguration StaticAssetServiceConfiguration { get; set; } = new();

    public IconConfiguration IconConfiguration { get; set; }
    public EmojiConfiguration EmojiConfiguration { get; set; }

    public ToastConfiguration ToastConfiguration { get; set; }

    public LibraryConfiguration()
    {
        IconConfiguration = new();
        EmojiConfiguration = new();
        ToastConfiguration = new();
    }
    
    public LibraryConfiguration(IconConfiguration iconConfiguration, EmojiConfiguration emojiConfiguration, ToastConfiguration? toastConfiguration = null)
    {
        IconConfiguration = iconConfiguration;
        EmojiConfiguration = emojiConfiguration;
        
        ToastConfiguration = toastConfiguration ?? new();
    }
    
}
