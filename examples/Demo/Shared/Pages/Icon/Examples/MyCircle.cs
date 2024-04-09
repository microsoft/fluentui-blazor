using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.Icon.Examples;

internal class MyCircle : Microsoft.FluentUI.AspNetCore.Components.Icon
{
    private const string SVG_CONTENT = "<svg version='1.1' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 320 320'><circle cx='160' cy='160' r='140'/></svg>";

    public MyCircle() : base("MyCircle", IconVariant.Regular, IconSize.Custom, SVG_CONTENT)
    {
        // Default Color (`fill` style)
        WithColor("#F97316");
    }
}
