using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared;


public record EmojiModel(string Name, string Folder, EmojiGroup Group, string Keywords, EmojiStyle? Style, EmojiSkintone? Skintone = null);
