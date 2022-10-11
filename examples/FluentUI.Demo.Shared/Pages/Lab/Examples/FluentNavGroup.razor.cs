using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace FluentUI.Demo.Shared;
public partial class FluentNavGroup : FluentComponentBase
{
    private bool _expanded = false;

    protected string? ClassValue => new CssBuilder(Class)
            .Build();

    protected string? StyleValue => new StyleBuilder()
            .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
            .AddStyle(Style)
            .Build();

    [CascadingParameter]
    public FluentNavMenu NavMenu { get; set; } = default!;

    protected override void OnInitialized()
    {
        NavMenu.AddNavGroup(this);
        base.OnInitialized();
    }

    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

    [Parameter]
    public bool Disabled { get; set; } = false;

    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }

    [Parameter]
    public bool Expanded

    {
        get
        {
            return _expanded;
        }
        set
        {
            _expanded = value;
            if (OnExpandedChanged.HasDelegate)
                OnExpandedChanged.InvokeAsync(value);
        }
    }

    protected void OnExpandedHandler(MouseEventArgs e)
    {
        if (!Disabled)
            Expanded = !Expanded;
    }

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string Text { get; set; } = "";

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected async Task OnClickHandler(MouseEventArgs e)
    {
        if (Disabled)
            return;

        await NavMenu.SelectOnlyThisLinkAsync(null);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);
    }
}
