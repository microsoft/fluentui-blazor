# Fluent UI web components library and Blazor integration investigation notes

## Summary

We wanted to look into the different approaches to offer fluent UI web components in Blazor applications. As part of that we have evaluated the vanilla experience in the editor when consumed from a Blazor application and devised a way to provide the best experience possible with a limited investment.

## Goals
* Enable Fluent UI components to be consumed in a natural way in Blazor applications.
  * Intellisense for common fluent UI web component properties.
  * Type aware attributes (no need to prefix `@` on certain attributes).
  * Event handlers.
  * Bindings.
  * Navigation integration.
  * Forms integration.
* Identify blockers within Blazor that prevent the successful usage of Fluent UI components in Blazor.
* Identify blockers within Fluent UI that prevent the successful usage of Fluent UI components in Blazor.
* Estimate of the remaining work to be done for productization.
* Guidance regarding general patterns to follow when adding a new component.

## Non-goals
* Complete implementation.
* Thorough testing or automation

## Investigation areas

We focused primarily on determining the work and viability of wrapping the existing Fluent UI web components as Blazor components. We believe this is the best approach for the following reasons:
* Offers intellisense for fluent UI component attributes like appearance.
* Offers the best experience across editors (VS, VS 4 Mac, VS Code)
* Offers C# typings for the component attributes
* Offers more flexibility in adjusting the APIs where necessary.
* Enables integration with Blazor concepts like Forms and NavLink.
* Can provide detailed doc comments about how to use a given feature/property.

We approach is two-fold:
* We enabled basic functionality like `@bind` to work against the fluent UI components in the same they work for regular HTML elements.
  * This enables `@bind` intellisense for those elements.
* We didn't need to do anything for event handlers since there doesn't seem to be any custom events defined.
  * We do have this capability if needed.

This makes the web components work with Blazor with less than desirable ergonomics:
* No intellisense for custom attributes.
* Bad experience when using string based attributes.
* No integration with framework primitives.

We solved this issues by providing a wrapping layer above the web components:
* Expose the custom attributes as component properties with proper C# types.
* Integrate with the given framework primitives (Forms, NavLink)

