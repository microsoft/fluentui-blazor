## V3.2.2
- Fix [#859](https://github.com/microsoft/fluentui-blazor/issues/859) and [#884](https://github.com/microsoft/fluentui-blazor/issues/859): Fix NavMenu for real by moving `<a>` tag up in the rendering tree

### Update Fluent UI System icons to 1.1.221
**What's new (Name / Size(s) / Variant(s))**
- Arrow Download Off / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Border Inside / 16, 20, 24 / Filled & Regular
- Chat Lock / 16, 20, 24, 28 / Filled & Regular
- Error Circle / 48 / Filled & Regular
- Full Screen Maximize / 28, 32 / Filled & Regular
- Full Screen Minimize / 28, 32 / Filled & Regular
- Link Person / 16, 20, 24, 32, 48 / Filled & Regular
- People Chat / 16, 20, 24 / Filled & Regular
- Person Support / 28 / Filled & Regular
- Shapes / 32 / Filled & Regular
- Slide Text Edit / 16, 20, 24, 28 / Filled & Regular
- Subtract Circle / 48 / Filled & Regular
- Subtract Parentheses / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Warning / 48 / Filled & Regular

**What's updated (Name / Size(s) / Variant(s))**
- Border None / 16 / Filled & Regular
- Flag Off / 48 / Filled & Regular
- Person Support / 16, 20, 24 / Filled & Regular
- Shapes / 28 / Filled & Regular
- Subtract Circle / 16, 32 / Filled & Regular

## V3.2.1
- Design Tokens WithDefault method implemented to allow setting a default value for a token. This is technically a breaking chage, but it is unlikely to affect anyone as the previous implementation did not do anything. See the `SiteSettingsPanel.razor.cs` in the demo site for an implementation example.
- Fix [#872](https://github.com/microsoft/fluentui-blazor/issues/872): A11y issue in FluentDivider
- Fix [#864](https://github.com/microsoft/fluentui-blazor/issues/864): A11y issue in FluentAutoComplete
- Fix [#861](https://github.com/microsoft/fluentui-blazor/issues/861): A11y issue in FluentDataGrid EmptyContent
- Fix [#848](https://github.com/microsoft/fluentui-blazor/issues/848): A11y issue in FluentDialogHeader
- Fix [#847](https://github.com/microsoft/fluentui-blazor/issues/847): A11y issues in FluentNavMenu
- Fix [#861](https://github.com/microsoft/fluentui-blazor/issues/861): A11y issue in FluentDataGrid EmptyContent
- Fix [#859](https://github.com/microsoft/fluentui-blazor/issues/859): Fix FluentNavMenuGroup not clickable ouside of `<a>` tag
- Fix [#857](https://github.com/microsoft/fluentui-blazor/issues/857): Fix copy Emoji code in search page
- Fix [#841](https://github.com/microsoft/fluentui-blazor/issues/841): Overflow causes loss of background with Panel
- Fix [#833](https://github.com/microsoft/fluentui-blazor/issues/833): Use correct abbreviation for day name in FluentCalendar (Chinese)
- Miscelaneous documentation and typo fixes
- Add more unit tests
- Update to FluentUI System Icons 1.1.220

- **What's new (Name / Size(s) / Variant(s))**
- Arrow Download / 28, 32 / Filled & Regular
- Arrow Expand / 16 / Filled & Regular
- Arrow Export Up / 16 / Filled & Regular
- Arrow Import / 16 / Filled & Regular
- Arrow Up Right Dashes / 16 / Filled & Regular
- Battery 10 / 16 / Filled & Regular
- Beaker Empty / 16 / Filled & Regular
- Book / 16 / Filled & Regular
- Border None / 16 / Filled & Regular
- Branch Request / 16 / Filled & Regular
- Clipboard Task List / 16 / Filled & Regular
- Cut / 16 / Filled & Regular
- Folder Search / 16, 20, 24 / Filled & Regular
- Hexagon / 28, 32, 48 / Filled & Regular
- Plug Connected / 16 / Filled & Regular
- Plug Disconnected / 16 / Filled & Regular
- Projection Screen Text / 20 / Filled & Regular
- RSS / 16 / Filled & Regular
- Shape Organic / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Teardrop Bottom Right / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Text Asterisk / 16 / Filled & Regular
- Text Edit Style / 16 / Filled & Regular
- Text Whole Word / 16 / Filled & Regular
- Triangle / 24, 28 / Filled & Regular
  
**What's updated (Name / Size(s) / Variant(s))**
- Arrow Bidirectional Left Right / 16 / Filled & Regular
- Arrow Download / 24, 48 / Filled & Regular
- Beaker Edit / 20 / Filled & Regular
- Beaker Off / 20 / Filled & Regular
- Beaker Settings / 20 / Filled & Regular
- Clipboard Letter / 24 / Filled & Regular
- Prohibited / 16, 24, 28, 48 / Filled & Regular 
 

## V3.2.0
- New NavMenu, NavGroup and NavLink components. **Breaking change** - See the [Upgrade guide](https://www.fluentui-blazor.net/UpgradeGuide) for details. See [NavMenu](https://www.fluentui-blazor.net/NavMenu) page for examples.
- New FluentInputLabel component.
- FluentCard: Add AreaRestricted parameter to allow content to break out of card area.
- Provide error message when FluentDialogProvider missing
- It is now possible to add a tooltip to DataGridColumns  

Fix [#796](https://github.com/microsoft/fluentui-blazor/pull/796): Fix IconColor doc page
Fix [#797](https://github.com/microsoft/fluentui-blazor/pull/797): Fix MessageBar issues
Fix [#805](https://github.com/microsoft/fluentui-blazor/pull/805): InlineStyleBuilder on .NET6
Fix [#810](https://github.com/microsoft/fluentui-blazor/pull/810): FluentDataGrid error if page is quickly exited
Fix [#815](https://github.com/microsoft/fluentui-blazor/pull/815): Manual upload on iOS
Fix [#828](https://github.com/microsoft/fluentui-blazor/pull/828): FluentSelect: Fix ValueChanged fired twice
Fix [#801](https://github.com/microsoft/fluentui-blazor/pull/801): Remove direct call to Items.Count()
Fix [#834](https://github.com/microsoft/fluentui-blazor/pull/834): Chinese abbreviated day name in FluentCalendar
Fix [#836](https://github.com/microsoft/fluentui-blazor/pull/836): Setting SelectedOptions does not update FluentSelct

## V3.1.1
- Fix [#776](https://github.com/microsoft/fluentui-blazor/issues/776): Icon throws exception when deployed to Azure
- Fix [#755](https://github.com/microsoft/fluentui-blazor/issues/755): Icon throws exception when deployed to Azure
- Fix [#789](https://github.com/microsoft/fluentui-blazor/issues/789): Navigation to "/" crashes with FluentNavMenu
- Fix [#780](https://github.com/microsoft/fluentui-blazor/issues/780): 'OK' button rendered outside of panel for Site settings
Also addresses some other padding and layout issues with FluentDialog variants introduced in 3.1.0
- Feature [#782](https://github.com/microsoft/fluentui-blazor/issues/782) Add ability to prevent dismissing a modal dialog by clicking on the overlay
- Added `Tooltip` and `Style` parameters to DataGrid's `PropertyColumn` and `TemplateColumn`. See [DataGrid](https://www.fluentui-blazor.net) page for examples.
- Add Icon.WithColor and update the Demo page
- Update Button Icon Color depending of Button Appearance
- Update the way to apply the Icon.Color in a Button
 
## V3.1.

### New components
* `FluentAutoComplete`
* `FluentPersona`
* `FluentMessageBar`

### What's Changed

#### Components
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

#### General
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

#### Templates
* Update templates by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/688
* Add missing usings Project template by @agriffard in https://github.com/microsoft/fluentui-blazor/pull/702

#### New Contributors
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

## Archives
See the [What's New](https://www.fluentui-blazor.net/WhatsNew) page on the documentation online to browse the archive