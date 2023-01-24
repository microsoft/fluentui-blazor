namespace Microsoft.Fast.Components.FluentUI;

public class EmojiConfiguration
{
     private EmojiGroup[] _groups = new[]
    {
        EmojiGroup.Activities,
        EmojiGroup.Animals_Nature,
        EmojiGroup.Flags,
        EmojiGroup.Food_Drink,
        EmojiGroup.Objects,
        EmojiGroup.People_Body,
        EmojiGroup.Smileys_Emotion,
        EmojiGroup.Symbols,
        EmojiGroup.Travel_Places,
    };

    private EmojiStyle[] _styles = new[]
    {
        EmojiStyle.Color,
        EmojiStyle.Flat,
        EmojiStyle.HighContrast
    };

    internal event Action? OnUpdate;

    public EmojiGroup[] Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            OnUpdate?.Invoke();
        }
    }


    public EmojiStyle[] Styles
    {
        get => _styles; 
        set 
        {
            _styles = value;
            OnUpdate?.Invoke();
        }
    }

}
