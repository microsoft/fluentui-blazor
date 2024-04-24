namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Call To Action type for the top action of the toast.
/// </summary>  
public enum ToastTopCTAType
{
    /// <summary>
    /// Toast shows a dismiss button after the title.
    /// </summary>
    Dismiss,

    /// <summary>
    /// Toast shows a call to action after the title
    /// </summary>
    Action,

    /// <summary>
    /// Toast shows a timestamp after the title.
    /// </summary>
    Timestamp,
}
