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
    public FluentNavMenu Owner { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [Parameter]
    public bool Disabled { get; set; } = false;

    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }

    [Parameter]
    public bool Expanded { get; set; } = false;

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string Text { get; set; } = "";

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected override void OnInitialized()
    {
        //Owner.Register(this);
        Owner.AddNavGroup(this);
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

        Owner.SelectOnlyThisLink(null);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);
    }
}
