namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public enum MessageBoxIntent
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
    /// THe message box is used to ask the user for confirmation.
    /// </summary>
    Confirmation,

    /// <summary>
    /// Informs about a custom event or operation. 
    /// </summary>
    Custom
}
