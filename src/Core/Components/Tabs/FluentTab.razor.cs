using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTab : FluentComponentBase
{
    private DotNetObjectReference<FluentTab>? _dotNetHelper = null;
    private IJSObjectReference _jsModule = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("height", $"calc({Owner?.Height} - 40px); overflow-y: auto", () => !string.IsNullOrEmpty(Owner?.Height))
        .Build();

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// When true, the control will be immutable by user interaction. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the visibility of a tab
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the label of the tab.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Callback to invoke when the label changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> LabelChanged { get; set; }

    /// <summary>
    /// Gets or sets the class, applied to the Label Tab Item.
    /// </summary>
    [Parameter]
    public virtual string? LabelClass { get; set; }

    /// <summary>
    /// Gets or sets the style, applied to the Label Tab Item.
    /// </summary>
    [Parameter]
    public virtual string? LabelStyle { get; set; }

    /// <summary>
    /// Gets or sets the customized content of the header.
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Gets or sets the icon to display in front of the tab
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets the index number of this tab.
    /// </summary>
    public int Index { get; set; } = 0;

    /// <summary>
    /// True to let the user edit the <see cref="Label"/> property.
    /// </summary>
    [Parameter]
    public bool LabelEditable { get; set; } = false;

    /// <summary>
    /// Render the tab content only when the tab is selected.
    /// </summary>
    [Parameter]
    public bool DeferredLoading { get; set; } = false;

    /// <summary>
    /// Gets or sets the customized content of this tab panel.
    /// </summary>
    [Parameter]
    public RenderFragment? Content { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTabs component.
    /// </summary>
    [CascadingParameter]
    public FluentTabs Owner { get; set; } = default!;

    /// <summary>
    /// If this tab is outside of visible tab panel area.
    /// </summary>
    public bool? Overflow { get; private set; }

    public FluentTab()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Index = Owner!.RegisterTab(this);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Tabs/FluentTab.razor.js");
            _dotNetHelper = DotNetObjectReference.Create(this);

            await _jsModule.InvokeVoidAsync("TabEditable_Changed", _dotNetHelper, $"#{Id} span[contenteditable='true']", Id);
        }
    }

    /// <summary />
    protected virtual Task CloseClickedAsync()
    {
        return Owner!.UnregisterTabAsync(this);
    }

    /// <summary />
    [JSInvokable]
    public async Task UpdateTabLabelAsync(string tabId, string label)
    {
        if (Id == tabId && Label != label)
        {
            Label = label;

            if (LabelChanged.HasDelegate)
            {
                await LabelChanged.InvokeAsync(label);
            }
        }
    }

    /// <summary />
    internal void SetProperties(bool? overflow)
    {
        Overflow = overflow == true ? overflow : null;
    }
}
