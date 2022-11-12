using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Components;

public partial class TableOfContents : IDisposable
{
    private record Anchor(string Level, string Text, string Href, Anchor[] Anchors);
    private Anchor[]? Anchors;

    private IJSObjectReference _jsModule = default!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/FluentUI.Demo.Shared/Components/TableOfContents.razor.js");
            await QueryDom();
        }
    }

    private async Task BackToTop()
    {
        await _jsModule.InvokeAsync<Anchor[]?>("backToTop");
    }

    private async Task QueryDom()
    {
        Anchors = await _jsModule.InvokeAsync<Anchor[]?>("queryDomForTocEntries");
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        // Subscribe to the event
        NavigationManager.LocationChanged += LocationChanged;
        base.OnInitialized();
    }

    private async void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        await QueryDom();
    }
    
    


    private RenderFragment? GetTocItems(IEnumerable<Anchor>? items)
    {
        if (items is not null)
        {
            return new RenderFragment(builder =>
            {
                //        @if(Anchors is not null)
                //{
                //    < ul >
                //    @foreach(Anchor anchor in Anchors)
                //    {
                //        < li >
                //            < FluentAnchor Href = "@anchor.Href" Appearance = "Appearance.Hypertext" > @anchor.Text </ FluentAnchor >
                //        </ li >
                //    }
                //    </ ul >

                //}

                int i = 0;

                builder.OpenElement(i++, "ul");
                foreach (Anchor item in items)
                {
                    builder.OpenElement(i++, "li");
                    builder.OpenComponent<FluentAnchor>(i++);
                    builder.AddAttribute(i++, "Href", item.Href);
                    builder.AddAttribute(i++, "Appearance", Appearance.Hypertext);
                    builder.AddAttribute(i++, "ChildContent", (RenderFragment)(content =>
                    {
                        content.AddContent(i++, item.Text);
                        
                    }));
                    builder.CloseComponent();
                    if (item.Anchors is not null)
                    {
                       builder.AddContent(i++, GetTocItems(item.Anchors));
                    }
                    builder.CloseElement();
                }
                builder.CloseElement();
            });
        }
        else
        {
            return new RenderFragment(builder =>
            {
                builder.AddContent(0, ChildContent);
            });
        }

    }

    void IDisposable.Dispose()
    {
        // Unsubscribe from the event when our component is disposed
        NavigationManager.LocationChanged -= LocationChanged;
    }
}