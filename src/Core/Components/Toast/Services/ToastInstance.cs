namespace Microsoft.FluentUI.AspNetCore.Components;

public sealed class ToastInstance
{
    public ToastInstance(Type? type, ToastParameters parameters, object content)
    {
        ContentType = type;
        Parameters = parameters;
        Content = content;
        Id = Parameters.Id ?? Identifier.NewId();
    }
    public string Id { get; }

    public Type? ContentType { get; set; }

    public object Content { get; set; } = default!;

    public ToastParameters Parameters { get; set; } = default!;

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "Content", Content } };
    }
}
