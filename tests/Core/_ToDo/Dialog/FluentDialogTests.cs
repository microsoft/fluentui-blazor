using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Dialog;
public class FluentDialogTests : TestBase
{
    [Fact]
    public void FluentDialog_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        bool preventScroll = default!;
        bool? modal = default!;
        bool hidden = default!;
        Action<System.Boolean> hiddenChanged = _ => { };
        bool? trapFocus = default!;
        string ariaDescribedby = default!;
        string ariaLabelledby = default!;
        string ariaLabel = default!;
        DialogInstance instance = default!;
        Action<DialogResult> onDialogResult = _ => { };
        
        var cut = TestContext.RenderComponent<FluentDialog>(parameters => parameters
            .Add(p => p.PreventScroll, preventScroll)
            .Add(p => p.Modal, modal)
            .Add(p => p.Hidden, hidden)
            .Add(p => p.HiddenChanged, hiddenChanged)
            .Add(p => p.TrapFocus, trapFocus)
            .Add(p => p.AriaDescribedby, ariaDescribedby)
            .Add(p => p.AriaLabelledby, ariaLabelledby)
            .Add(p => p.AriaLabel, ariaLabel)
            .Add(p => p.Instance, instance)
            .AddChildContent(childContent)
            .Add(p => p.OnDialogResult, onDialogResult)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






