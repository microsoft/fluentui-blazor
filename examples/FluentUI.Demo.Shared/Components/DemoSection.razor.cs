using System.Reflection.Metadata;
using FluentUI.Demo.Generators;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;
public partial class DemoSection : ComponentBase
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public RenderFragment? Description { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    //No .razor needed
    public string? CodeFilename { get; set; }

    [Parameter]
    public bool IsNew { get; set; }

    private bool HasCode { get; set; }

    private string? CodeContents { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            bool isDisplayDemoCode = true;
            bool hasFilename = !string.IsNullOrEmpty(CodeFilename);

            HasCode = hasFilename && isDisplayDemoCode;

            if (HasCode)
            {
                SetCodeContents();
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected void SetCodeContents()
    {
        try
        {
            CodeContents = DemoSnippets.GetRazor($"{CodeFilename}.razor");
            StateHasChanged();
        }
        catch
        {
            //Do Nothing
        }
    }
}
