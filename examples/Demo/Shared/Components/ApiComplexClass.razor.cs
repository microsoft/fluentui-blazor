using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class ApiComplexClass
{
    [Parameter]
    [EditorRequired]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public object Item { get; set; } = default!;

    public IEnumerable<PropertyChildren>? Properties { get; set; }

    [Parameter]
    public EventCallback<PropertyChildren> OnChanged { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Properties = Item.GetType().GetPropertyChildren().ToArray();
            StateHasChanged();
        }
    }
}
