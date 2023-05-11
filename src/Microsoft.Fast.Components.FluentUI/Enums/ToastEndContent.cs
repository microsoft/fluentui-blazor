using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum ToastEndContent
{
    /// <summary>
    /// Toast shows a call to action after the title
    /// </summary>
    Action,

    /// <summary>
    /// Toast shows a dismiss button after the title.
    /// </summary>
    Dismiss,

    /// <summary>
    /// Toast shows a timestamp after the title.
    /// </summary>
    Timestamp,
}
