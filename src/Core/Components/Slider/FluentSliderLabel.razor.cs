using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentSliderLabel<TValue> : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Slider/FluentSliderLabel.razor.js";

    public FluentSliderLabel()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the value for this slider position.
    /// </summary>
    [Parameter]
    public TValue? Position { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether marks are hidden.
    /// </summary>
    [Parameter]
    public bool? HideMark { get; set; }

    /// <summary>
    /// Gets or sets disabled state of the label. This is generally controlled by the parent.
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected string? FormatValueAsString(TValue? value)
    {
        // Avoiding a cast to IFormattable to avoid boxing.
        return value switch
        {
            null => null,
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await Module.InvokeVoidAsync("updateSliderLabel", Id);
        }
    }

    public ValueTask DisposeAsync()
    {
        if (Module is not null)
        {
            return Module.DisposeAsync();
        }
        return ValueTask.CompletedTask;
    }
}
