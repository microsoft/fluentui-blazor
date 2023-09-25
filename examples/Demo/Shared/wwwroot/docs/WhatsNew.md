## V3.1.1
- Fix [#776](https://github.com/microsoft/fluentui-blazor/issues/776): Icon throws exception when deployed to Azure
- Fix [#780](https://github.com/microsoft/fluentui-blazor/issues/780): 'OK' button rendered outside of panel for Site settings
Also addresses some other issues with FluentDialog variants introduced in 3.1.0
- Fix [#755](https://github.com/microsoft/fluentui-blazor/issues/755): Icon throws exception when deployed to Azure
- Feature [#782](https://github.com/microsoft/fluentui-blazor/issues/782) Add ability to prevent dismissing a modal dialog by clicking on the overlay

## V3.1.

## New components
* `FluentAutoComplete`
* `FluentPersona`
* `FluentMessageBar`

## What's Changed

### Components
* [FluentButton] Update the Button.Loading when using with a Icon by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/752
* [FluentCodeEditor] update docs by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/692
* [FluentCombobox, FluentSelect, FluentListbox] Add Width and Height property and associated styles by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/663
* [FluentDataGrid] Add `EmptyContent` parameter by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/731
* [FluentDataGrid] multiline-text word-break by @ssccinng in https://github.com/microsoft/fluentui-blazor/pull/711
* [FluentDataGridCell, FluentDataGridRow] Add item reference (#695) by @hksalessio in https://github.com/microsoft/fluentui-blazor/pull/700
* [FluentDataGridRow] Update for sticky header and example by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/747
* [FluentDatePicker and FluentTimePicker] Keep existing time/date by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/726
* [FluentDatePicker] Update the popup position by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/724
* [FluentDialog] Dialog enhancements (add dialog type, fix panel width setable), 
* [FluentDialog] Enhancements by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/717
* [FluentDialog] Include components to customize the Dialog-box by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/764
* [FluentDialog] Make all DialogService.Show..Async methods return IDialogReference by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/694
* [FluentMenu/FluentPopover/FluentOverlay] Close menu/popover automatically when user clicks outside of it (by using overlay) by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/729
* [FluentNavMenu] Fix route/subroute not selecting correct item by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/755
* [FluentOptionPeople] Add ChildContent by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/728
* [FluentOverflow] Fix the Tooltip used by the FluentOverflow by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/678
* [FluentPresenceBadge  & FluentOptionPeople] - Update the Status badge to be compliant FluentUI 2 by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/732
* [FluentProgressToast] Changes by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/657
* [FluentTreeItem] Fix select area when Text property is used by @ProH4Ck in https://github.com/microsoft/fluentui-blazor/pull/667
* [Multiple] Add `PreventScroll` to Dialog and Overlay by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/758
* [Multiple] Apply the Immediate property to "text" components by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/696
* [Multiple] Set TextField.AutoComplete property to string type by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/682

### General
* [CssBuilder and StyleBuilder] Fixing the built format and position of the custom styles by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/745
* [Documentation] Add UnitTests markdown page by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/658
* [Documentation] Update the Upgrade Guide and details on Icons by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/673
* Add MaxHeight to DemoSection by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/740
* Fix #621  by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/683
* Fix #674 by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/676
* Fix Icons and Emojis trimmed during the WASM publishing by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/666
* fix typos by @LuohuaRain in https://github.com/microsoft/fluentui-blazor/pull/751
* Re-add scripts to index.html/_Layout.cshtml, Update docs by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/697
* Rewrite the Icons.AllIcons property using relfection by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/685
* Update CodeSetup.md by @LuohuaRain in https://github.com/microsoft/fluentui-blazor/pull/730
* Update Fluent UI System Icons to 1.1.217  by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/765

### Templates
* Update templates by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/688
* Add missing usings Project template by @agriffard in https://github.com/microsoft/fluentui-blazor/pull/702

## New Contributors
* @agriffard made their first contribution in https://github.com/microsoft/fluentui-blazor/pull/702
* @hksalessio made their first contribution in https://github.com/microsoft/fluentui-blazor/pull/700
* @ssccinng made their first contribution in https://github.com/microsoft/fluentui-blazor/pull/711

Thanks to all contributors!

## V3.0.1
- Fix [#643](https://github.com/microsoft/fluentui-blazor/issues/643)**: Visual Studio 2022 freezes and then crashes with v3-RC-1
- Fix [#684](https://github.com/microsoft/fluentui-blazor/pull/684): Make FluentToastContainer work better on mobile devices
- Fix [#621](https://github.com/microsoft/fluentui-blazor/issues/621): Add area-hidden to FluentSearch clear button
- Fix [#674](https://github.com/microsoft/fluentui-blazor/issues/674): Collapse FluentNavMenuGroup on click if no custom action/href is assigned
- Fix [#680](https://github.com/microsoft/fluentui-blazor/issues/680): FluentTextField AutoComplete should be a string or enum and not a boolean 
- Fix [#668](https://github.com/microsoft/fluentui-blazor/issues/668): v3.0.0 IIS debugging/deployments 'Failed to load resource'
- Fix [#677](https://github.com/microsoft/fluentui-blazor/issues/677): FluentOverflow not working correctly
- Fix [#667](https://github.com/microsoft/fluentui-blazor/issues/667): FluentTreeItem select area when Text property is used
- Fix [#660](https://github.com/microsoft/fluentui-blazor/issues/660): Allow FluentMessageBox Text to not show an Icon
- Fix [#624](https://github.com/microsoft/fluentui-blazor/issues/624): v3 Unhandled exception rendering in NavMenuContent of FluentMainLayout
- Fix [#655](https://github.com/microsoft/fluentui-blazor/pull/655): ProgressToast rendering error for WASM

**) To fix this issue update Microsoft.Fast.Components.FluentUI.Icons v3.0.1 

### Other changes
- FluentCombobox, FluentSelect and FluentListbox now support Width and Height properties
- Update several documentation pages about how to use Icons and Emoji
- Included the script needed to implement Blazor Hybrid workaround easier (see readme)

## V3.0.0
** 26 New components**
- Header
- Footer
- BodyContent
- Grid
- Layout
- MainLayout
- Spacer
- Splitter
- Stack
- CounterBadge
- PresenceBadge
- CodeEditor
- Date & Time (Calendar, DatePicker, TimePicker)
- Dialog
- DragDrop
- Highlighter
- Label
- MessageBox
- Overflow
- Overlay
- Panel
- Popover
- SplashScreen
- Toast


**Completely redone components (and configuration)**
- Icon
- Emoji

**Other changes**
- Added initializersLoader.webview.js to work around Blazor Hybrid bug (see readme)

For older changes see the [What's New archive](WhatsNew-Archive.md)