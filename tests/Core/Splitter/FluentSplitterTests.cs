using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Splitter;
public partial class FluentSplitterTests : TestContext
{
    private TestContext TestContext => new(); // TODO: To remove and to use the `RenderComponent` inherited method.

    [Fact]
    public void FluentSplitter_Default()
    {
        //Arrange
        var panel1 = "<b>render me</b>";
        var panel2 = "<b>render me</b>";
        Orientation orientation = default!;
        string panel1Size = default!;
        string panel2Size = default!;
        var cut = TestContext.RenderComponent<FluentSplitter>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .Add(p => p.Panel1, panel1)
            .Add(p => p.Panel2, panel2)
            .Add(p => p.Panel1Size, panel1Size)
            .Add(p => p.Panel2Size, panel2Size)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

