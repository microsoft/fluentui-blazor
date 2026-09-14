// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class DynamicComponentRenderer
{
    // TODO: When ASP.NET Core's parameter-assignment path preserves the DynamicallyAccessedMembers
    // annotation on DynamicComponent.Type without IL2110/IL2111, replace this helper's call sites with
    // DynamicComponent and delete this helper. Keep the component-type annotations: DynamicComponent.Type
    // has the same member-preservation requirement.
    public static void RenderDynamicComponent(
        this RenderTreeBuilder builder,
        int sequence,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType,
        IEnumerable<KeyValuePair<string, object?>>? parameters,
        object? key = null)
    {
        builder.OpenComponent(sequence, componentType);

        if (key is not null)
        {
            builder.SetKey(key);
        }

        if (parameters is not null)
        {
            foreach (var parameter in parameters)
            {
                builder.AddComponentParameter(sequence + 1, parameter.Key, parameter.Value);
            }
        }

        builder.CloseComponent();
    }
}