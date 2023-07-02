namespace Microsoft.Fast.Components.FluentUI;

public sealed class ToastInstance
{
    public ToastInstance(Type? type, object content, ToastParameters parameters)
    {
        ContentType = type;
        Content = content;
        Parameters = parameters;
        Id = Parameters.Id ?? Identifier.NewId();
    }
    public string Id { get; }

    public Type? ContentType { get; set; }

    public object Content { get; set; } = default!;

    public ToastParameters Parameters { get; set; } = default!;

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "Content", Content! } };
    }
}
