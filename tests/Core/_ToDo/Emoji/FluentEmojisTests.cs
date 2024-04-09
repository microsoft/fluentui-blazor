using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Emojis;
public class FluentEmojiTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentEmojis_Default()
    {
        //Arrange
        string slot = default!;
        string title = default!;
        string width = default!;
        Emoji value = default!;
        Action<MouseEventArgs> onClick = _ => { };
        var cut = TestContext.RenderComponent<FluentEmoji<Emoji>>(parameters => parameters
            .Add(p => p.Slot, slot)
            .Add(p => p.Title, title)
            .Add(p => p.Width, width)
            .Add(p => p.Value, value)
            .Add(p => p.OnClick, onClick)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

