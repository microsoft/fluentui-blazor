using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Menu;
public class FluentMenuItemTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentMenuItem_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        string menuItems = "<b>render me</b>";
        string label = default!;
        bool disabled = default!;
        bool expanded = default!;
        Microsoft.Fast.Components.FluentUI.MenuItemRole? role = default!;
        bool Checked = default!;
        Action onClick = () => { };
        Microsoft.Fast.Components.FluentUI.FluentMenu owner = default!;
        var cut = TestContext.RenderComponent<FluentMenuItem>(parameters => parameters
            .Add(p => p.Label, label)
            .Add(p => p.Disabled, disabled)
            .Add(p => p.Expanded, expanded)
            .Add(p => p.Role, role)
            .Add(p => p.Checked, Checked)
            .AddChildContent(childContent)
            .Add(p => p.MenuItems, menuItems)
            .Add(p => p.OnClick, onClick)
            .Add(p => p.Owner, owner)
        );
        //Act

        //Assert
		cut.Verify();
    }
}