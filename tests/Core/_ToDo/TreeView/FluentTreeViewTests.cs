using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.TreeView;
public class FluentTreeViewTests : TestBase
{
    [Fact]
    public void FluentTreeView_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        bool renderCollapsedNodes = default!;
        Microsoft.Fast.Components.FluentUI.FluentTreeItem currentSelected = default!;
        Action<Microsoft.Fast.Components.FluentUI.FluentTreeItem> currentSelectedChanged = _ => { };
        Action<Microsoft.Fast.Components.FluentUI.FluentTreeItem> onSelectedChange = _ => { };
        Action<Microsoft.Fast.Components.FluentUI.FluentTreeItem> onExpandedChange = _ => { };
        var cut = TestContext.RenderComponent<FluentTreeView>(parameters => parameters
            .Add(p => p.RenderCollapsedNodes, renderCollapsedNodes)
            .Add(p => p.CurrentSelected, currentSelected)
            .Add(p => p.CurrentSelectedChanged, currentSelectedChanged)
            .AddChildContent(childContent)
            .Add(p => p.OnSelectedChange, onSelectedChange)
            .Add(p => p.OnExpandedChange, onExpandedChange)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






