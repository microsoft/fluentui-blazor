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
