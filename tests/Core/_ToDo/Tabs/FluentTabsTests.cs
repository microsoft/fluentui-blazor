using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Tabs;
public class FluentTabsTests : TestBase
{
    public FluentTabsTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNeCore.Components/Components/Overflow/FluentOverflow.razor.js");
    }

    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentTabs_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        Orientation orientation = default!;
        Action<FluentTab> onTabSelect = _ => { };
        Action<FluentTab> onTabClose = _ => { };
        bool showClose = default!;
        TabSize? size = default!;
        string width = default!;
        string height = default!;
        string activeTabId = default!;
        Action<string> activeTabIdChanged = _ => { };
        bool showActiveIndicator = default!;
        Action<FluentTab> onTabChange = _ => { };
        var cut = TestContext.RenderComponent<FluentTabs>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .Add(p => p.OnTabSelect, onTabSelect)
            .Add(p => p.OnTabClose, onTabClose)
            .Add(p => p.ShowClose, showClose)
            .Add(p => p.Size, size)
            .Add(p => p.Width, width)
            .Add(p => p.Height, height)
            .Add(p => p.ActiveTabId, activeTabId)
            .Add(p => p.ActiveTabIdChanged, activeTabIdChanged)
            .Add(p => p.ShowActiveIndicator, showActiveIndicator)
            .AddChildContent(childContent)
            .Add(p => p.OnTabChange, onTabChange)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

