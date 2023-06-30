using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class ToastParameters : ComponentParameters, IToastParameters
{
    public string? Id { get; set; }
    public ToastIntent Intent { get; set; }
    public string? Title { get; set; }
    public ToastTopCTAType TopCTAType { get; set; }

    public ToastAction? TopAction { get; set; }
    public EventCallback<ToastResult> OnTopAction { get; set; } = default!;

    public (string Name, Color Color, IconVariant Variant)? Icon { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public int? Timeout { get; set; }

    public ToastAction? PrimaryAction { get; set; }
    public EventCallback<ToastResult> OnPrimaryAction { get; set; } = default!;

    public ToastAction? SecondaryAction { get; set; }
    public EventCallback<ToastResult> OnSecondaryAction { get; set; } = default!;
}

public class ToastParameters<TToastContent> : ToastParameters
    where TToastContent : class
{
    /// <summary>
    /// The data to be passed to the toast content component.
    /// </summary>
    public TToastContent ToastContent { get; set; } = default!;
}
