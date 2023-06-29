namespace Microsoft.Fast.Components.FluentUI;

public sealed class ToastInstance
{
    public ToastInstance(Type? type, object data, ToastSettings settings)
    {
        ContentType = type;
        Data = data;
        Settings = settings;
    }

    public string Id { get; } = Identifier.NewId();

    public DateTime Timestamp { get; } = DateTime.Now;

    public ToastIntent Intent { get; }

    public Type? ContentType { get; set; }

    public object Data { get; set; } = default!;

    public ToastSettings Settings { get; set; } = default!;

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "Data", Data! } };
    }
}
