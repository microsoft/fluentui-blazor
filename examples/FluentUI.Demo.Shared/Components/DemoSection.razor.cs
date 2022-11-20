using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;


namespace FluentUI.Demo.Shared.Components;
public partial class DemoSection : ComponentBase
{
    [Inject]
    private HttpClient Http { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? Description { get; set; }

    /// <summary>
    /// The component for wich the example will be shown. Enter the name only. '.razor' will be added internally
    /// </summary>
    [Parameter, EditorRequired]
    public Type Component { get; set; } = default!;

    ///<summary>
    /// Any parameters that need to be supplied to the component
    /// </summary>
    [Parameter]
    public Dictionary<string, object>? ComponentParameters { get; set; }

    /// <summary>
    /// Any collocated (isolated) .cs, .css or .js files. Enter the extensions only
    /// Example: @(new[] { "css", "js" })
    /// </summary>
    [Parameter]
    public string[]? CollocatedFiles { get; set; }

    [Parameter]
    public bool New { get; set; }

    /// <summary>
    /// Show download links for the example sources
    /// Default = true
    /// </summary>
    [Parameter]
    public bool ShowDownloads { get; set; } = true;


    /// <summary>
    /// Hide the 'Example' tab
    /// </summary>
    [Parameter]
    public bool HideExample { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool HasCode { get; set; } = false;

    private Dictionary<string, string> TabPanelsContent { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            HasCode = true;
            await SetCodeContentsAsync();

        }
    }

    protected async Task SetCodeContentsAsync()
    {
        try
        {
            if (Http.BaseAddress is null)
            {
                Http.BaseAddress = new Uri(NavigationManager.BaseUri);
            }
            foreach (string source in GetSources())
            {
                TabPanelsContent.Add(source, await Http.GetStringAsync($"./_content/FluentUI.Demo.Shared/sources/{source}.txt"));
            }
            StateHasChanged();
        }
        catch
        {
            //Do Nothing
        }
    }

    IEnumerable<string> GetSources()
    {
        yield return $"{Component.Name}.razor";
        foreach (string ext in CollocatedFiles ?? Enumerable.Empty<string>())
        {
            yield return $"{Component.Name}.razor.{ext}";
        }
    }

    static string GetDisplayName(string name)
    {

        if (name.EndsWith(".cs"))
        {
            return "C#";
        }

        if (name.EndsWith(".razor"))
        {
            return "Razor";
        }

        if (name.EndsWith(".css"))
        {
            return "CSS";
        }

        if (name.EndsWith(".js"))
        {
            return "JavaScript";
        }

        return name;
    }

    static string? TabLanguageClass(string tabName)
    {
        if (tabName.EndsWith(".cs"))
        {
            return "language-csharp";
        }

        if (tabName.EndsWith(".razor"))
        {
            return "language-cshtml-razor";
        }

        if (tabName.EndsWith(".css"))
        {
            return "language-css";
        }
        if (tabName.EndsWith(".js"))
        {
            return "language-javascript";
        }

        return null;
    }
}
