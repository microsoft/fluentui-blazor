using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.Icon.Examples
{
    internal class MyIcon : Microsoft.FluentUI.AspNetCore.Components.Icon
    {
        private const string SVG_CONTENT = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 124 124"">
                                                <rect width=""124"" height=""124"" rx=""24""/>
                                                <path d=""M19.375 36.7818V100.625C19.375 102.834 21.1659 104.625 23.375 104.625H87.2181C90.7818 104.625 92.5664 100.316 90.0466 97.7966L26.2034 33.9534C23.6836 31.4336 19.375 33.2182 19.375 36.7818Z"" fill=""white""/>
                                                <circle cx=""63.2109"" cy=""37.5391"" r=""18.1641"" fill=""black""/>
                                                <rect opacity=""0.4"" x=""81.1328"" y=""80.7198"" width=""17.5687"" height=""17.3876"" rx=""4"" transform=""rotate(-45 81.1328 80.7198)"" fill=""#FDBA74""/>
                                                </svg>";

        public MyIcon() : base("MyIcon", IconVariant.Filled, IconSize.Custom, SVG_CONTENT)
        {
            // Default Color
            WithColor("#F97316");
        }
    }
}
