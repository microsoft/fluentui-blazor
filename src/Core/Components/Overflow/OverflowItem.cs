// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents an item that may be subject to overflow handling, typically used in scenarios where content or data
/// exceeds a predefined limit. Used by other components besides FluentOverflow to manage overflow state.
/// </summary>
public class OverflowItem
{
    /// <summary />
    public string? Id { get; set; }

    /// <summary />
    public bool? Overflow { get; set; }

    /// <summary />
    public string? Text { get; set; }
}
