using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FluentUI.Demo.Shared.Components;
public partial class DemoSection : ComponentBase
{
    private bool _hasCode = false;
    private readonly Dictionary<string, string> _tabPanelsContent = [];
    private readonly List<string> _allFiles = [];
    private string? _ariaId;

    private readonly Regex _pattern = Pattern();
    private ErrorBoundary? errorBoundary;

    [Inject]
    private HttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IStaticAssetService StaticAssetService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? Description { get; set; }

    /// <summary>
    /// Gets or sets the component for which the example will be shown. Enter the type (typeof(...)) _name 
    /// </summary>
    [Parameter, EditorRequired]
    public Type Component { get; set; } = default!;

    ///<summary>
    /// Any parameters that need to be supplied to the component
    /// </summary>
    [Parameter]
    public Dictionary<string, object>? ComponentParameters { get; set; }

    /// <summary>
    /// Any collocated isolated .cs, .css or .js files (enter the extensions only) that need to be shown in a tab and as a download. 
    /// Example: @(new[] { "css", "js", "abc.cs" })
    /// </summary>
    [Parameter]
    public string[]? CollocatedFiles { get; set; }

    /// <summary>
    /// Any additional files that need to be shown in a tab and as a download. 
    /// Example: @(new[] { "abc.cs", "def.js" })
    /// </summary>
    [Parameter]
    public string[]? AdditionalFiles { get; set; }

    [Parameter]
    public string MaxHeight { get; set; } = string.Empty;

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

    /// <summary>
    /// Hides all but the 'Example' tab
    /// </summary>
    [Parameter]
    public bool HideAllButExample { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (_allFiles.Count > 0)
        {
            _allFiles.Clear();
        }

        _allFiles.AddRange(GetCollocatedFiles());
        _allFiles.AddRange(GetAdditionalFiles());

        _ariaId = _pattern.Replace(Title.ToLower(), "");
        if (_ariaId.Length > 20)
        {
            _ariaId = _ariaId[..20];
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetCodeContentsAsync();
            _hasCode = true;
            StateHasChanged();
        }
    }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }

    protected async Task SetCodeContentsAsync()
    {
        HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);

        try
        {
            foreach (var source in _allFiles)
            {
                var result = await StaticAssetService.GetAsync($"./_content/FluentUI.Demo.Shared/sources/{source}.txt");
                _tabPanelsContent.Add(source, result ?? string.Empty);
            }
        }
        catch
        {
            //Do Nothing
        }
    }

    private IEnumerable<string> GetCollocatedFiles()
    {
        yield return $"{Component.Name}.razor";
        foreach (var ext in CollocatedFiles ?? Enumerable.Empty<string>())
        {
            yield return $"{Component.Name}.razor.{ext}";
        }
    }

    private IEnumerable<string> GetAdditionalFiles()
    {
        foreach (var name in AdditionalFiles ?? Enumerable.Empty<string>())
        {
            yield return name;
        }
    }

    private static string GetDisplayName(string name)
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

    private static string? TabLanguageClass(string tabName)
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

    [GeneratedRegex(@"[;,<>&(){}!$^#@=/\ ]")]
    private static partial Regex Pattern();
}
