// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public sealed class DialogInstance
{
    public DialogInstance(Type? type, DialogParameters parameters, object content, IJSObjectReference previouslyFocusedElement)
    {
        ContentType = type;
        Parameters = parameters;
        Content = content;
        Id = Parameters.Id ?? Identifier.NewId();
        PreviouslyFocusedElement = previouslyFocusedElement;
    }

    public string Id { get; }

    public Type? ContentType { get; }

    public object Content { get; internal set; } = default!;

    public IJSObjectReference PreviouslyFocusedElement { get; }

    public DialogParameters Parameters { get; internal set; }

    internal Dictionary<string, object>? GetParameterDictionary()
    {
        if (Content is null)
        {
            return null;
        }
        else
        {
            return new Dictionary<string, object> { { "Content", Content } };
        }
    }
}
