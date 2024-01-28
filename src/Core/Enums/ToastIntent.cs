namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public enum ToastIntent
{
    /// <summary>
    /// Positive confirmation of status, process, or operation.The person or task can proceed.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates possible or upcoming issues that need to be addressed. It’s a signal that 
    /// something isn’t quite right.The person can continue without addressing it, but warnings 
    /// can become errors.
    /// </summary>
    Warning,

    /// <summary>
    /// Negative status due to an incomplete process or a failed task.The person can continue 
    /// using the app, but the item with the error will need to be addressed.
    /// </summary>
    Error,

    /// <summary>
    /// Neutral status and information related to the person’s action.
    /// </summary>
    Info,

    /// <summary>
    /// Informs that an operation someone kicked off is in progress.The person can continue using
    /// the app while the process continues.They can also cancel the process.
    /// </summary>
    Progress,

    /// <summary>
    /// Informs that an upload operation someone kicked off is in progress.The person can continue using
    /// the app while the process continues.They can also cancel the process.
    /// </summary>
    Upload,

    /// <summary>
    /// Informs that a dowload operation someone kicked off is in progress.The person can continue using
    /// the app while the process continues.They can also cancel the process.
    /// </summary>
    Download,

    /// <summary>
    /// Informs about an operation concerning an event. 
    /// </summary>
    Event,

    /// <summary>
    /// Mention is used when another person performs an action related to the user or their account.
    /// For example, the user receives a meeting invitation or is mentioned in a chat. 
    /// </summary>
    Mention,

    /// <summary>
    /// Informs about a custom event or operation. 
    /// </summary>
    Custom
}
