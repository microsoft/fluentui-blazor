// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// List of slots used in FluentUI Blazor components.
/// </summary>
public static class FluentSlot
{
    /// <summary>
    /// Slot for start icons or elements.
    /// </summary>
    public const string Start = "start";

    /// <summary>
    /// Slot for end icons or elements.
    /// </summary>
    public const string End = "end";

    /// <summary>
    /// Slot for the FluentMenuItem submenu indicator element.
    /// </summary>
    public const string Indicator = "indicator";

    /// <summary>
    /// Slot for the FluentMenuItem submenu glyph element.
    /// </summary>
    public const string SubMenuGlyph = "submenu-glyph";

    /// <summary>
    /// Slot for the FluentMenu button trigger element.
    /// </summary>
    public const string Trigger = "trigger";

    /// <summary>
    /// Slot for the FluentSlider thumb element.
    /// </summary>
    public const string Thumb = "thumb";

    /* ***************************************** */
    /* Only for FluentUI Blazor Lib internal use */
    /* ***************************************** */

    /// <summary>
    /// Slot for the label of a Fluent Field.
    /// </summary>
    internal const string FieldLabel = "label";

    /// <summary>
    /// Slot for the message of a Fluent Field.
    /// </summary>
    internal const string FieldMessage = "message";

    /// <summary>
    /// Slot for all input element included in a FluentField.
    /// </summary>
    internal const string FieldInput = "input";

    /// <summary>
    /// Slot for the FluentOption or FluentCompoundButton
    /// </summary>
    internal const string Description = "description";

    /// <summary>
    /// Slot for the sub menu element.
    /// </summary>
    internal const string SubMenu = "submenu";

    /// <summary>
    /// The slot for the SVG element used as the rating icon
    /// </summary>
    internal const string Icon = "icon";

    /// <summary>
    /// Slot for the dialog actions, such as buttons.
    /// </summary>
    internal const string DialogAction = "action";

    /// <summary>
    /// Slot for the title element.
    /// </summary>
    internal const string DialogTitle= "title";

    /// <summary>
    /// Slot for the footer element.
    /// </summary>
    internal const string DialogFooter = "footer";

    /// <summary>
    /// Slot for the close element.
    /// </summary>
    internal const string DialogClose = "close";

    /// <summary>
    /// Slot for the title action elements (e.g. Close button). When the dialog type is set to non-modal and no title action is provided, a default title action button is rendered.
    /// </summary>
    internal const string DialogTitleAction = "title-action";

    /// <summary>
    /// Slot for the expanded/collapsed element of a fluent-tree-item.
    /// </summary>
    internal const string Chevron = "chevron";

    /// <summary>
    /// Slot for the right-side element of a fluent-tree-item.
    /// </summary>
    internal const string Aside = "aside";
}
