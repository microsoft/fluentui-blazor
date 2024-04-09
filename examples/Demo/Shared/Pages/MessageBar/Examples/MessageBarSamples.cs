
namespace FluentUI.Demo.Shared.Pages.MessageBar.Examples;

/// <summary />
public class MessageBarSamples
{
    public static string[] Messages => new[]
    {
        "The first five days after the weekend are the toughest.",
        "The leading source of computer problems is computer solutions.",
        "The only scenario where you really need a landline today is when you're trying to find your cell phone.",
        "The perfect man doesn't swear, doesn't smoke, doesn't get angry, doesn't drink. He also doesn't exist.",
        "The road to success is always under construction.",
        "The shortest horror story: Monday.",
        "The snorers are always the ones to fall asleep first.",
        "There is no lousy weather, only lousy choice of clothing.",
        "Whenever I'm sad, I stop being sad and be awesome instead.",
        "It's just, eventually we're all gonna move on. It's called growing up.",
        "Whether a gesture's charming or alarming depends on how it's received.",
    };

    public static string OneRandomMessage =>
            Messages.OrderBy(i => Guid.NewGuid()).First();
}
