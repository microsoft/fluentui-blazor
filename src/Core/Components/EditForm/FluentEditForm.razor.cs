// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentEditForm
{
    private EditContext? _editContext;
    private bool _hasSetEditContextExplicitly;

    private EditForm _editForm = null!;

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created <c>form</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Supplies the edit context explicitly. If using this parameter, do not
    /// also supply <see cref="Model"/>, since the model value will be taken
    /// from the <see cref="EditContext.Model"/> property.
    /// </summary>
    [Parameter]
    public EditContext? EditContext
    {
        get => _editContext;
        set
        {
            _editContext = value;
            _hasSetEditContextExplicitly = value != null;
        }
    }

    /// <summary>
    /// If enabled, form submission is performed without fully reloading the page. This is
    /// equivalent to adding <code>data-enhance</code> to the form.
    ///
    /// This flag is only relevant in server-side rendering (SSR) scenarios. For interactive
    /// rendering, the flag has no effect since there is no full-page reload on submit anyway.
    /// </summary>
    [Parameter] public bool Enhance { get; set; }

    /// <summary>
    /// Specifies the top-level model object for the form. An edit context will
    /// be constructed for this model. If using this parameter, do not also supply
    /// a value for <see cref="EditContext"/>.
    /// </summary>
    [Parameter] public object? Model { get; set; }

    /// <summary>
    /// Specifies the content to be rendered inside this <see cref="EditForm"/>.
    /// </summary>
    [Parameter] public RenderFragment<EditContext>? ChildContent { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted.
    ///
    /// If using this parameter, you are responsible for triggering any validation
    /// manually, e.g., by calling <see cref="EditContext.Validate"/>.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted and the
    /// <see cref="EditContext"/> is determined to be valid.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted and the
    /// <see cref="EditContext"/> is determined to be invalid.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Gets or sets the form handler name. This is required for posting it to a server-side endpoint.
    /// It is not used during interactive rendering.
    /// </summary>
    [Parameter] public string? FormName { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (_hasSetEditContextExplicitly && Model != null)
        {
            throw new InvalidOperationException($"{nameof(EditForm)} requires a {nameof(Model)} " +
                $"parameter, or an {nameof(EditContext)} parameter, but not both.");
        }
        else if (!_hasSetEditContextExplicitly && Model == null)
        {
            throw new InvalidOperationException($"{nameof(EditForm)} requires either a {nameof(Model)} " +
                $"parameter, or an {nameof(EditContext)} parameter, please provide one of these.");
        }

        // Update _editContext if we don't have one yet, or if they are supplying a
        // potentially new EditContext, or if they are supplying a different Model
        if (Model != null && Model != _editContext?.Model)
        {
            _editContext = new EditContext(Model!);
        }
    }
}
