using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Tabs;
public class FluentTabTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentTab_Default()
    {
        //Arrange
        var header = "<b>render me</b>";
        var content = "<b>render me</b>";
        var childContent = "<b>render me</b>";
        bool disabled = default!;
        string label = default!;
        Action<string> labelChanged = _ => { };
        string labelClass = default!;
        string labelStyle = default!;
        Icon icon = default!;
        bool labelEditable = default!;
        bool deferredLoading = default!;
        FluentTabs owner = default!;
        var cut = TestContext.RenderComponent<FluentTab>(parameters => parameters
            .Add(p => p.Disabled, disabled)
            .Add(p => p.Label, label)
            .Add(p => p.LabelChanged, labelChanged)
            .Add(p => p.LabelClass, labelClass)
            .Add(p => p.LabelStyle, labelStyle)
            .Add(p => p.Header, header)
            .Add(p => p.Icon, icon)
            .Add(p => p.LabelEditable, labelEditable)
            .Add(p => p.DeferredLoading, deferredLoading)
            .Add(p => p.Content, content)
            .AddChildContent(childContent)
            .Add(p => p.Owner, owner)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

