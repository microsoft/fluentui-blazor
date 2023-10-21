using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.TreeView;
public class FluentTreeItemTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentTreeItem_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        string text = default!;
        bool expanded = default!;
        Action<System.Boolean> expandedChanged = _ => { };
        bool selected = default!;
        Action<System.Boolean> selectedChanged = _ => { };
        bool disabled = default!;
        bool initiallyExpanded = default!;
        bool initiallySelected = default!;
        var cut = TestContext.RenderComponent<FluentTreeItem>(parameters => parameters
            .Add(p => p.Text, text)
            .Add(p => p.Expanded, expanded)
            .Add(p => p.ExpandedChanged, expandedChanged)
            .Add(p => p.Selected, selected)
            .Add(p => p.SelectedChanged, selectedChanged)
            .Add(p => p.Disabled, disabled)
            .AddChildContent(childContent)
            .Add(p => p.InitiallyExpanded, initiallyExpanded)
            .Add(p => p.InitiallySelected, initiallySelected)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






