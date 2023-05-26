namespace Microsoft.Fast.Components.FluentUI;

public sealed class ToastInstance
{
    public ToastInstance(Type type, ToastParameters parameters, ToastSettings settings)
    {
        ContentType = type;
        Parameters = parameters;
        Settings = settings;

        Intent = parameters.Intent;
        Title = parameters.Title;

    }

    public string Id { get; } = Identifier.NewId();

    public DateTime Timestamp { get; } = DateTime.Now;

    public ToastIntent Intent { get; }

    public string? Title { get; set; }

    public Type? ContentType { get; set; }

    public ToastParameters Parameters { get; set; } = new();

    public ToastSettings Settings { get; }
}
