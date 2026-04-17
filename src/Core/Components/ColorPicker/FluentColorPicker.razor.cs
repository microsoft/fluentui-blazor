// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.ColorPicker;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The ColorPicker component is used to select a color from a palette.
/// </summary>
public partial class FluentColorPicker : FluentComponentBase
{
    private HsvColor _hsv = HsvColor.Default;
    private DotNetObjectReference<FluentColorPicker>? _dotNetHelper;

    /// <summary />
    public FluentColorPicker(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-color-picker")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, when: Width is not null)
        .AddStyle("height", Height, when: Height is not null)

        // ColorPickerView.SwatchPalette
        .AddStyle("width", "calc(19 * 20px)", when: Width is null && View == ColorPickerView.SwatchPalette && Orientation == Orientation.Horizontal)
        .AddStyle("width", "calc(10 * 20px)", when: Width is null && View == ColorPickerView.SwatchPalette && Orientation == Orientation.Vertical)
        .AddStyle("height", "calc(10 * 20px)", when: Height is null && View == ColorPickerView.SwatchPalette && Orientation == Orientation.Horizontal)
        .AddStyle("height", "calc(19 * 20px)", when: Height is null && View == ColorPickerView.SwatchPalette && Orientation == Orientation.Vertical)

        // ColorPickerView.ColorWheel
        .AddStyle("width", "300px", when: Width is null && View == ColorPickerView.ColorWheel)
        .AddStyle("height", "260px", when: Height is null && View == ColorPickerView.ColorWheel)

        // ColorPickerView.HsvSquare
        .AddStyle("width", "300px", when: Width is null && View == ColorPickerView.HsvSquare)
        .AddStyle("height", "200px", when: Height is null && View == ColorPickerView.HsvSquare)
        .Build();

    /// <summary>
    /// Gets or sets the view of the color picker.
    /// </summary>
    [Parameter]
    public ColorPickerView View { get; set; } = ColorPickerView.SwatchPalette;

    /// <summary>
    /// Gets or sets the orientation of the color items in the swatch palette view.
    /// Default is <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the width of the color picker.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the color picker.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the currently selected color.
    /// </summary>
    [Parameter]
    public string? SelectedColor { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the selected color changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> SelectedColorChanged { get; set; }

    /// <summary>
    /// Gets or sets the custom color palette to display in the Swatch or ColorWheel view.
    /// If not set, the default palette will be used.
    /// The palette should contain hex color strings (e.g. "#FF0000").
    /// </summary>
    [Parameter]
    public IReadOnlyList<string>? Palette { get; set; }

    private async Task ColorSelectHandlerAsync(string color)
    {
        if (!string.Equals(SelectedColor, color, StringComparison.OrdinalIgnoreCase))
        {
            SelectedColor = color;

            if (SelectedColorChanged.HasDelegate)
            {
                await SelectedColorChanged.InvokeAsync(color);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified color is the currently selected color.
    /// </summary>
    private bool IsSelectedColor(string color)
    {
        return string.Equals(SelectedColor, color, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (!parameters.TryGetValue<ColorPickerView>(nameof(View), out var view))
        {
            view = View;
        }

        if (parameters.TryGetValue<IReadOnlyList<string>>(nameof(Palette), out var palette))
        {
            var requiredCount = view switch
            {
                ColorPickerView.SwatchPalette => 190,
                ColorPickerView.ColorWheel => 127,
                _ => 0,
            };

            if (requiredCount > 0 && palette.Count < requiredCount)
            {
                throw new ArgumentException($"The Palette must contain at least {requiredCount} colors for the {view} view, but only {palette.Count} were provided.");
            }
        }

        if (view == ColorPickerView.HsvSquare
            && parameters.TryGetValue<string>(nameof(SelectedColor), out var selectedColor)
            && !string.IsNullOrEmpty(selectedColor)
            && !string.Equals(selectedColor, SelectedColor, StringComparison.OrdinalIgnoreCase))
        {
            _hsv = HsvColor.FromHex(selectedColor);
        }

        return base.SetParametersAsync(parameters);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && View == ColorPickerView.HsvSquare)
        {
            _dotNetHelper = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.ColorPicker.Initialize", _dotNetHelper, Id, _hsv.Hue, _hsv.Saturation, _hsv.Value);
        }
    }

    /// <summary>
    /// Called by JavaScript when the user selects a color in the HSV picker.
    /// </summary>
    [JSInvokable("FluentColorPicker.ColorChangedAsync")]
    public async Task HsvColorChangedAsync(string hexColor)
    {
        SelectedColor = hexColor;

        if (SelectedColorChanged.HasDelegate)
        {
            await SelectedColorChanged.InvokeAsync(hexColor);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary />
    public override async ValueTask DisposeAsync()
    {
        if (_dotNetHelper != null)
        {
            await JSRuntime.SafelyInvokeAsync("Microsoft.FluentUI.Blazor.Components.ColorPicker.Dispose", Id);

            _dotNetHelper.Dispose();
            _dotNetHelper = null;
        }

        await base.DisposeAsync();
    }
}
