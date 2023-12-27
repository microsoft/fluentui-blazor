using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class ApiComplexClassItem
{
    [Parameter]
    [EditorRequired]
    public PropertyChildren Property { get; set; } = default!;
}
