using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDesignTheme : ComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/DesignSystemProvider/FluentDesignTheme.razor.js";
    private DotNetObjectReference<FluentDesignTheme>? _dotNetHelper = null;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the component.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the Theme mode: Dark, Light, or browser System theme.
    /// </summary>
    [Parameter]
    public DesignThemeModes Mode { get; set; } = DesignThemeModes.System;

    /// <summary>
    /// Gets or sets the Accent base color.
    /// </summary>
    [Parameter]
    public string? CustomColor { get; set; }

    /// <summary>
    /// Gets or sets the application to defined the Accent base color.
    /// </summary>
    [Parameter]
    public OfficeColor? OfficeColor { get; set; }

    [Parameter]
    public EventCallback<DesignThemeModes> ModeChanged { get; set; }

    /// <summary />
    [JSInvokable]
    public async Task OnChangeRaisedAsync(string name, string oleValue, string newValue)
    {
        Console.WriteLine($"{name} - {oleValue} - {newValue}");

        if (name == "mode" && ModeChanged.HasDelegate)
        {
            if (!Enum.TryParse<DesignThemeModes>(newValue, true, out var mode))
            {
                mode = DesignThemeModes.System;
            }

            await ModeChanged.InvokeAsync(mode);
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _dotNetHelper = DotNetObjectReference.Create(this);

            await Module.InvokeVoidAsync("addThemeChangeEvent", _dotNetHelper, Id);
        }
    }

    /// <summary />
    private string? GetColor()
    {
        if (CustomColor != null)
        {
            return CustomColor;
        }

        if (OfficeColor != null && OfficeColor.HasValue)
        {
            return Enum.GetName(OfficeColor.Value);
        }

        return null;
    }

    private string? GetMode()
    {
        return Mode switch
        {
            DesignThemeModes.Dark => "dark",
            DesignThemeModes.Light => "light",
            _ => null
        };
    }
}