// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Localization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for selecting parameters for a FluentField,
/// from the component itself or from an existing Input component.
/// </summary>
internal class FluentFieldParameterSelector : IFluentField
{
    private readonly FluentField _component;
    private readonly IFluentLocalizer _localizer;

    internal FluentFieldParameterSelector(FluentField component, IFluentLocalizer localizer)
    {
        _component = component;
        _localizer = localizer;
    }

    public bool HasInputComponent => _component.InputComponent != null;

    public bool FocusLost
    {
        get => _component.InputComponent?.FocusLost ?? false;
        set => throw new NotSupportedException();
    }

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
        get => _component.Message ?? _component.InputComponent?.Message ?? StateToMessage();
        set => throw new NotSupportedException();
    }

    public Icon? MessageIcon
    {
        get => _component.MessageIcon ?? _component.InputComponent?.MessageIcon ?? StateToIcon();
        set => throw new NotSupportedException();
    }

    public RenderFragment? MessageTemplate
    {
        get => _component.MessageTemplate ?? _component.InputComponent?.MessageTemplate ?? StateToMessageTemplate();
        set => throw new NotSupportedException();
    }

    public Func<IFluentField, bool>? MessageCondition
    {
        get => _component.MessageCondition ?? _component.InputComponent?.MessageCondition ?? (field => true);
        set => throw new NotSupportedException();
    }
    public FieldMessageState? MessageState
    {
        get => _component.MessageState ?? _component.InputComponent?.MessageState;
        set => throw new NotSupportedException();
    }

    private Icon? StateToIcon()
    {
        return MessageState switch
        {
            FieldMessageState.Success => FluentStatus.SuccessIcon,
            FieldMessageState.Error => FluentStatus.ErrorIcon,
            FieldMessageState.Warning => FluentStatus.WarningIcon,
            _ => null
        };
    }

    private string? StateToMessage()
    {
        return MessageState switch
        {
            FieldMessageState.Success => _localizer[LanguageResource.Field_SuccessMessage],
            FieldMessageState.Error => _localizer[LanguageResource.Field_ErrorMessage],
            FieldMessageState.Warning => _localizer[LanguageResource.Field_WarningMessage],
            _ => null
        };
    }

    private RenderFragment? StateToMessageTemplate()
    {
        return MessageState switch
        {
            FieldMessageState.Success => builder =>
            {
                builder.OpenComponent(0, typeof(FluentText));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(contentBuilder =>
                {
                    contentBuilder.AddContent(0, Message);
                }));
                builder.AddAttribute(2, "Color", Color.Success);
                builder.CloseComponent();
            }
            ,
            FieldMessageState.Error => builder =>
            {
                builder.OpenComponent(0, typeof(FluentText));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(contentBuilder =>
                {
                    contentBuilder.AddContent(0, Message);
                }));
                builder.AddAttribute(2, "Color", Color.Error);
                builder.CloseComponent();
            }
            ,
            FieldMessageState.Warning => builder =>
            {
                builder.OpenComponent(0, typeof(FluentText));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(contentBuilder =>
                {
                    contentBuilder.AddContent(0, Message);
                }));
                builder.AddAttribute(2, "Color", Color.Info);
                builder.CloseComponent();
            }
            ,
            _ => null
        };
    }
}
