using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDesignTheme : ComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/DesignSystemProvider/FluentDesignTheme.razor.js";
    private DotNetObjectReference<FluentDesignTheme>? _dotNetHelper = null;
    private LocalizationDirection? _direction;
    private readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    [Inject]
    private GlobalState GlobalDesign { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the component.
    /// </summary> 
    [Parameter]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the Theme mode: Dark, Light, or browser System theme.
    /// </summary>
    [Parameter]
    public DesignThemeModes Mode { get; set; } = DesignThemeModes.System;

    /// <summary>
    /// Gets or sets a callback that updates the <see cref="Mode"/>.
    /// </summary>
    [Parameter]
    public EventCallback<DesignThemeModes> ModeChanged { get; set; }

    /// <summary>
    /// Gets or sets the Accent base color.
    /// </summary>
    [Parameter]
    public string? CustomColor { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the <see cref="CustomColor"/>.
    /// </summary>
    [Parameter]
    public EventCallback<string?> CustomColorChanged { get; set; }

    /// <summary>
    /// Gets or sets the application to defined the Accent base color.
    /// </summary>
    [Parameter]
    public OfficeColor? OfficeColor { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the <see cref="OfficeColor"/>.
    /// </summary>
    [Parameter]
    public EventCallback<OfficeColor?> OfficeColorChanged { get; set; }

    /// <summary>
    /// Gets or sets the local storage name to save and retrieve the <see cref="Mode"/> and the <see cref="OfficeColor"/> / <see cref="CustomColor"/>.
    /// </summary> 
    [Parameter]
    public string? StorageName { get; set; }

    /// <summary>
    /// Gets or sets the body.dir value.
    /// </summary> 
    [Parameter]
    public LocalizationDirection? Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            if (value is not null)
            {
                GlobalDesign.SetDirection((LocalizationDirection)value);
            }
            Module?.InvokeVoidAsync("UpdateDirection", value.ToAttributeValue());
        }
    }

    /// <summary>
    /// Callback raised when the Dark/Light luminance changes.
    /// </summary>
    [Parameter]
    public EventCallback<LuminanceChangedEventArgs> OnLuminanceChanged { get; set; }

    /// <summary>
    /// Callback raised when the component is rendered for the first time.
    /// </summary>
    [Parameter]
    public EventCallback<LoadedEventArgs> OnLoaded { get; set; }

    /// <summary>
    /// Gets or sets the content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public FluentDesignTheme()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Method raised by the JavaScript code when the "mode" changes.
    /// </summary>
    /// <param name="name">Attribute name: only "mode".</param>
    /// <param name="value">New value: for "mode" = "dark", "light", "system-dark", "system-light"</param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OnChangeRaisedAsync(string name, string value)
    {
        switch (name.ToLower())
        {
            case "mode":
                if (!Enum.TryParse<DesignThemeModes>(value, true, out var mode))
                {
                    mode = DesignThemeModes.System;
                }

                if (ModeChanged.HasDelegate)
                {
                    await ModeChanged.InvokeAsync(mode);
                }

                if (OnLuminanceChanged.HasDelegate)
                {
                    await OnLuminanceChanged.InvokeAsync(new LuminanceChangedEventArgs(mode, value.Contains("dark")));
                }
                GlobalDesign.SetLuminance(value.Contains("dark") ? StandardLuminance.DarkMode : StandardLuminance.LightMode);

                break;

            case "primary-color":
                if (value.StartsWith('#'))
                {
                    if (CustomColorChanged.HasDelegate)
                    {
                        await CustomColorChanged.InvokeAsync(value);
                    }
                }
                else
                {
                    if (!Enum.TryParse<OfficeColor>(value, true, out var color))
                    {
                        color = AspNetCore.Components.OfficeColor.Default;
                    }

                    if (OfficeColorChanged.HasDelegate)
                    {
                        await OfficeColorChanged.InvokeAsync(color);
                    }
                }

                break;
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _dotNetHelper = DotNetObjectReference.Create(this);

            var dir = await Module.InvokeAsync<string?>("GetDirection");
            if (!string.IsNullOrEmpty(dir))
            {
                _direction = dir switch
                {
                    "ltr" => LocalizationDirection.LeftToRight,
                    "rtl" => LocalizationDirection.RightToLeft,
                    _ => LocalizationDirection.LeftToRight
                };
            }

            var themeJSON = await Module.InvokeAsync<string>("addThemeChangeEvent", _dotNetHelper, Id);
            var theme = themeJSON == null ? null : JsonSerializer.Deserialize<DataLocalStorage>(themeJSON, JSON_OPTIONS);

            await ApplyLocalStorageValuesAsync(theme);

            var realLuminance = await Module.InvokeAsync<string>("GetGlobalLuminance");
            realLuminance = string.IsNullOrWhiteSpace(realLuminance) ? "1.0" : realLuminance;
            var isDark = double.Parse(realLuminance, CultureInfo.InvariantCulture) < 0.5;
            GlobalDesign.SetLuminance(isDark ? StandardLuminance.DarkMode : StandardLuminance.LightMode);

            if (OnLoaded.HasDelegate)
            {
                await OnLoaded.InvokeAsync(new LoadedEventArgs(Mode, isDark, CustomColor, OfficeColor, StorageName, Direction.ToAttributeValue()));
            }
        }
    }

    /// <summary>
    /// Clears the local storage.
    /// </summary>
    public async Task ClearLocalStorageAsync()
    {
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        await Module.InvokeVoidAsync("ClearLocalStorage", Id);
    }

    /// <summary />
    private async Task ApplyLocalStorageValuesAsync(DataLocalStorage? theme)
    {
        // Mode (Dark / Light / System)
        if (!string.IsNullOrEmpty(theme?.Mode))
        {
            if (!Enum.TryParse(theme.Mode, true, out DesignThemeModes mode))
            {
                mode = DesignThemeModes.System;
            }

            Mode = mode;
            await OnChangeRaisedAsync("mode", theme.Mode);
        }

        // Color
        if (!string.IsNullOrEmpty(theme?.PrimaryColor))
        {
            if (theme.PrimaryColor.StartsWith('#'))
            {
                CustomColor = theme.PrimaryColor;
                await OnChangeRaisedAsync("primary-color", theme.PrimaryColor);
            }
            else
            {
                if (!Enum.TryParse<OfficeColor>(theme.PrimaryColor, true, out var color))
                {
                    color = AspNetCore.Components.OfficeColor.Default;
                }
                OfficeColor = color;
                await OnChangeRaisedAsync("primary-color", theme.PrimaryColor);
            }
        }
    }

    /// <summary />
    private class DataLocalStorage
    {
        public string? Mode { get; set; }
        public string? PrimaryColor { get; set; }
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

        return OfficeColorUtilities.GetRandom().ToAttributeValue();
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

public record LuminanceChangedEventArgs(DesignThemeModes Mode, bool IsDark);

public record LoadedEventArgs(DesignThemeModes Mode, bool IsDark, string? CustomColor, OfficeColor? OfficeColor, string? StorageName, string? Direction);
