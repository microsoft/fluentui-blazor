using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.PresenceBadge;
public class FluentPresenceBadgeTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentPresenceBadge_Default()
    {
        //Arrange
        //Services.AddSingleton<GlobalState,/*Add implementation for GlobalState*/>();
        var childContent = "<b>render me</b>";
        string title = default!;
        PresenceStatus? status = default!;
        string statusTitle = default!;
        bool outOfOffice = default!;
        PresenceBadgeSize size = default!;
        var cut = TestContext.RenderComponent<FluentPresenceBadge>(parameters => parameters
            .AddChildContent(childContent)
            .Add(p => p.Title, title)
            .Add(p => p.Status, status)
            .Add(p => p.StatusTitle, statusTitle)
            .Add(p => p.OutOfOffice, outOfOffice)
            .Add(p => p.Size, size)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

