using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens
{
    public class DesignToken<T> : IAsyncDisposable
    {

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        private T? _defaultValue;

        /// <summary>
        /// Gets the name of this design token 
        /// </summary>
        public string? Name { get; private set; }

        private DesignToken(IJSRuntime jsRuntime, IConfiguration configuration)
        {

            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", configuration["FluentWebComponentsScriptSource"]).AsTask());
        }

        /// <summary>
        /// Constructs an instance of a DesignToken.
        /// </summary>
        public DesignToken(IJSRuntime jsRuntime, IConfiguration configuration, string name) : this(jsRuntime, configuration)
        {
            Name = name;


            //WIP: Create your own token
            //bool hasField = typeof(Constants).GetFields(BindingFlags.Public | BindingFlags.Static).
            //    Select(x => x.GetRawConstantValue()?.ToString()).Contains(name);

            //if (!hasField)
            //{
            //    Create(name);
            //}

        }

        //WIP: Create your own token
        //private async ValueTask Create(string name)
        //{
        //    IJSObjectReference module = await moduleTask.Value;
        //    await module.InvokeVoidAsync(Name + ".create", typeof(T), name);
        //}

        /// <summary>
        /// Sets the default value of this token
        /// </summary>
        public DesignToken<T> WithDefault(T value)
        {
            _defaultValue = value;
            return this;
        }

        /// <summary>
        /// Sets the value of the for the associated <see cref="ElementReference"/> to the default value
        /// </summary>
        /// <param name="element">the associated <see cref="ElementReference"/> </param>
        /// <exception cref="Exception"></exception>
        public async ValueTask SetValueFor(ElementReference element)
        {
            if (_defaultValue == null)
                throw new Exception("WithDefault should be called before calling SetValueFor");

            await SetValueFor(element, _defaultValue);
        }

        /// <summary>
        /// Sets the value of the for the associated <see cref="ElementReference"/> to the supplied value
        /// </summary>
        /// <param name="element">the associated <see cref="ElementReference"/></param>
        /// <param name="value">the value to set</param>
        /// <returns></returns>
        public async ValueTask SetValueFor(ElementReference element, T value)
        {
            IJSObjectReference module = await moduleTask.Value;
            await module.InvokeVoidAsync(Name + ".setValueFor", element, value);
        }

        /// <summary>
        /// Deletes the set value for the associated <see cref="ElementReference"/>
        /// </summary>
        /// <param name="element">the associated <see cref="ElementReference"/></param>
        /// <returns></returns>
        public async ValueTask DeleteValueFor(ElementReference element)
        {
            IJSObjectReference module = await moduleTask.Value;
            await module.InvokeVoidAsync(Name + ".deleteValueFor", element);
        }

        /// <summary>
        /// Gets the set value for the associated <see cref="ElementReference"/>
        /// </summary>
        /// <param name="element">the associated <see cref="ElementReference"/></param>
        /// <returns>the value</returns>
        public async ValueTask<T> GetValueFor(ElementReference element)
        {
            IJSObjectReference module = await moduleTask.Value;
            return await module.InvokeAsync<T>(Name + ".getValueFor", element);
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not needed")]
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                IJSObjectReference module = await moduleTask.Value;
                await module.DisposeAsync();
            }

        }
    }
}