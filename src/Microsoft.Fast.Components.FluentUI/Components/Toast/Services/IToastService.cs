namespace Microsoft.Fast.Components.FluentUI;

public interface IToastService : IDisposable
{
    event Action? OnToastUpdated;

    IEnumerable<Toast> ShownToasts { get; }

    ToastGlobalOptions Configuration { get; }

    Toast Add(Action<ToastOptions> options);

    Toast Add(string message);

    Toast Add(string message, ToastIntent severity);

    void Clear();

    void RemoveToast(Toast toast);
}
