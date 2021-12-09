using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI
{
    public class DesignToken<T> : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>>? moduleTask;
        private IJSObjectReference? module;
       
        [Parameter]
        public T? Value { get; set; }

        [Parameter]
        public T? DefaultValue { get; set; }

        [Parameter]
        public string? Name { get; set; }

        /// <summary>
        /// Constructs an instance of a DesignToken"/>.
        /// </summary>
        public DesignToken(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Microsoft.Fast.Components.FluentUI/DesignTokenInterop.js").AsTask());
        }

        public DesignToken(IJSRuntime jsRuntime, string name) : this(jsRuntime)
        {
            Name = name;
        }

        public DesignToken<T> WithDefault(T value)
        {
            this.DefaultValue = value;
            return this;
        }

        public async ValueTask SetValueFor(string selector)
        {
            if (DefaultValue == null)
                throw new ArgumentNullException(nameof(DefaultValue), $"{nameof(DefaultValue)} should be set before calling SetValueFor");

            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("setValueForSelector", Name, selector, DefaultValue);

        }

        public async ValueTask SetValueFor(string selector, T value)
        {
            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("setValueForSelector", Name, selector, value);

        }

        public async ValueTask SetValueFor(ElementReference element)
        {
            if (DefaultValue == null)
                throw new ArgumentNullException(nameof(DefaultValue), $"{nameof(DefaultValue)} should be set before calling SetValueFor");

            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("setValueFor", Name, element, DefaultValue);

        }

        public async ValueTask SetValueFor(ElementReference element, T value)
        {
            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("setValueFor", Name, element, value);

        }

        public async ValueTask DeleteValueFor(string selector)
        {
            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("deleteValueForSelector", Name, selector);
        }

        public async ValueTask DeleteValueFor(ElementReference element)
        {
            module = await moduleTask!.Value;
            await module.InvokeVoidAsync("deleteValueFor", Name, element);
        }

        public async ValueTask<T> GetValueFor(string selector)
        {
            module = await moduleTask!.Value;
            return await module.InvokeAsync<T>("getValueForSelector", Name, selector);
        }

        public async ValueTask<T> GetValueFor(ElementReference element)
        {
            module = await moduleTask!.Value;
            return await module.InvokeAsync<T>("getValueFor", Name, element);
        }


        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
        public async ValueTask DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }

        }
    }
}