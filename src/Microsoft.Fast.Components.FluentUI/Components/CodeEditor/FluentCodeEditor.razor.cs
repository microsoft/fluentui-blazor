using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;


namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentCodeEditor : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/CodeEditor/FluentCodeEditor.razor.js";
    private const string MONACO_VS_PATH = "./_content/Microsoft.Fast.Components.FluentUI/lib/monaco-editor/min/vs";

    private DotNetObjectReference<FluentCodeEditor>? _objRef = null;
    private string _value = """
                            using System;
                            void Main()
                            {
                                Console.WriteLine("Hello World");
                            }
                            """;

    
    protected string? ClassValue => new CssBuilder(Class)
         .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("border: calc(var(--stroke-width) * 1px) solid var(--neutral-stroke-rest)")
        .AddStyle(Style)
        .Build();

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference _jsModule { get; set; } = default!;

    /// <summary>
    /// Language used by the editor: csharp, javascript, ...
    /// </summary>
    [Parameter]
    public string Language { get; set; } = "csharp";

    /// <summary>
    /// Height of this component.
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "300px";

    /// <summary>
    /// Width of this component.
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// Theme of the editor (Light or Dark).
    /// </summary>
    [Parameter]
    public bool IsDarkMode { get; set; } = false;

    /// <summary>
    /// Gets or sets the value of the input. This should be used with two-way binding.
    /// </summary>
    [Parameter]
    public string Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
        }
    }

    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    public FluentCodeEditor()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override async Task OnParametersSetAsync()
    {
        if (_jsModule != null)
        {
            await _jsModule.InvokeVoidAsync(
                "monacoSetOptions",
                Id,
                new { Value, Theme = GetTheme(IsDarkMode), Language, });
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            _objRef = DotNetObjectReference.Create(this);

            var options = new
            {
                Value,
                Language,
                Theme = GetTheme(IsDarkMode),
                Path = MONACO_VS_PATH,
                LineNumbers = true,
                ReadOnly = false,
            };
            await _jsModule.InvokeVoidAsync("monacoInitialize", Id, _objRef, options);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary />
    private static string GetTheme(bool isDarkMode)
    {
        return isDarkMode ? "vs-dark" : "light";
    }

    public async Task Focus() => await _jsModule!.InvokeVoidAsync("focus");

    public async Task Resize() => await _jsModule!.InvokeVoidAsync("resize");

    /// <summary />
    [JSInvokable]
    public async Task UpdateValueAsync(string value)
    {
        _value = value;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(_value);
        }
    }

    /// <summary />
    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
        }

        if (_objRef is not null)
        {
            _objRef.Dispose();
        }
    }
}
