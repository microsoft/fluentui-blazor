using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Pages.Icon;
public partial class IconPage
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? module;

    private EditContext? editContext;
    List<IconModel> icons = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/FluentUI.Demo.Shared/Pages/Icon/IconPage.razor.js");
        }
    }

    public void HandleStyle()
    {
        if (Form.Filled && Form.Regular || !Form.Filled && !Form.Regular)
        {
            Form.Style = null;
        }
        else if (Form.Filled && !Form.Regular)
        {
            Form.Style = true;
        }
        else if (!Form.Filled && Form.Regular)
        {
            Form.Style = false;
        }

        HandleSearch();
    }

    public void HandleSize(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
        {
            Form.Size = Enum.Parse<IconSize>((string)args.Value);
            HandleSearch();
        }

        HandleSearch();
    }

    public void HandleColor(ChangeEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Value?.ToString()))
        {
            Form.Color = Enum.Parse<IconColor>((string)args.Value);
            HandleSearch();
        }

        HandleSearch();
    }


    public void HandleSearch()
    {
        if (Form.Searchterm is not null && Form.Searchterm.Trim().Length > 2)
        {
            icons = FluentIcons.GetFilteredIcons(searchterm: Form.Searchterm.Trim(), size: Form.Size, filled: Form.Style);
        }

        StateHasChanged();
        return;
    }

    public async void HandleClick(IconModel icon)
    {
        Console.WriteLine($"You clicked on {icon.Name}");

        string Text = $@"<FluentIcon Name=""@FluentIcons.{icon.Folder}"" Size=""IconSize.{icon.Size}"" Filled={icon.Filled.ToString().ToLower()} Color=""IconColor.{Form.Color}""/>";

        if (module is not null)
        {
            await module.InvokeVoidAsync("copyText", Text);
        }
    }

    public class FormModel
    {
        public IconSize Size { get; set; }

        public string? Searchterm { get; set; }

        public bool? Style { get; set; }

        public bool Filled { get; set; }

        public bool Regular { get; set; }

        public IconColor Color { get; set; }
    }

    private FormModel Form = new() { Size = IconSize.Size24, Searchterm = "", Style = null, Color = IconColor.Accent };
    protected override void OnInitialized()
    {
        editContext = new EditContext(Form);
    }
}