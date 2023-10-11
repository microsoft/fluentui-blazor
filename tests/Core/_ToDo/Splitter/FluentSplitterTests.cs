using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Splitter;
public class FluentSplitterTests : TestBase
{
    [Fact]
    public void FluentSplitter_Default()
    {
        //Arrange
        string panel1 = "<b>render me</b>";
        string panel2 = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.Orientation orientation = default!;
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






