using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.Infrastructure
{
    public class CacheStorageAccessor : IAsyncDisposable
    {
        private Lazy<IJSObjectReference> _jsModule = new();
        private readonly IJSRuntime _jsRuntime;

        public CacheStorageAccessor(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task WaitForReference()
        {
            if (_jsModule.IsValueCreated is false)
            {
                _jsModule = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                    "./_content/Microsoft.Fast.Components.FluentUI/js/CacheStorageAccessor.js"));
            }
        }

        public async Task PutAsync(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
        {
            await WaitForReference();
            string requestMethod = requestMessage.Method.Method;
            string requestBody = await GetRequestBodyAsync(requestMessage);
            string responseBody = await responseMessage.Content.ReadAsStringAsync();

            await _jsModule.Value.InvokeVoidAsync("put", requestMessage.RequestUri, requestMethod, requestBody, responseBody);
        }

        public async Task<string> PutAndGetAsync(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
        {
            await WaitForReference();
            string requestMethod = requestMessage.Method.Method;
            string requestBody = await GetRequestBodyAsync(requestMessage);
            string responseBody = await responseMessage.Content.ReadAsStringAsync();

            await _jsModule.Value.InvokeVoidAsync("put", requestMessage.RequestUri, requestMethod, requestBody, responseBody);

            return responseBody;
        }


        public async Task<string> GetAsync(HttpRequestMessage requestMessage)
        {
            await WaitForReference();
            string requestMethod = requestMessage.Method.Method;
            string requestBody = await GetRequestBodyAsync(requestMessage);
            string result = await _jsModule.Value.InvokeAsync<string>("get", requestMessage.RequestUri, requestMethod, requestBody);

            return result;
        }

        public async Task RemoveAsync(HttpRequestMessage requestMessage)
        {
            await WaitForReference();
            string requestMethod = requestMessage.Method.Method;
            string requestBody = await GetRequestBodyAsync(requestMessage);
            await _jsModule.Value.InvokeVoidAsync("remove", requestMessage.RequestUri, requestMethod, requestBody);
        }

        public async Task RemoveAllAsync()
        {
            await WaitForReference();
            await _jsModule.Value.InvokeVoidAsync("removeAll");
        }

        private static async Task<string> GetRequestBodyAsync(HttpRequestMessage requestMessage)
        {
            string requestBody = "";

            if (requestMessage.Content is not null)
            {
                requestBody = await requestMessage.Content.ReadAsStringAsync() ?? "";
            }

            return requestBody;
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not needed")]
        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_jsModule.IsValueCreated)
                {
                    await _jsModule.Value.DisposeAsync();
                }
            }
            catch (JSDisconnectedException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
    }
}
