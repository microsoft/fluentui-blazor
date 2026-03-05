// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentToastTitle is a component that represents the title of a toast. It can contain an icon, a title, and an action.
/// </summary>
public partial class FluentToastTitle
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    /// <remarks>Use this parameter to specify child elements or markup that will be rendered within the
    /// component's body. Typically used to allow consumers of the component to provide custom UI content.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the media content to render within the component.
    /// </summary>
    /// <remarks>Use this property to provide custom media elements, such as images, icons, or videos, that
    /// will be displayed in the designated area of the component. The content is rendered as a fragment and can include
    /// any valid Blazor markup.</remarks>
    [Parameter]
    public RenderFragment? Media { get; set; }

    /// <summary>
    /// Gets or sets the content to render as the action area of the component.
    /// </summary>
    /// <remarks>Use this property to provide custom interactive elements, such as buttons or links, that
    /// appear in the action area. The content is rendered as a fragment and can include arbitrary markup or
    /// components.</remarks>

    [Parameter]
    public RenderFragment? Action { get; set; }

    /// <summary>
    /// Gets or sets the intent of the toast notification, indicating its purpose or severity.
    /// </summary>
    [CascadingParameter]
    public ToastIntent? ToastIntent { get; set; }
}
