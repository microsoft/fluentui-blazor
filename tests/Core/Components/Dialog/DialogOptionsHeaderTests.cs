// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog;

public class DialogOptionsHeaderTests
{
    [Fact]
    public void DialogOptionsHeader_DefaultValues()
    {
        // Arrange & Act
        var header = new DialogOptionsHeader();

        // Assert
        Assert.Null(header.Title);
        Assert.NotNull(header.CloseAction);
        Assert.NotNull(header.InfoAction);
        Assert.True(header.CloseAction.IsClosedAction);
        Assert.False(header.InfoAction.IsClosedAction);
        Assert.IsType<CoreIcons.Regular.Size20.Dismiss>(header.CloseAction.Icon);
        Assert.IsType<CoreIcons.Regular.Size20.Info>(header.InfoAction.Icon);

        // No actions are visible by default
        Assert.False(header.HasActions);
    }

    [Fact]
    public void DialogOptionsHeader_CloseAction_Visible()
    {
        // Arrange
        var header = new DialogOptionsHeader();

        // Act
        header.CloseAction.Visible = true;

        // Assert
        Assert.True(header.CloseAction.ToDisplay);
        Assert.True(header.HasActions);
    }

    [Fact]
    public void DialogOptionsHeader_InfoAction_Visible()
    {
        // Arrange
        var header = new DialogOptionsHeader();

        // Act
        header.InfoAction.Visible = true;

        // Assert
        Assert.True(header.InfoAction.ToDisplay);
        Assert.True(header.HasActions);
    }

    [Fact]
    public void DialogOptionsHeader_InfoAction_NoIconAndNoLabel_NotDisplayed()
    {
        // Arrange
        var header = new DialogOptionsHeader();
        header.InfoAction.Visible = true;
        header.InfoAction.Icon = null;
        header.InfoAction.Label = null;

        // Assert
        Assert.False(header.InfoAction.ToDisplay);
        Assert.False(header.HasActions);
    }

    [Fact]
    public void DialogOptionsHeader_AddAction_AppendsExtraAction()
    {
        // Arrange
        var header = new DialogOptionsHeader();

        // Act
        var action = header.AddAction(a =>
        {
            a.Label = "Help";
            a.Tooltip = "Get some help";
            a.Title = "Help";
        });

        // Assert
        Assert.NotNull(action);
        Assert.Equal("Help", action.Label);
        Assert.Equal("Get some help", action.Tooltip);
        Assert.Equal("Help", action.Title);
        Assert.True(action.Visible);
        Assert.True(action.ToDisplay);
        Assert.True(header.HasActions);
    }

    [Fact]
    public void DialogOptionsHeader_AddAction_NullOptions_Throws()
    {
        // Arrange
        var header = new DialogOptionsHeader();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => header.AddAction(null!));
    }

    [Fact]
    public void DialogOptionsHeader_GetActions_ReturnsExtrasThenInfoThenClose()
    {
        // Arrange
        var header = new DialogOptionsHeader();
        var first = header.AddAction(a => a.Label = "First");
        var second = header.AddAction(a => a.Label = "Second");

        // Act
        var actions = header.GetActions().ToList();

        // Assert
        Assert.Equal(4, actions.Count);
        Assert.Same(first, actions[0]);
        Assert.Same(second, actions[1]);
        Assert.Same(header.InfoAction, actions[2]);
        Assert.Same(header.CloseAction, actions[3]);
    }

    [Fact]
    public void DialogOptionsHeader_HasActions_TrueWhenExtraActionVisible()
    {
        // Arrange
        var header = new DialogOptionsHeader();
        header.AddAction(a => a.Label = "Custom");

        // Assert
        Assert.True(header.HasActions);
    }

    [Fact]
    public async Task DialogOptionsHeader_AddAction_OnClickAsync_IsInvoked()
    {
        // Arrange
        var header = new DialogOptionsHeader();
        var clicked = false;

        var action = header.AddAction(a =>
        {
            a.Label = "Run";
            a.OnClickAsync = _ =>
            {
                clicked = true;
                return Task.CompletedTask;
            };
        });

        // Act
        await action.OnClickAsync!(null!);

        // Assert
        Assert.True(clicked);
    }
}
