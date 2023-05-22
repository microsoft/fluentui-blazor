namespace Microsoft.Fast.Components.FluentUI;

public record CommunicationToastParameters
{
    public ToastIntent Intent { get; init; }
    public string? Title { get; init; }
    public ToastEndContentType EndContentType { get; init; } = ToastEndContentType.Dismiss;
    public string? Subtitle { get; init; }
    public string? Details { get; init; }
    public ToastAction? PrimaryAction { get; init; }
    public ToastAction? SecondaryAction { get; init; }

    public ToastParameters Parameters
    {
        get
        {
            ToastParameters _parameters = new();

            _parameters.Add(nameof(Intent), Intent);
            _parameters.Add(nameof(Title), Title);
            _parameters.Add(nameof(EndContentType), EndContentType);
            _parameters.Add(nameof(Subtitle), Subtitle);
            _parameters.Add(nameof(Details), Details);
            _parameters.Add(nameof(PrimaryAction), PrimaryAction);
            _parameters.Add(nameof(SecondaryAction), SecondaryAction);

            return _parameters;
        }
    }

}
