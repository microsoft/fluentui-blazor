## Design Token support
 
The Fluent UI Blazor Components are built on FAST's Adaptive UI technology, which enables design customization and personalization, while automatically 
maintaining accessibility. This is accomplished through setting various "Design Tokens". In earlier versions of this library, the only way to manipulate the 
design tokens was through using the `<FluentDesignSystemProvider>` component. This Blazor component (and it's underlying Web Component) exposed a little 
over 60 variables that could be used to change things like typography, color, sizes, UI spacing, etc. FAST has been extended a while ago and now has a much 
more granular way of working with individual design tokens instead of just through a design system provider model. See [this page on learn.microsoft.com](https://learn.microsoft.com/en-us/fluent-ui/web-components/design-system/design-tokens) 
for more information on how Design Tokens work. 

In total there are now over 160 distinct design tokens defined in the FAST model and you can use all of these from Blazor, both from C# code as in a declarative way in your .razor pages.

### Using Design Tokens from code
 
Given the following .razor page fragment:

```cshtml
<FluentButton @ref="ref1" Appearance="Appearance.Filled">A button</FluentButton>
<FluentButton @ref="ref2" Appearance="Appearance.Filled">Another button</FluentButton>
<FluentButton @ref="ref3" Appearance="Appearance.Filled">And one more</FluentButton>
<FluentButton @ref="ref4" Appearance="Appearance.Filled" @onclick=OnClick>Last button</FluentButton>
```

You can use Design Tokens to manipulate the styles from C# code as follows:

```csharp
[Inject]
private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

[Inject]
private AccentBaseColor AccentBaseColor { get; set; } = default!;

[Inject]
private BodyFont BodyFont { get; set; } = default!;

[Inject]
private StrokeWidth StrokeWidth { get; set; } = default!;

[Inject]
private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

private FluentButton? ref1;
private FluentButton? ref2;
private FluentButton? ref3;
private FluentButton? ref4;

protected override async Task OnAfterRenderAsync(bool firstRender)
{
if (firstRender)
{
	//Set to dark mode
	await BaseLayerLuminance.SetValueFor(ref1!.Element, (float)0.15);

	//Set to Excel color
	await AccentBaseColor.SetValueFor(ref2!.Element, "#185ABD".ToSwatch());

	//Set the font
	await BodyFont.SetValueFor(ref3!.Element, "Comic Sans MS");

	//Set 'border' width for ref4
	await StrokeWidth.SetValueFor(ref4!.Element, 7);
	//And change conrner radius as well
	await ControlCornerRadius.SetValueFor(ref4!.Element, 15);

	StateHasChanged();
}

}

public async Task OnClick()
{
	//Remove the wide border
	await StrokeWidth.DeleteValueFor(ref4!.Element);
}
```

As can be seen in the code above (with the `ref4.Element`), it is posible to apply multiple tokens to the same component.
 
For Design Tokens that work with a color value, you must call the `ToSwatch()` extension method on a string value or use one of the Swatch constructors. This 
makes sure the color is using a format that Design Tokens can handle. A Swatch has a lot of commonality with the `System.Drawing.Color` struct. Instead of 
the values of the components being between 0 and 255, in a Swatch the components are expressed as a value between 0 and 1.

> **Important**
> 
> 
> **The Design Tokens are manipulated through JavaScript interop working with an `ElementReference`. There is no JavaScript element until after the component 
is rendered. This means you can only work with the Design Tokens from code after the component has been rendered in `OnAfterRenderAsync` and not in any earlier 
lifecycle methods**.

### Using Design Tokens as components
The Design Tokens can also be used as components in a `.razor` page directely. It looks like this:

```html
<BaseLayerLuminance Value="(float?)0.15">
	<FluentCard ParentReference="@context">
		<div class="contents">
			Dark
			<FluentButton Appearance="Appearance.Accent">Accent</FluentButton>
			<FluentButton Appearance="Appearance.Stealth">Stealth</FluentButton>
			<FluentButton Appearance="Appearance.Outline">Outline</FluentButton>
			<FluentButton Appearance="Appearance.Lightweight">Lightweight</FluentButton>
		</div>
	</FluentCard>
</BaseLayerLuminance>
```

To make this work, a link needs to be created between the Design Token component and its child components. This is done with the `ParentReference="@context"` construct. 

> **Note**
> 
> Only one Design Token component at a time can be used this way. If you need to set more tokens, use the code approach as described in Option 1 above.


### Using the `<FluentDesignSystemProvider>`
Another way to customize the design in Blazor is to wrap the entire block you want to manipulate in a `<FluentDesignSystemProvider>`. This special element 
has a number of properties you can set to configure a subset of the tokens. **Not all tokens are available/supported** and we recommend this to only be 
used as a fall-back mechanism. The preferred method of working with the design tokens is to manipulate them from code as described above. 

Here's an example of changing the "accent base color" and switching the system into dark mode (in the file `app.razor`):

```html
<FluentDesignSystemProvider AccentBaseColor="#464EB8" BaseLayerLuminance="0">
	<Router AppAssembly="@typeof(App).Assembly">
		<Found Context="routeData">
			<RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
		</Found>
		<NotFound>
			<PageTitle>Not found</PageTitle>
			<LayoutView Layout="@typeof(MainLayout)">
				<p role="alert">Sorry, there's nothing at this address.</p>
			</LayoutView>
		</NotFound>
	</Router>
</FluentDesignSystemProvider>
```

> **Note**
> 
> FluentDesignSystemProvider token attributes can be changed on-the-fly like any other Blazor component attribute.

## Colors for integration with specific Microsoft products
If you are configuring the components for integration into a specific Microsoft product, the following table provides `AccentBaseColor` values you can use. 
*The library offers an `OfficeColor` enumeration which contains the specific accent colors for 17 different Office applications.*

Product | AccentBaseColor
------- | ---------------
| Office | #D83B01 |
| Word | #185ABD |
| Excel | #107C41 |
| PowerPoint | #C43E1C |
| Teams | #6264A7 |
| OneNote | #7719AA |
| SharePoint | #03787C |
| Stream | #BC1948 |

