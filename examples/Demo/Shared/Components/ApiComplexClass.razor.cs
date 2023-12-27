using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class ApiComplexClass
{
    [Parameter]
    [EditorRequired]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Item { get; set; } = default!;

    public IEnumerable<PropertyChildren>? Properties { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Properties = Item.GetPropertyChildren().ToArray();
            StateHasChanged();
        }
    }
}