![Areas](https://user-images.githubusercontent.com/6995051/111641443-65695200-87ba-11eb-8a37-10a69909aecf.png)

We used the story book at [packages/web-components](https://github.com/microsoft/fluentui/tree/master/packages/web-components) and tried to replicate it to a degree using Blazor components in a Blazor server app. Along the process we defined the properties we saw on the story book examples.

![image](https://user-images.githubusercontent.com/6995051/111641716-a3ff0c80-87ba-11eb-97dd-f206c34776ad.png)

We validated that event handlers work in the way that we expect for a few elements (button, menu item)

![image](https://user-images.githubusercontent.com/6995051/111641848-c85ae900-87ba-11eb-9682-e707421a8606.png)

We validated the web component bindings work to our satisfaction

![image](https://user-images.githubusercontent.com/6995051/111642083-035d1c80-87bb-11eb-9a97-71f8ed31a428.png)

We validated that the Blazor components work with and without our forms integration

![image](https://user-images.githubusercontent.com/6995051/111642323-38696f00-87bb-11eb-940e-9087a227c8f4.png)

## General conclusions

We were able to wrap the components without much effort and following a very mechanical approach. There are some defects/discrepancies that we will need to look into on both sides, but nothing that is blocking.

## Findings

### Functional
* I couldn't get fluent-listbox the element binding working with fluent-listbox, we need to figure out how to properly bind to it (doesn't expose a value property) and how to reflect the selection within the component.
* For "list style" components, we need to ensure that updating the value also updates the selected fluent option to reflect it on the UI.
  * This enables wrappers to just deal with the value property.
  * It's not clear what the semantics are when value and the selected attribute are out of sync.
    * There should be a winner here (either value wins or selected wins and the looser gets updated automagically)

### Integration
* fluent-anchor and NavLink: Blazor provides a NavLink component that automatically applies an "active" css class to the element when the url for the link matches the url on the browser.
  * I built two components that used fluent-anchor, a simple wrapper over the web component and a version that extended NavLink to provide the same functionality but backed by fluent-anchor. I was not able to get the "active" functionality to work.
  * Ideally, users should be able to replace NavLink with FluentNavLink in their apps and enjoy the same capabilities the regular NavLink offers.

* Client side navigation: Some fluent components like fluent-menu-item seem to rely on `event.preventDefault()` which inhibits our client-side navigation mechanism. An example of this is when trying to use a fluent-anchor within a fluent-menu-item. This might not be the correct usage/combination for those two components, but it's in general something we need to look into.
  * The resulting effect is that the client navigation is ignored by Blazor, so when you click the link, nothing happens.

* Scoped css integration: Blazor offers a feature called scoped css or css isolation where when you opt-in to the feature, we automatically add a unique attribute to all the HTML elements in a Razor file and transform an associated CSS file to update the selectors to include the unique attribute selector.
  * As a result of wrapping all Fluent-UI elements within a component, this feature has no applicability since the scope does not applied to the components used in the file.
  * Is it important? Fluent UI components provide a design system targetted at offering a consistent UI experience across all components, so it's unlikely one off customizations are necessary and when they are, there are options.

### Visual
* SVG renders fine within the scenarios the storybook covers (this is a somewhat problematic area for Blazor), however there are visual differences regarding spacing (there's no spacing between svg icons and the text in elements like buttons, menu items, etc.) that I'm not sure are due to styling differences or due to the way Blazor renders the SVGs.

* Checkbox indeterminate state: I couldn't replicate the same state for the fluent-checkbox and I'm not sure if its due to a styling issue or an integration issue.

* Combobox position attribute seems to not be respected in the sense that doesn't force the position as in the storybook. It's not clear if it's an integration issue or a styling issue.

### Ergonomics
* Slotted fluent-components and child contents: We took the simplest approach to wrap the components by using "ChildContent render fragments".
  * A render fragment in Blazor represents the block inside a component tag.
  * By default is passed in as Childcontent to the component.
  * Blazor components have the ability to define multiple render fragments that users can specify via nested tags when using the components. 
    * For example, the AuthorizeView component offers fragments for Authorizing,Authorized,NotAuthorized.
  * Many fluent UI components offer slots where the consumer can put content.
  * Should we introduce additional RenderFragments for the individual slots a component offers?
    * This would enable "intellisense" for the slots. For example:
      ```html
      <FluentButton><Start>...</Start><Content>...</Content></FluentButton>
      ```
      In addition to the common form:
      ```html
      <FluentButton><span slot="start">...</span><span>...</span></FluentButton>
      ```

## General patterns for enabling new components

* Create a new `.razor` file with the component name (by convention pascal-case the custom element name)
* Create pascal-cased (by convention) properties for all the attributes the custom element exposes and add the `[Parameter]` attribute to them.
* Create a property `[Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }` to hold on to additional attributes (not defined explicitly) to flow to the component. 
  * The property name is not important.
* Create a RenderFragment property to hold on the contents passed when using the component `[Parameter] public RenderFragment ChildContent { get; set; }`
  * **The property name is important** to associate the default content implicitly.
* Render the component as follows:
  `<fluent-component attribute="@Property" @attributes="AdditionalAttributes">@ChildContent</fluent-component>`
* Can this component be used within a form/have a "value" bound to it?
  * Look at `BindAttributes.cs` to define the bindings for the custom element.
  * Check the docs [here](https://docs.microsoft.com/en-us/dotnet/api/microsoft.Fast.components.bindelementattribute.-ctor?view=aspnetcore-5.0) for details.
  * Consider extending `FluentInputBase<TValue>` to integrate with Blazor forms and validation.
* Does this component integrate with an existing primitive on the framework?
  * Strongly consider subclassing the existing primitive to offer the same functionality.

## Estimated remaining work
The following is a non-exhustive list of the work that might need to happen here:
* Additional validation
  * Fix/make sure that all components work. 
    * While we've covered all components in the prototype the quality mileage might vary and later changes might have broken previously working components.
    * It should not be hard to get any failing component to work again and we can help if you get stuck.
  * Validation on Blazor Webassembly. The existing sample covers Blazor server; we haven't validated Blazor webassembly, but there should be no functional difference between the two, this is mostly a sanity check.
* Complete component definitions:
  * We've defined the properties based on the usage in the story book samples, but the list is likely not exhaustive, so each component needs to be reviewed individually and missing properties defined and applied to the underlying element.
* Define new/missing components:
  * We didn't create a component for `fluent-design-system-provider` but this might be something you want to do.
* Potential refactorings/quality of life improvements:
  * Other than `FluentInputBase` we haven not defined base classes for fluent components.
    * It likely make sense to move some of the common component properties into a common base class as well as maybe define interfaces for common component traits to make adding new components easier.
      * The interfaces in particular might help making sure that a component is not missing a property the custom element offers and making sure the typing, etc is consistent.
  * Cleanups: General code cleanup; since this is a protoype/investigation we haven't polished the code. Some missing things here are:
    * License header
    * XML docs
* Packaging: The sample references the fluent ui web components from unpkg. Ideally the file should be included in the final nuget package as a static web asset.
  * All you need to do for this is place the file/bundle in the wwwroot.
  * It will be available for consumers under `_content/Microsoft.Components.FluentUI/relative-path-from-wwwroot.js`
  * You can change the prefix by explicitly setting `<StaticWebAssetsBasePath>` in the project file (but please avoid putting it on the root folder).
* Styles and whatnot to produce the desired look and feel
  * As per the guidance for the JS file, you can put those assets in wwwroot and they'll be packaged consumable from the nuget package at the described urls.
* Test automation: This is not something we've explored as part of this work and leave up to you to define the best approach to follow.
* (Optional) Component generation automation: Based on the time we spent to port this (around 12-16H total including defining patterns and implementation approaches) I would recommend agains trying to automate the component generation.
  * Writing the definition for the component is straightforward following the patterns for other components.
  * Component interfaces are unlikely to change over time based on our discussions.
  * Regressions due to changes will likely be caught by automation provided there is automation in place.
  * The components that are "harder" to implement are likely also "harder" to generate.
