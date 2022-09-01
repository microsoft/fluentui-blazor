using System.Reflection.Metadata;
using FluentUI.Demo.Generators;
using Microsoft.AspNetCore.Components;


namespace FluentUI.Demo.Shared.Components;
public partial class DemoSection : ComponentBase
{
    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? Description { get; set; }

    [Parameter, EditorRequired]
    //No .razor needed
    public string ExampleFile { get; set; } = string.Empty;

    [Parameter]
    public bool IsNew { get; set; }

    private bool HasCode { get; set; } = false;

    private string? CodeContents { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (!string.IsNullOrEmpty(ExampleFile))
            {
                HasCode = true;
                SetCodeContents();
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected void SetCodeContents()
    {
        try
        {
            CodeContents = DemoSnippets.GetRazor($"{ExampleFile}");
            StateHasChanged();
        }
        catch
        {
            //Do Nothing
        }
    }
}
