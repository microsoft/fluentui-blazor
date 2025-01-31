// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for selecting parameters for a FluentField,
/// from the component itself or from an existing Input component.
/// </summary>
internal class FluentFieldParameterSelector : IFluentField
{
    private readonly FluentField _component;

    public FluentFieldParameterSelector(FluentField component) => _component = component;

    public bool HasInputComponent => _component.InputComponent != null;

    public string? Label
    {
        get => _component.Label ?? _component.InputComponent?.Label;
        set => throw new NotSupportedException();
    }

    public RenderFragment? LabelTemplate
    {
        get => _component.LabelTemplate ?? _component.InputComponent?.LabelTemplate;
        set => throw new NotSupportedException();
    }

    public FieldLabelPosition? LabelPosition
    {
        get => _component.LabelPosition ?? _component.InputComponent?.LabelPosition ?? FieldLabelPosition.Above;
        set => throw new NotSupportedException();
    }

    public string? LabelWidth
    {
        get => _component.LabelWidth ?? _component.InputComponent?.LabelWidth;
        set => throw new NotSupportedException();
    }

    public bool? Required
    {
        get => _component.Required ?? _component.InputComponent?.Required ?? false;
        set => throw new NotSupportedException();
    }

    public bool? Disabled
    {
        get => _component.Disabled ?? _component.InputComponent?.Disabled ?? false;
        set => throw new NotSupportedException();
    }

    public string? Message
    {
        get => _component.Message ?? _component.InputComponent?.Message;
        set => throw new NotSupportedException();
    }

    public Icon? MessageIcon
    {
        get => _component.MessageIcon ?? _component.InputComponent?.MessageIcon;
        set => throw new NotSupportedException();
    }

    public RenderFragment? MessageTemplate
    {
        get => _component.MessageTemplate ?? _component.InputComponent?.MessageTemplate;
        set => throw new NotSupportedException();
    }

    public Func<bool>? MessageCondition
    {
        get => _component.MessageCondition ?? _component.InputComponent?.MessageCondition ?? (() => true);
        set => throw new NotSupportedException();
    }

    public bool? MessageState
    {
        get => _component.MessageState ?? _component.InputComponent?.MessageState;
        set => throw new NotSupportedException();
    }
}
