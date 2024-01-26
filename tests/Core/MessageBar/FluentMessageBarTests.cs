using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.MessageBar;

public class FluentMessageBarTests : TestBase
{
    private const string FluentAnchorRazorJs = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Anchor/FluentAnchor.razor.js";

    public FluentMessageBarTests()
    {
        TestContext.JSInterop.SetupModule(FluentAnchorRazorJs);
    }

    [Fact]
    public void FluentMessageBar_Default()
    {
        TestContext.Services.AddFluentUIComponents();

        // Arrange
        var cut = TestContext.RenderComponent<FluentMessageBar>(parameters =>
        {
            parameters.Add(p => p.Title, "This is a message");
        });

        // Assert
        cut.Verify();
    }

    //[Fact]
    //public void FluentMessageBar_Body_Link()
    //{
    //    // Arrange
    //    var content = new Message();
    //    var cut = TestContext.RenderComponent<FluentMessageBar>(parameters =>
    //    {
    //        content.Body = "This is a message body";
    //        content.I
    //        content.Link = new MessageAction()
    //        {
    //            Href = "https://fast.design",
    //            Text = "Learn more"
    //        };

    //        parameters.ShowMessageBar(p => p.Title, "This is a message");
    //        parameters.ShowMessageBar(p => p.Content, content);

    //    });

    //    // Assert
    //    cut.Verify();
    //}
}
