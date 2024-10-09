## V4.10.2

### General
- \[General\] Update to latest .NET 8 and .NET 9 RC2 SDK. 
- \[Debounce\] Fix the Debounce class with async methods ([#2759](https://github.com/microsoft/fluentui-blazor/pull/2759))

### Components
- \[Button\] Add StopPropagation + UnitTests ([#2732](https://github.com/microsoft/fluentui-blazor/pull/2732))
- \[DataGrid\] Add `HeaderTootip` to columns to allow for custom header tooltip text ([#2775](https://github.com/microsoft/fluentui-blazor/pull/2775))
- \[DataGrid\] Changes related to loading behavior ([#2739](https://github.com/microsoft/fluentui-blazor/pull/2739))
- \[DataGrid\] Fix disposed object access error in EntityFrameworkAdapter [#2769] ([#2781](https://github.com/microsoft/fluentui-blazor/pull/2781))
- \[DataGrid\] Support for multiple IAsyncQueryExecutor registrations ([#2730](https://github.com/microsoft/fluentui-blazor/pull/2730))
- \[FluentNavLink\] An issue where empty strings were not allowed in Href ([#2722](https://github.com/microsoft/fluentui-blazor/pull/2722))
- \[FluentRadioGroup\] Fix binding error ([#2742](https://github.com/microsoft/fluentui-blazor/pull/2742))
- \[Menu\] Add z-index to MenuProvider ([#2772](https://github.com/microsoft/fluentui-blazor/pull/2772))
- \[MenuButton\] Fix the MenuItem OnClick used with MenuButton ([#2784](https://github.com/microsoft/fluentui-blazor/pull/2784))
- \[MessageBar\] Update way animation is applied to the MessageBar ([#2723](https://github.com/microsoft/fluentui-blazor/pull/2723))
- \[NavMenu\] Make submenu in collapsed state work again ([#2771](https://github.com/microsoft/fluentui-blazor/pull/2771))
- \[Overlay\] Fix the detection of ExcludedElement for WASM ([#2755](https://github.com/microsoft/fluentui-blazor/pull/2755))
- \[ProfileMenu\] Add `Open` attribute ([#2741](https://github.com/microsoft/fluentui-blazor/pull/2741))
- \[Providers\] To prevent the menu and tooltip from displaying a scrollbar in body ([#2744](https://github.com/microsoft/fluentui-blazor/pull/2744))
- \[Tooltip\] Add role so screen reader can announce tooltip text ([#2724](https://github.com/microsoft/fluentui-blazor/pull/2724))
- \[TreeView\] Improve RTL support ([#2770](https://github.com/microsoft/fluentui-blazor/pull/2770))
- \[TreeView\] prevent runtime errors in change handeling ([#2776](https://github.com/microsoft/fluentui-blazor/pull/2776))

### Demo site and documentation
- \[Docs\] Components inheriting ListComponentBase missing member descriptions  ([#2735](https://github.com/microsoft/fluentui-blazor/pull/2735))
- \[Docs\] Fix nullable reference types not always showing in API documentation ([#2758](https://github.com/microsoft/fluentui-blazor/pull/2758))
- \[Docs\] Fixes missing method descriptions for APIs ([#2764](https://github.com/microsoft/fluentui-blazor/pull/2764))
- \[Docs\] Removes extra period from TemplatesPage.razor ([#2777](https://github.com/microsoft/fluentui-blazor/pull/2777))
- \[Docs\] Removes unnecessary output to console for Grid page ([#2767](https://github.com/microsoft/fluentui-blazor/pull/2767))


### Icons
- Update to Fluent UI System Icons 1.1.260 (changes since 1.1.258). New **color** icons have been added!
    
    **What's new (Name / Size(s) / Variant(s))**
    - Arrow Circle Up Left / 16 / Filled & Regular
    - Arrow Circle Up Right / 16 / Filled & Regular
    - Building Checkmark / 16, 20 / Filled & Regular
    - Clock Alarm / 48 / Filled & Regular
    - Clothes Hanger / 12, 16, 20, 24 / Filled & Regular
    - Comment Quote / 16, 20, 24, 28 / Filled & Regular
    - Comment Text / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Glance Horizontal / 28, 48 / Filled & Regular
    - Glance / 16, 28, 32, 48 / Filled & Regular
    - Megaphone / 12 / Filled & Regular
    - Mic Link / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Pen Sync / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - People Link / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - People Queue / 28, 32, 48 / Filled & Regular
    - Person Head Hint / 16, 20, 24 / Filled & Regular
    - Person Sound Spatial / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Sound Wave Circle Sparkle / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Sound Wave Circle / 16, 28, 32, 48 / Filled & Regular

    **What's updated (Name / Size(s) / Variant(s))**

    - Comment / 12 / Filled & Regular
    - TV USB / 16, 48 / Filled & Regular

    **New style — Color!** 

    - Add Circle / 16, 20, 24, 28, 32 / Color
    - Alert / 16, 20, 24, 28, 32, 48 / Color
    - Approvals App / 16, 20, 24, 28, 32 / Color
    - Apps / 16, 20, 24, 28, 32, 48 / Color
    - Arrow Trending Lines / 20, 24 / Color
    - Beach / 16, 20, 24, 28, 32, 48 / Color
    - Building Multiple / 20, 24 / Color
    - Building People / 16, 20, 24 / Color
    - Building Store / 16, 20, 24 / Color
    - Building / 16, 20, 24, 32, 48 / Color
    - Calendar Cancel / 16, 20, 24 / Color
    - Calendar Checkmark / 16, 20, 24 / Color
    - Calendar Clock / 16, 20, 24 / Color
    - Calendar People / 20 / Color
    - Calendar / 16, 20, 24, 28, 32, 48 / Color
    - Camera / 16, 20, 24 / Color
    - Chat Bubbles Question / 16, 20, 24 / Color
    - Chat More / 16, 20, 24 / Color
    - Chat Multiple / 16, 20, 24 / Color
    - Checkbox Person / 16, 20, 24 / Color
    - Checkbox / 16, 20, 24 / Color
    - Checkmark Circle / 16, 20, 24, 32, 48 / Color
    - Clipboard Text Edit / 20, 24, 32 / Color
    - Clipboard / 16, 20, 24, 28, 32, 48 / Color
    - Clock Alarm / 16, 20, 24, 32, 48 / Color
    - Cloud Dismiss / 16, 20, 24, 28, 32, 48 / Color
    - Cloud / 16, 20, 24, 28, 32, 48 / Color
    - Code Block / 16, 20, 24, 28, 32, 48 / Color
    - Coin Multiple / 16, 20 / Color
    - Data Bar Vertical Ascending / 16, 20, 24 / Color
    - Dismiss Circle / 16, 20, 24, 28, 32, 48 / Color
    - Document Add / 16, 20, 24, 28, 48 / Color
    - Document Folder / 16, 20, 24 / Color
    - Document Lock / 16, 20, 24, 28, 48 / Color
    - Document / 16, 20, 24, 28, 32, 48 / Color
    - Edit / 16, 20, 24, 32 / Color
    - Error Circle / 16, 20, 24, 48 / Color
    - Food / 16, 20, 24, 28, 32, 48 / Color
    - Game Chat / 20 / Color
    - Globe Shield / 20, 24, 48 / Color
    - Headphones / 20, 24, 28, 32, 48 / Color
    - Headset / 16, 20, 24, 28, 32, 48 / Color
    - History / 16, 20, 24, 28, 32, 48 / Color
    - Home / 16, 20, 24, 28, 32, 48 / Color
    - Library / 16, 20, 24, 28, 32 / Color
    - Mail Multiple / 16, 20, 24, 28, 32 / Color
    - Mail / 16, 20, 24, 28, 32, 48 / Color
    - Mic / 16, 20, 24, 28, 32, 48 / Color
    - Org / 16, 20, 24, 28, 32, 48 / Color
    - People Home / 16, 20, 24, 28, 32, 48 / Color
    - People Team / 16, 20, 24, 28, 32, 48 / Color
    - People / 16, 20, 24, 28, 32, 48 / Color
    - Person Available / 16, 20, 24 / Color
    - Person / 16, 20, 24, 28, 32, 48 / Color
    - Pin / 16, 20, 24, 28, 32, 48 / Color
    - Poll / 16, 20, 24, 32 / Color
    - Question Circle / 16, 20, 24, 28, 32, 48 / Color
    - Receipt / 16, 20, 24, 28, 32 / Color
    - Reward / 16, 20, 24 / Color
    - Scan Person / 16, 20, 24, 28, 48 / Color
    - Scan Type / 20, 24 / Color
    - Search Visual / 16, 20, 24 / Color
    - Shield Checkmark / 16, 20, 24, 28, 48 / Color
    - Shield / 16, 20, 24, 28, 32, 48 / Color
    - Shifts / 16, 20, 24, 28, 32 / Color
    - Text Edit Style / 16, 20, 24 / Color
    - Vault / 16, 20, 24 / Color
    - Video / 16, 20, 24, 28, 32, 48 / Color
    - Warning / 16, 20, 24, 28, 32, 48 / Color
    - Wrench / 16, 20, 24 / Color

## v4.10.1
For a complete list of changes in this release, also see the [4.10.1](https://github.com/microsoft/fluentui-blazor/releases) release page on GitHub

### General
- \[General\] Update to .NET 9 RC1 SDK ([#2639](https://github.com/microsoft/fluentui-blazor/pull/2639))
- \[Github\] Update the actions/upload-artifact to v4 ([#2650](https://github.com/microsoft/fluentui-blazor/pull/2650))

## Accessiblity
- \[DateTime\] Add `role`, `tabindex` and catch Enter/Space ([#2688](https://github.com/microsoft/fluentui-blazor/pull/2688))
- \[Select\] Fix the unannounced `Required` keyword with FluentSelect ([#2706](https://github.com/microsoft/fluentui-blazor/pull/2706))

### Components
- \[Anchor\] Revert #2624 and replace with a better solution ([#2640](https://github.com/microsoft/fluentui-blazor/pull/2640))
- \[Autocomplete\] Fix Autocomplete Aria-Hidden and Focusable ([#2648](https://github.com/microsoft/fluentui-blazor/pull/2648))
- \[Button\] Remove style override when in loading state ([#2686](https://github.com/microsoft/fluentui-blazor/pull/2686))
- \[Calendar\] Add ReadOnly in day click/keydown logic ([#2720](https://github.com/microsoft/fluentui-blazor/pull/2720))
- \[Combobox\] Add Combobox Immediate property ([#2685](https://github.com/microsoft/fluentui-blazor/pull/2685))
- \[DataGrid\] Add a Selectable function parameter to SelectColumn ([#2709](https://github.com/microsoft/fluentui-blazor/pull/2709))
- \[DataGrid\] EF Core Adapter - A second operation was started ([#2653](https://github.com/microsoft/fluentui-blazor/pull/2653))
- \[DataGrid\] Fix column options popup being blank ([#2674](https://github.com/microsoft/fluentui-blazor/pull/2674))
- \[DataGrid\] Public method override additions ([#2711](https://github.com/microsoft/fluentui-blazor/pull/2711))
- \[Debounce\] Replace `Debouncer` with the new `DebounceTask` v5 ([#2678](https://github.com/microsoft/fluentui-blazor/pull/2678))
- \[Design\] Fix fluent-design-system-provider attribute names ([#2693](https://github.com/microsoft/fluentui-blazor/pull/2693))
- \[Label\] Add FluentLabel.Id ([#2704](https://github.com/microsoft/fluentui-blazor/pull/2704))
- \[Lists\] Fix reset issue ([#2660](https://github.com/microsoft/fluentui-blazor/pull/2660))
- \[MenuButton\] Add an optional start icon ([#2707](https://github.com/microsoft/fluentui-blazor/pull/2707))
- \[MessageBar\] Add an optional toggle for the fade in animation ([#2716](https://github.com/microsoft/fluentui-blazor/pull/2716))
- \[NumberField\] Use error outline when invalid ([#2705](https://github.com/microsoft/fluentui-blazor/pull/2705))
- \[ProfileMenu\] Fix the `TopCorner` style ([#2632](https://github.com/microsoft/fluentui-blazor/pull/2632))
- \[RadioGroup\] Fix keyboard selection ([#2663](https://github.com/microsoft/fluentui-blazor/pull/2663))
- \[Select\] Change height in listbox style to use fit-content when `Height` value is provided ([#2680](https://github.com/microsoft/fluentui-blazor/pull/2680))
- \[Slider\] Fix FluentSlider two-way binding issue ([#2665](https://github.com/microsoft/fluentui-blazor/pull/2665))
- \[SplitPanel\] Remove console logging ([#2636](https://github.com/microsoft/fluentui-blazor/pull/2636))

### Demo site and documentation
- \[Docs\] Add MarkupString to DemoSearch ([#2626](https://github.com/microsoft/fluentui-blazor/pull/2626))
- \[Docs\] Fix missing hljs object ([#2631](https://github.com/microsoft/fluentui-blazor/pull/2631))
- \[Docs\] Add FluentMenuProvider notes ([#2649](https://github.com/microsoft/fluentui-blazor/pull/2649))
- \[Docs\] Blazor Hybrid code snippet reformat ([#2673](https://github.com/microsoft/fluentui-blazor/pull/2673))

### Icons
- Update to Fluent UI System Icons 1.1.258 (changes since 1.1.256)
    
    **What's new (Name / Size(s) / Variant(s))**
    - Add Circle / 48 / Filled & Regular
    - Alert / 32 / Light
    - Book Number / 16 / Filled
    - Clipboard Text Edit / 48 / Filled & Regular
    - Design Ideas / 28, 32, 48 / Filled & Regular
    - Document Folder / 28, 32, 48 / Filled & Regular
    - Eye Off / 32 / Filled & Light & Regular
    - Eye / 32 / Light
    - Learning App / 16 / Filled & Regular
    - Newsletter / 32 / Light
    - Receipt / 48 / Filled & Regular
    - Video Bluetooth / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Video USB / 16, 20, 24, 28, 32, 48 / Filled & Regular
 
    **What's updated (Name / Size(s) / Variant(s))**
    - Approvals App / 32, 48 / Filled & Regular
    - Design Ideas / 16, 20, 24 / Filled & Regular
    - Puzzle Piece Shield / 20 / Filled & Regular
    - Speaker Bluetooth / 28 / Filled
    
## v4.10.0

### Important notes

With version 4.10 (and higher), we've introduced a new provider:
`FluentMenuProvider` which corrects some menu positioning problems.
See [#2510](https://github.com/microsoft/fluentui-blazor/pull/2510)

By default, this provider is enabled. You must therefore add `<FluentMenuProvider />` 
to your application. See [Code Setup](https://www.fluentui-blazor.net/CodeSetup)
page of the documentation for more information.

### Components
- \[General\] Add .net9.0 as targeted framework ([#2590](https://github.com/microsoft/fluentui-blazor/pull/2590))
- \[Javascript Caching\] Do not add version number to lib.module.js file ([#2572](https://github.com/microsoft/fluentui-blazor/pull/2572))

- \[Anchor\] Fix hypertext appearance with icon in start/end slot ([#2624](https://github.com/microsoft/fluentui-blazor/pull/2624))
- \[Autocomplete\] Add missing AdditionalAttributes ([#2522](https://github.com/microsoft/fluentui-blazor/pull/2522))
- \[Checkbox\] Fix looping value error when used in a Stack ([#2417](https://github.com/microsoft/fluentui-blazor/pull/2417))
- \[DataGrid\] Add AutoFit feature to size columns automatically as good as possible ([#2496](https://github.com/microsoft/fluentui-blazor/pull/2496))
- \[DataGrid\] Add parameter to provide labels to resize UI ([#2585](https://github.com/microsoft/fluentui-blazor/pull/2585))
- \[DataGrid\] Don't hover on header row (no PR)
- \[DataGrid\] Fix pagination when using itemprovider ([#2530](https://github.com/microsoft/fluentui-blazor/pull/2530))
- \[DataGrid\] Fix resizing to initial column widths issue ([#2561](https://github.com/microsoft/fluentui-blazor/pull/2561))
- \[DataGrid\] Improve data refresh logic ([#2512](https://github.com/microsoft/fluentui-blazor/pull/2512))
- \[DataGrid\] Provide new way to render column actions ([#2586](https://github.com/microsoft/fluentui-blazor/pull/2586))
- \[DataGrid\] Trigger OnRowClick on keyboard enter in DataGrid row ([#2577](https://github.com/microsoft/fluentui-blazor/pull/2577))
- \[DatePicker\] Added OnDoubleClick event and DoubleClickToDate parameter ([#2567](https://github.com/microsoft/fluentui-blazor/pull/2567))
- \[Dialog\] Fix regression, see [#2542](https://github.com/microsoft/fluentui-blazor/pull/2542) for details ([#2568](https://github.com/microsoft/fluentui-blazor/pull/2568))
- \[Dialog\] Make dismiss button larger and use neutral color ([#2565](https://github.com/microsoft/fluentui-blazor/pull/2565))
- \[Dialog\] Remove tabindex=-1 on fluent-dialog tag ([#2584](https://github.com/microsoft/fluentui-blazor/pull/2584))
- \[DragContainer\] Add an event "onDragEnd" ([#2504](https://github.com/microsoft/fluentui-blazor/pull/2504))
- \[InputBase\] Force `EditContext` to be re-associated with the Dispatcher ([#2620](https://github.com/microsoft/fluentui-blazor/pull/2620))
- \[Menu\] Add a FluentMenuProvider ([#2510](https://github.com/microsoft/fluentui-blazor/pull/2510))
- \[Menu\] Pass through more parameters to region the menu is anchored to ([#2579](https://github.com/microsoft/fluentui-blazor/pull/2579))
- \[MessageBar\] Add id attribute ([#2505](https://github.com/microsoft/fluentui-blazor/pull/2505))
- \[NumberField\] Add AutoComplete parameter  ([#2560](https://github.com/microsoft/fluentui-blazor/pull/2560))
- \[Overlay\] Add Interactive and InteractiveExceptId parameters ([#2580](https://github.com/microsoft/fluentui-blazor/pull/2580))
- \[SplashScreen\] Pass parameters.Modal to `ShowSplashScreen...` methods ([#2449](https://github.com/microsoft/fluentui-blazor/pull/2449))
- \[Templates\] Fix Template.Client services injection ([#2485](https://github.com/microsoft/fluentui-blazor/pull/2485))
- \[Templates\] Fix some whitespace errors in generated .csproj files (no PR)
- \[Templates\] Use latest SDK packages (no PR)
- \[Toast\] Width issue when using timestamp in i18n ([#2508](https://github.com/microsoft/fluentui-blazor/pull/2508))
- \[Wizard\] Fix Done button when last step is disabled ([#2503](https://github.com/microsoft/fluentui-blazor/pull/2503))

### Demo site and documentation
- \[Docs\] Add some extra information to TreeView page (no PR)
- \[Docs\] Fix a typo ([#2518](https://github.com/microsoft/fluentui-blazor/pull/2518))
- \[Docs\] Fix typo in TemplatesPage.razor ([#2452](https://github.com/microsoft/fluentui-blazor/pull/2452))
- \[Docs\] Improve warning for required interactivity ([#2469](https://github.com/microsoft/fluentui-blazor/pull/2469))
- \[Examples\] Add 2024 Olymics data and let examples use that

### Icons
- Update to Fluent UI System Icons 1.1.256 (since 1.1.249)
    
    **What's new (Name / Size(s) / Variant(s))**
    - Airplane / 16, 28, 32, 48 / Filled & Regular
    - Arrow Sync Circle / 28, 32, 48 / Filled & Regular
    - Bin Full / 48 / Filled & Regular
	- Calendar Arrow Repeat All / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Clock Toolbox / 32 / Filled & Regular
	- Coin Multiple / 28, 32, 48 / Filled & Regular
    - Database Search / 32 / Filled
    - Database Search / 32 / Regular
    - Document Globe / 20, 24 / Filled & Regular
    - Form Sparkle / 20 / Filled & Regular
    - Globe Off / 12, 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Hat Graduation / 32, 48 / Filled & Regular
    - Line Style Sketch / 16, 20, 24, 28, 32 / Filled & Regular
    - Microscope / 32 / Filled & Regular
    - Person Board Add / 16, 24, 28, 32 / Filled & Regular
    - Puzzle Piece / 20, 28, 32, 48 / Filled & Regular
    - Reward / 32 / Filled & Regular
    - Shopping Bag / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Shopping Bag Tag / 16, 28, 32, 48 / Filled & Regular
    - Tab Add / 32 / Filled, Light & Regular
    - Teaching / 16, 24, 28, 32, 48 / Filled & Regular
    - Window Brush / 20, 24 / Filled & Regular
    - Window Column One Fourth Left / 20 / Filled & Regular
    - Window Column One Fourth Left Focus Left / 20 / Filled
    - Window Column One Fourth Left Focus Top / 20 / Filled

 
    **What's updated (Name / Size(s) / Variant(s))**
    - Arrow Sync Circle / 24/ Filled & Regular
    - Building People / 16, 20, 24 / Filled & Regular          
    - Chat Bubbles Question / 20 / Filled & Regular
    - Circle Half Fill / 20, 24 / Filled & Regular
    - Clipboard Text Edit / 32 / Filled & Regular
    - Contact Card Link / 16, 20 / Filled & Regular
    - Folder People / 20, 24 / Filled & Regular
    - Puzzle Piece / 20 / Filled & Regular
    - Run / 16, 20, 24 / Filled & Regular
    - Shopping Bag / 16, 20, 24 / Filled

## Before v4.10.0
For versions before 4.10, see the [What's New? (v4.0 - v4.9)](/WhatsNew-Before410) page.

## Archives
For versions before 4.0, see [What's new? (archives)](/WhatsNew-Archive) page.
