// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the properties of a field.
/// </summary>
public interface IFluentField
{
    /// <summary>
    /// Gets a value indicating whether the input component has already lost the focus.
    /// As long as the user has been in this field at least once and has left it, this property remains false.
    /// As soon as the user leaves the field, it becomes true.
    /// </summary>
    bool FocusLost { get; }

    /// <summary>
    /// Gets or sets the text to label the input. This is usually displayed just above the input.
    /// </summary>
    string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content to label the input component.
    /// This is usually displayed just above the input: see <see cref="LabelPosition"/> 
    /// </summary>
    RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the position of the label relative to the input.
    /// </summary>
    LabelPosition? LabelPosition { get; set; }

    /// <summary>
    /// Gets or sets the width of the label.
    /// </summary>
    string? LabelWidth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element needs to have a value.
    /// </summary>
    bool? Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the form control is disabled and doesn't participate in form submission.
    /// </summary>
    bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the message to display below the field.
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    /// Gets or sets the icon to display next to the message.
    /// You can use predefined icons from the <see cref="FluentStatus"/> class.
    /// </summary>
    Icon? MessageIcon { get; set; }

    /// <summary>
    /// Gets or sets the template for the message to display below the field.
    /// </summary>
    RenderFragment? MessageTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message should be displayed.
    /// </summary>
    Func<IFluentField, bool>? MessageCondition { get; set; }

    /// <summary>
    /// Gets or sets a value that affects the content
    /// of the <see cref="Message"/> and the <see cref="MessageIcon" />.
    /// </summary>
    MessageState? MessageState { get; set; }
}
