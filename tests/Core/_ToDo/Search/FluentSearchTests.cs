using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Search;
public class FluentSearchTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentSearch_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        string dataList = default!;
        int? maxlength = default!;
        int? minlength = default!;
        string pattern = default!;
        int? size = default!;
        bool? spellcheck = default!;
        FluentInputAppearance appearance = default!;
        var cut = TestContext.RenderComponent<FluentSearch>(parameters => parameters
            .Add(p => p.DataList, dataList)
            .Add(p => p.Maxlength, maxlength)
            .Add(p => p.Minlength, minlength)
            .Add(p => p.Pattern, pattern)
            .Add(p => p.Size, size)
            .Add(p => p.Spellcheck, spellcheck)
            .Add(p => p.Appearance, appearance)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

