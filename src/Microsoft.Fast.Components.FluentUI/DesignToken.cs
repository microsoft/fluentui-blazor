using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI
{
    public class DesignToken<T> : IAsyncDisposable
    {

        private readonly Lazy<Task<IJSObjectReference>>? moduleTask;

        private IJSObjectReference? module;


        private IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter]
        public string? Selector { get; set; }

        [Parameter]
        public T? Value { get; set; }

        [Parameter]
        public string? Name { get; set; }

        /// <summary>
        /// Constructs an instance of DesignToken"/>.
        /// </summary>
        public DesignToken()
        {
        }


        public DesignToken(IJSRuntime jsRuntime, string name)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Microsoft.Fast.Components.FluentUI/DesignTokenInterop.js").AsTask());

            Name = name;
        }

        public async ValueTask SetValueFor(string selector, T value)
        {
            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("setValueFor", Name, selector, value);

        }

        public async ValueTask DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }

        }
    }
}