using Microsoft.JSInterop;

namespace BcdLib.Components;


public class DocumentJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public DocumentJsInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "/_content/BcdLib.PullComponent/dist/index.js").AsTask());
    }

    public async ValueTask<int> GetScrollDistToTopAsync()
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<int>("Document.getScrollDistToTop");
    }


    public async ValueTask<int> GetScrollDistToBottomAsync()
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<int>("Document.getScrollDistToBottom");
    }
    


    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
