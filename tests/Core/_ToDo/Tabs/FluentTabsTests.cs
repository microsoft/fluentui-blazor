using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Tabs;
public class FluentTabsTests : TestBase
{
    public FluentTabsTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.Fast.Components.FluentUI/Components/Overflow/FluentOverflow.razor.js");
    }

    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentTabs_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.Orientation orientation = default!;
        Action<Microsoft.Fast.Components.FluentUI.FluentTab> onTabSelect = _ => { };
        Action<Microsoft.Fast.Components.FluentUI.FluentTab> onTabClose = _ => { };
        bool showClose = default!;
        Microsoft.Fast.Components.FluentUI.TabSize? size = default!;
        string width = default!;
        string height = default!;
        string activeTabId = default!;
        Action<System.String> activeTabIdChanged = _ => { };
        bool showActiveIndicator = default!;
        Action<Microsoft.Fast.Components.FluentUI.FluentTab> onTabChange = _ => { };
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






