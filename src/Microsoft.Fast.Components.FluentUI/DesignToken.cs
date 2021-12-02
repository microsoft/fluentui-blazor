using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI
{
    public class DesignToken<T> : ComponentBase, IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>>? moduleTask;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter]
        public Func<ComponentBase>? GetControl { get; set; }

        [Parameter]
        public string? Element { get; set; }

        [Parameter]
        public T? Value { get; set; }

        public DesignToken()
        {
            moduleTask = new(() => JSRuntime.InvokeAsync<IJSObjectReference>(
                  "import", "./_content/Microsoft.Fast.Components.FluentUI/DesignTokenInterop.js").AsTask());
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (GetControl is null)
            //    throw new ArgumentNullException(nameof(GetControl));
            if (firstRender)
            {
                var module = await moduleTask!.Value;
                //await module.InvokeVoidAsync("setBaseHeightMultiplier", GetControl(), Value);

                await module.InvokeVoidAsync("setBaseHeightMultiplier", Element, Value);
            }

        }

        public async ValueTask SetValueFor(string element, T value)
        {
            var module = await moduleTask!.Value;
            await module.InvokeVoidAsync("setBaseHeightMultiplier", element, value);

        }
        public async ValueTask DisposeAsync()
        {
            if (moduleTask is not null && moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}