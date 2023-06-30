namespace Microsoft.Fast.Components.FluentUI;

public class ToastParameters<TData> : ToastData
{
    ///// <summary>
    ///// The intent of the toast.
    ///// </summary>
    //public ToastIntent Intent { get; set; }

    ///// <summary>
    ///// The main text of the toast.
    ///// </summary>
    //public string? Title { get; set; }

    ///// <summary>
    ///// The type of the top Call To Action.
    ///// </summary>
    //public ToastTopCTAType TopCTAType { get; set; }

    ///// <summary>
    ///// If the top CTA is of type <see cref="ToastTopCTAType.Action"/>, this is the action to be performed.
    ///// </summary>  
    //public ToastAction? TopAction { get; set; }

    ///// <summary>
    ///// Icon to show in from of the title.
    ///// </summary>
    //public (string Name, Color Color, IconVariant Variant)? Icon { get; set; }

    ///// <summary>
    ///// The timestamp of the toast. If the top CTA is of type <see cref="ToastTopCTAType.Timestamp"/>, this is the value displayed.
    ///// </summary>
    //public DateTime Timestamp { get; set; } = DateTime.Now;

    ///// <summary>
    ///// The timeout of the toast. Defaults to the timeout of the <see cref="FluentToastContainer"/> if not specified.
    ///// </summary>
    //public int? Timeout { get; set; }

    //public ToastAction? PrimaryAction { get; set; }

    //public ToastAction? SecondaryAction { get; set; }

    /// <summary>
    /// The data to be passed to the toast content component.
    /// </summary>
    public TData Data { get; set; } = default!;

    ///// <summary>
    ///// Callback function for the result.
    ///// </summary>
    //public EventCallback<ToastResult> OnToastResult { get; set; } = default!;
}
