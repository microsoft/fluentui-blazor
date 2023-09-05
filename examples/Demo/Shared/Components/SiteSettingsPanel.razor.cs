using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class SiteSettingsPanel
{
    [Parameter]
    public Settings Content { get; set; } = default!;


}