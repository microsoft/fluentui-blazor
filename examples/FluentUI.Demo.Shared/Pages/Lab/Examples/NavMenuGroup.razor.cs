using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;
public partial class NavMenuGroup : FluentComponentBase
{
    private bool _expanded = false;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-group")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle(Style)
        .Build();

    [CascadingParameter]
    public NavMenu Owner { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuCollapsed")]
    private bool NavMenuCollapsed { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool Selected { get; set; }


    [Parameter]
    public bool Expanded { get; set; }

    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string Text { get; set; } = "";

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected override void OnInitialized()
    {
        Owner.AddNavGroup(this);
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (_expanded != Expanded)
        {
            _expanded = Expanded;
            if (OnExpandedChanged.HasDelegate)
                OnExpandedChanged.InvokeAsync(_expanded);
        }

        base.OnParametersSet();
    }

    protected void OnExpandedHandler(MouseEventArgs e)
    {
        if (!Disabled)
            Expanded = !Expanded;
    }

    protected async Task OnClickHandler(MouseEventArgs e)
    {
        if (Disabled)
            return;

        Owner.SelectOnlyThisGroup(this);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);
    }

    protected async Task OnKeypressHandler(KeyboardEventArgs e)
    {
        if (e.Code == "Space" || e.Code == "Enter")
        {
            await OnClickHandler(new MouseEventArgs());
        }
    }

    internal void SetSelected(bool value)
    {
        Selected = value;
    }
}
