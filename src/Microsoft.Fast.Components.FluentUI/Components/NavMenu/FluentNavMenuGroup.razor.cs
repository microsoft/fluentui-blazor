using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentNavMenuGroup : FluentComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed for this group
    /// in the <see cref="FluentNavMenu"/> gutter when the menu is collapsed.
    /// This setting takes priority over <see cref="NavMenuGutterIcon"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? NavMenuGutterIconContent { get; set; }

    /// <summary>
    /// Gets or sets the name of the icon to display for this group
    /// in the <see cref="FluentNavMenu"/> gutter when the nav menu is collapsed.
    /// This setting is not used when <see cref="NavMenuGutterIconContent"/>
    /// is not null.
    /// </summary>
    [Parameter]
    public string NavMenuGutterIcon { get; set; } = FluentIcons.MoreHorizontal;

    /// <summary>
    /// Gets or sets the icon content to be displayed for this group
    /// before its <see cref="Text"/>.
    /// This setting takes priority over <see cref="Icon"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? IconContent { get; set; }

    /// <summary>
    /// Gets or sets the name of the icon to display for this group
    /// before its <see cref="Text"/>.
    /// This setting is not used when <see cref="IconContent"/>
    /// is not null.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Gets or sets the destination of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the menu group is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }


    /// <summary>
    /// Gets or sets whether the menu group is expanded
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets whether the menu group is selected
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets the text of the menu group
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the menu group
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Callback function for when the menu group is expanded
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }


    [CascadingParameter(Name = "NavMenu")]
    public FluentNavMenu NavMenu { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    public bool NavMenuExpanded { get; set; }

    public FluentNavMenuGroup()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-group")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    private bool HasIcon => !string.IsNullOrWhiteSpace(Icon) || IconContent is not null;

    internal bool HasNavMenuGutterIcon => !string.IsNullOrWhiteSpace(NavMenuGutterIcon) || NavMenuGutterIconContent is not null;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        bool originalExpanded = Expanded;
        await base.SetParametersAsync(parameters);
        if (Expanded != originalExpanded)
            await OnExpandedChanged.InvokeAsync(Expanded);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavMenu.AddNavMenuGroup(this);
    }

    private async Task ExpandMenu()
    {
        if (Disabled)
            return;

        // Expand the Nav Menu and this Group if the user clicks on its NavMenuToggleIcon
        if (!NavMenuExpanded)
        {
            await NavMenu.CollapsibleClickAsync();

            Expanded = true;
            await OnExpandedChanged.InvokeAsync(Expanded);

            Selected = true;
        }
    }

    private void HandleIconClick()
    {
        if (!Disabled)
            Selected = true;
    }
}
