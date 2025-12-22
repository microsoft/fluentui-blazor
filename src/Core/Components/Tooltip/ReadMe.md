# Add ITooltipComponent and common properties

In many components, it is often useful to use a tooltip to provide more information about an element.
Having a simpler way than creating a `FluentTooltip` will be interesting for the developer.
Several other libraries, for example, have components with a `Tooltip` parameter that you simply
need to complete for the entire mechanism to be set up.

```xml
<FluentButton Tooltip="Example of tooltip">Default</FluentButton>
              ^^^^^^^
```

To implement this, you need to implement the interface `ITooltipComponent` on component which need this `Tooltip` parameter,
and to add this code to render the tooltip.

```csharp
/// <inheritdoc cref="ITooltipComponent.Tooltip" />
[Parameter]
public string? Tooltip { get; set; }

/// <summary />
protected override async Task OnInitializedAsync()
{
    await base.RenderTooltipAsync(Tooltip);
}
```

> See a working example in the `FluentButton` component.

A global unit test will validate all components which implement this interface, to verify that the tooltip is rendered correctly.
