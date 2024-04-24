using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class ToastParameters : ComponentParameters, IToastParameters
{
    public string? Id { get; set; }
    public ToastIntent Intent { get; set; }
    public string? Title { get; set; }
    public ToastTopCTAType TopCTAType { get; set; }

    public string? TopAction { get; set; }
    public EventCallback<ToastResult>? OnTopAction { get; set; } = default!;

    public (Icon Value, Color Color)? Icon { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int? Timeout { get; set; }

    public string? PrimaryAction { get; set; }
    public EventCallback<ToastResult>? OnPrimaryAction { get; set; } = default!;

    public string? SecondaryAction { get; set; }
    public EventCallback<ToastResult>? OnSecondaryAction { get; set; } = default!;
}

public class ToastParameters<TContent> : ToastParameters, IToastParameters<TContent>
    where TContent : class
{
    /// <summary>
    /// Gets or sets the content to be shown in the toast body.
    /// </summary>
    public TContent Content { get; set; } = default!;
}
