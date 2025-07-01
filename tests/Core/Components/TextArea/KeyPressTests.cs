// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.TextArea;

public class FluentTextArea_KeyPress
{
    [Fact]
    public void FluentTextArea_KeyPress_Combined_True()
    {
        var keys = KeyPress.For(AspNetCore.Components.KeyCode.KeyA)
                           .AndAltKey()
                           .AndCtrlKey()
                           .AndMetaKey()
                           .AndShiftKey()
                           .WithPreventDefault();

        Assert.True(keys.AltKey);
        Assert.True(keys.CtrlKey);
        Assert.True(keys.ShiftKey);
        Assert.True(keys.MetaKey);
        Assert.False(keys.PreventDefault);
    }

    [Fact]
    public void FluentTextArea_KeyPress_Combined_False()
    {
        var keys = KeyPress.For(AspNetCore.Components.KeyCode.KeyA)
                           .AndAltKey(pressed: false)
                           .AndCtrlKey(pressed: false)
                           .AndMetaKey(pressed: false)
                           .AndShiftKey(pressed: false)
                           .WithPreventDefault(prevent: true);

        Assert.False(keys.AltKey);
        Assert.False(keys.CtrlKey);
        Assert.False(keys.ShiftKey);
        Assert.False(keys.MetaKey);
        Assert.True(keys.PreventDefault);
    }

    [Fact]
    public void FluentTextArea_KeyPress_MultipleCombined()
    {
        var keys = KeyPress.For(AspNetCore.Components.KeyCode.KeyA)
                           .AndAltKey(pressed: true).AndAltKey(pressed: false)
                           .AndCtrlKey(pressed: true).AndCtrlKey(pressed: false).AndCtrlKey(pressed: true)
                           .AndMetaKey(pressed: true).AndMetaKey(pressed: false)
                           .AndShiftKey(pressed: true).AndShiftKey(pressed: false).AndShiftKey(pressed: true)
                           .WithPreventDefault().WithPreventDefault(prevent: true);

        Assert.False(keys.AltKey);
        Assert.True(keys.CtrlKey);
        Assert.False(keys.MetaKey);
        Assert.True(keys.ShiftKey);
        Assert.True(keys.PreventDefault);
    }
}
