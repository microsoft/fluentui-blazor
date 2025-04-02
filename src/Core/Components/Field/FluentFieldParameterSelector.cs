// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Localization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for selecting parameters for a FluentField,
/// from the component itself or from an existing FieldInput component.
/// </summary>
internal class FluentFieldParameterSelector : IFluentField
{
    private readonly FluentField _component;
    private readonly IFluentLocalizer _localizer;

    /// <summary />
    internal FluentFieldParameterSelector(FluentField component, IFluentLocalizer localizer)
    {
        _component = component;
        _localizer = localizer;
    }

    /// <summary />
    public bool HasInputComponent => _component.InputComponent != null;

    /// <summary />
    public bool FocusLost
    {
        get => _component.InputComponent?.FocusLost ?? false;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public string? Label
    {
        get => _component.Label ?? _component.InputComponent?.Label;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public RenderFragment? LabelTemplate
    {
        get => _component.LabelTemplate ?? _component.InputComponent?.LabelTemplate;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public LabelPosition? LabelPosition
    {
        get => _component.LabelPosition ?? _component.InputComponent?.LabelPosition ?? Components.LabelPosition.Above;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public string? LabelWidth
    {
        get => _component.LabelWidth ?? _component.InputComponent?.LabelWidth;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public bool? Required
    {
        get => _component.Required ?? _component.InputComponent?.Required ?? false;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public bool? Disabled
    {
        get => _component.Disabled ?? _component.InputComponent?.Disabled ?? false;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public string? Message
    {
        get => _component.Message ?? _component.InputComponent?.Message ?? StateToMessage(MessageState, _localizer);
        set => throw new NotSupportedException();
    }

    /// <summary />
    public Icon? MessageIcon
    {
        get => _component.MessageIcon ?? _component.InputComponent?.MessageIcon ?? StateToIcon(MessageState);
        set => throw new NotSupportedException();
    }

    /// <summary />
    public RenderFragment? MessageTemplate
    {
        get => _component.MessageTemplate ?? _component.InputComponent?.MessageTemplate ?? StateToMessageTemplate(MessageState, Message);
        set => throw new NotSupportedException();
    }

    /// <summary />
    public Func<IFluentField, bool>? MessageCondition
    {
        get => _component.MessageCondition ?? _component.InputComponent?.MessageCondition ?? FluentFieldCondition.Always;
        set => throw new NotSupportedException();
    }

    /// <summary />
    public MessageState? MessageState
    {
        get => _component.MessageState ?? _component.InputComponent?.MessageState;
        set => throw new NotSupportedException();
    }

    /// <summary />
    internal static Icon? StateToIcon(MessageState? state)
    {
        return state switch
        {
            Components.MessageState.Success => FluentStatus.SuccessIcon,
            Components.MessageState.Error => FluentStatus.ErrorIcon,
            Components.MessageState.Warning => FluentStatus.WarningIcon,
            _ => null
        };
    }

    /// <summary />
    internal static string? StateToMessage(MessageState? state, IFluentLocalizer localizer)
    {
        return state switch
        {
            Components.MessageState.Success => localizer[LanguageResource.Field_SuccessMessage],
            Components.MessageState.Error => localizer[LanguageResource.Field_ErrorMessage],
            Components.MessageState.Warning => localizer[LanguageResource.Field_WarningMessage],
            _ => null
        };
    }

    /// <summary />
    internal static RenderFragment? StateToMessageTemplate(MessageState? state, string? message)
    {
        return state switch
        {
            Components.MessageState.Success => builder =>
            {
                builder.OpenComponent(0, typeof(FluentText));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(contentBuilder =>
                {
                    contentBuilder.AddContent(0, message);
                }));
                builder.AddAttribute(2, "Color", Color.Success);
                builder.CloseComponent();
            }
            ,
            Components.MessageState.Error => builder =>
            {
                builder.OpenComponent(0, typeof(FluentText));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(contentBuilder =>
                {
                    contentBuilder.AddContent(0, message);
                }));
                builder.AddAttribute(2, "Color", Color.Error);
                builder.CloseComponent();
            }
            ,
            Components.MessageState.Warning => builder =>
            {
                builder.OpenComponent(0, typeof(FluentText));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(contentBuilder =>
                {
                    contentBuilder.AddContent(0, message);
                }));
                builder.AddAttribute(2, "Color", Color.Info);
                builder.CloseComponent();
            }
            ,
            _ => null
        };
    }
}
