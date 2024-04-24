using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Drag;

public class FluentDragTests : TestBase
{
    [Fact]
    public void FluentDrag_SimpleTest()
    {
        // Arrange
        using var ctx = new TestContext();

        // Act
        var cut = ctx.RenderComponent<FluentDragContainer<int>>(parameters =>
        {
            parameters.Add(p => p.OnDragStart, (e) => { });
            parameters.Add(p => p.OnDragEnter, (e) => { });
            parameters.Add(p => p.OnDragOver, (e) => { });
            parameters.Add(p => p.OnDragLeave, (e) => { });
            parameters.Add(p => p.OnDropEnd, (e) => { });

            parameters.AddChildContent<FluentDropZone<int>>(zone =>
            {
                zone.Add(p => p.Draggable, true);
                zone.Add(p => p.Item, 1);
                zone.AddChildContent("Item 1");

                zone.Add(p => p.OnDragStart, (e) => { });
                zone.Add(p => p.OnDragEnter, (e) => { });
                zone.Add(p => p.OnDragOver, (e) => { });
                zone.Add(p => p.OnDragLeave, (e) => { });
                zone.Add(p => p.OnDropEnd, (e) => { });
            });

            parameters.AddChildContent<FluentDropZone<int>>(zone =>
            {
                zone.Add(p => p.Droppable, true);
                zone.Add(p => p.Item, 2);
                zone.AddChildContent("Item 2");

                zone.Add(p => p.OnDragStart, (e) => { });
                zone.Add(p => p.OnDragEnter, (e) => { });
                zone.Add(p => p.OnDragOver, (e) => { });
                zone.Add(p => p.OnDragLeave, (e) => { });
                zone.Add(p => p.OnDropEnd, (e) => { });
            });
        });

        // Assert
        cut.Verify();
    }
}
