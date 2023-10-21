using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.TextField;
public class FluentTextFieldTests : TestBase
{
    [Fact]
    public void FluentTextField_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.TextFieldType? textFieldType = default!;
        string dataList = default!;
        int? maxlength = default!;
        int? minlength = default!;
        string pattern = default!;
        int? size = default!;
        bool? spellcheck = default!;
        Microsoft.Fast.Components.FluentUI.FluentInputAppearance appearance = default!;
        string autoComplete = default!;
        var cut = TestContext.RenderComponent<FluentTextField>(parameters => parameters
            .Add(p => p.TextFieldType, textFieldType)
            .Add(p => p.DataList, dataList)
            .Add(p => p.Maxlength, maxlength)
            .Add(p => p.Minlength, minlength)
            .Add(p => p.Pattern, pattern)
            .Add(p => p.Size, size)
            .Add(p => p.Spellcheck, spellcheck)
            .Add(p => p.Appearance, appearance)
            .AddChildContent(childContent)
            .Add(p => p.AutoComplete, autoComplete)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






