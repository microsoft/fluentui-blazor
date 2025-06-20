## V4.12.1

## V4.12.0

### General
- \[General\] Use latest .NET SDKs (including .NET 10 preview 4)([#3789](https://github.com/microsoft/fluentui-blazor/pull/3789))

### Components
- \[AutoComplete\] Do not show previous/next when no option is selected ([#3780](https://github.com/microsoft/fluentui-blazor/pull/3780))
- \[Autocomplete\] Fix active highlight size for single-select mode ([#3819](https://github.com/microsoft/fluentui-blazor/pull/3819))
- \[AutoComplete\] Fix using different height for MaxAutoHeight (#3793) ([#3796](https://github.com/microsoft/fluentui-blazor/pull/3796))
- \[DataGrid\] Add UseMenuService parameter to override menu behavior ([#3862](https://github.com/microsoft/fluentui-blazor/pull/3862))
- \[DataGrid\] Make Select/Deselect all more robust ([#3866](https://github.com/microsoft/fluentui-blazor/pull/3866))
- \[DataGrid\] Refine logic that determines if the DataGrid data needs to be reloaded ([#3864](https://github.com/microsoft/fluentui-blazor/pull/3864))
- \[DataGrid\] Resize enhancements ([#3767](https://github.com/microsoft/fluentui-blazor/pull/3767))
- \[DataGrid\] Tweak new resize behavior ([#3787](https://github.com/microsoft/fluentui-blazor/pull/3787))
- \[DataGrid\] Use ColumnOptionsLabels.Icon in more places ([#3865](https://github.com/microsoft/fluentui-blazor/pull/3865))
- \[DataGrid\] Add `HeaderCellTitleTemplate` to `ColumnBase` ([#3860](https://github.com/microsoft/fluentui-blazor/pull/3860))
- \[DesignTheme\] Store selected color in GlobalState ([#3833](https://github.com/microsoft/fluentui-blazor/pull/3833))
- \[Lists\] Do not set internal value when using multiple ([#3835](https://github.com/microsoft/fluentui-blazor/pull/3835))
- \[MainLayout\] Remove double header reduction ([#3841](https://github.com/microsoft/fluentui-blazor/pull/3841))
- \[MenuButton\] Pass through AdditionalAttributes ([#3798](https://github.com/microsoft/fluentui-blazor/pull/3798))
- \[MenuProvider\] Pass through class and style from menu's. ([#3809](https://github.com/microsoft/fluentui-blazor/pull/3809))
- \[MultiSplitter\] Remove position from panel CSS ([#3870](https://github.com/microsoft/fluentui-blazor/pull/3870))
- \[SortableList\] Extend the CSS class with more predefined variables([#3877](https://github.com/microsoft/fluentui-blazor/pull/3877))
- \[Tabs\] Make initialization logic more robust ([#3878](https://github.com/microsoft/fluentui-blazor/pull/3878))

### Demo site and documentation
- \[Docs\] Add docs about `FluentEditForm` ([#3832](https://github.com/microsoft/fluentui-blazor/pull/3832))
- \[Docs\] Fix typo in InputFile documentation ([#3769](https://github.com/microsoft/fluentui-blazor/pull/3769))
- \[Docs\] Fix typo in Card documentation ([#3839](https://github.com/microsoft/fluentui-blazor/pull/3839))
- \[Docs\] Remove Autofocus to prevent scrolling in the docs (#3826) ([#3828](https://github.com/microsoft/fluentui-blazor/pull/3828))
- \[Docs\] Update reboot section for IIS hosting workaround ([#3788](https://github.com/microsoft/fluentui-blazor/pull/3788))

### Icons and Emoji
  - Update to Fluent UI System Icons 1.1.302.
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.

## V4.11.9  

### General  
- \[General\] A11y - Fix certain controls not adapting text spacing ([#3706](https://github.com/microsoft/fluentui-blazor/pull/3706))  
- \[General\] Add missing try..catch in DisposeAsync ([#3699](https://github.com/microsoft/fluentui-blazor/pull/3699))  
- \[General\] Remove the FluentAssertions dependency ([#3701](https://github.com/microsoft/fluentui-blazor/pull/3701))  
- \[General\] Use newer Markdig package ([#3703](https://github.com/microsoft/fluentui-blazor/pull/3703))  

### Components
- \[AutoComplete\] Introduce Position parameter. ([#3718](https://github.com/microsoft/fluentui-blazor/pull/3718))  
- \[AutoComplete\] Allow the ability to trigger the search options via code. ([#3570](https://github.com/microsoft/fluentui-blazor/pull/3570))  
- \[AutoComplete\] Close the dropdown via code ([#3715](https://github.com/microsoft/fluentui-blazor/pull/3715))  
- \[AutoComplete\] Enable "Multiple = false" when selecting a single item is desired ([#3571](https://github.com/microsoft/fluentui-blazor/pull/3571))  
- \[Combobox\] Fix GetOptionValue ([#3739](https://github.com/microsoft/fluentui-blazor/pull/3739))  
- \[DataGrid\] Do not handle keypress if active element is text field ([#3749](https://github.com/microsoft/fluentui-blazor/pull/3749))  
- \[DataGrid\] Do not use 'display: flex' on table header cells ([#3760](https://github.com/microsoft/fluentui-blazor/pull/3760))  
- \[InputFile\] a11y - Fix missing focus indicator in FluentInputFile ([#3722](https://github.com/microsoft/fluentui-blazor/pull/3722))  
- \[InputFile\] Fix DivideByZeroException in FluentInputFile when uploading 0 byte files ([#3719](https://github.com/microsoft/fluentui-blazor/pull/3719))  
- \[Menu\] Account for screen width and height when positioning menu ([#3682](https://github.com/microsoft/fluentui-blazor/pull/3682))  
- \[ProfileMenu\] Add OpenChanged event ([#3750](https://github.com/microsoft/fluentui-blazor/pull/3750))  
- \[Tabs\] Fix issue with publication/trimming ([#3677](https://github.com/microsoft/fluentui-blazor/pull/3677))  
- \[Templates\] Fix overwriting standard Blazor Hybrid template ([#3673](https://github.com/microsoft/fluentui-blazor/pull/3673))  
- \[Tooltip\] Add AdditionalProperties when using TooltipService ([#3691](https://github.com/microsoft/fluentui-blazor/pull/3691))  
- \[Wizard\] Add the ability to invoke `OnFinish` ([#3648](https://github.com/microsoft/fluentui-blazor/pull/3648))  

### Demo site and documentation
- \[Docs\] Add message for when using reboot with different BasePath ([#3678](https://github.com/microsoft/fluentui-blazor/pull/3678))  
- \[Docs\] Clarify FluentSelect SelectedOptionsChanged not triggered with manual options ([#3680](https://github.com/microsoft/fluentui-blazor/pull/3680))  
- \[Docs\] Fix missing `DisplayMode` attribute in `DataGridRemoteData.razor` ([#3729](https://github.com/microsoft/fluentui-blazor/pull/3729))  
- \[Docs\] Try to use more correct terminology ([#3705](https://github.com/microsoft/fluentui-blazor/pull/3705))  
- \[Docs\] Update InputFile docs to explain usage of different InputFileModes ([#3757](https://github.com/microsoft/fluentui-blazor/pull/3757))

### Icons and Emoji
  - Update to Fluent UI System Icons 1.1.298.
    > As of this version we will no longer highlight individual icon additions/changes. You can find the full list of changes in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/).

## V4.11.8

### General
- \[General\] Add .NET 10 Preview ~~2~~ 3 support ([#3641](https://github.com/microsoft/fluentui-blazor/pull/3641))
- \[General\] Fix trimming error ([#3578](https://github.com/microsoft/fluentui-blazor/pull/3578))  

### Components
- \[Combobox\] Fix issue when used in Dialog ([#3603](https://github.com/microsoft/fluentui-blazor/pull/3603))  
- \[DataGrid\] Add icons to column menus ([#3621](https://github.com/microsoft/fluentui-blazor/pull/3621))  
- \[DataGrid\] Fix rendering issue in Table mode ([#3615](https://github.com/microsoft/fluentui-blazor/pull/3615))  
- \[DataGrid\] Fix Width when `ResizableColumns` and `Sortable` ([#3593](https://github.com/microsoft/fluentui-blazor/pull/3593))  
- \[Icons\] AddIconsExtensions.TryGetInstance ([#3569](https://github.com/microsoft/fluentui-blazor/pull/3569))  
- \[InputFile\] Better handling of dispose error ([#3605](https://github.com/microsoft/fluentui-blazor/pull/3605))  
- \[NavMenu\] Add code to only respond to certain key codes ([#3595](https://github.com/microsoft/fluentui-blazor/pull/3595))  
- \[TreeView\] Add MultiSelect example ([#3602](https://github.com/microsoft/fluentui-blazor/pull/3602))  

### Demo site and documentation
- \[Docs\] Remove an unnecessary line in FluentDialog ([#3550](https://github.com/microsoft/fluentui-blazor/pull/3550))  
- \[Docs\] Make cookie consent responsive ([#3555](https://github.com/microsoft/fluentui-blazor/pull/3555))  
- \[Docs\] Remove extra '>' ([#3572](https://github.com/microsoft/fluentui-blazor/pull/3572))  
- \[Docs\] Fix typo ([#3589](https://github.com/microsoft/fluentui-blazor/pull/3589))  

### Icons and Emoji
- Update to Fluent UI System Icons 1.1.293 (changes since 1.1.292).

    **What's new (Name / Size(s) / Variant(s))**
    - Chat Multiple Checkmark / 16, 20, 24, 28 / Filled & Regular
    - Chat Multiple Minus / 16, 20, 24, 28 / Filled & Regular
    - Circle Multiple Hint Checkmark / 16, 32 / Filled & Regular
    - Code Block Edit / 16, 20, 24 / Filled & Regular
    - Cube Checkmark / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Cube / 28 / Filled & Regular
    - Database Switch / 24 / Filled & Regular
    - Flow Sparkle / 16, 20 / Filled & Regular 
    - Mail Fish Hook / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Shopping Bag Checkmark / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Sparkle Info / 20, 24 / Filled & Regular
    - Table Column Top Bottom Edit / 16, 20, 24, 28 / Filled & Regular
    - Table Column Top Bottom / 16, 28 / Filled & Regular
    - Task List Square Database / 24 / Filled & Regular
    - Vehicle Truck Checkmark / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Vehicle Truck Profile / 28, 32, 48 / Filled & Regular
    - Vote / 16 / Filled & Regular

    **What's updated (Name / Size(s) / Variant(s))**
    - Dismiss Circle / 16 / Filled & Regular

    **What's got color now (Name / Size(s) / Variant(s))**
    - Chat Add / 16, 20, 24, 28, 32, 48 / Color
    - Chat / 28, 32, 48 / Color
    - People Chat / 24 / Color
    - Person Add / 24 / Color
    - Share iOS / 24 / Color 


## V4.11.7

### Components
- \[General\] Enable having multiple paginators ([#3487](https://github.com/microsoft/fluentui-blazor/pull/3487))
- \[CounterBadge\] Add support for additional attributes ([#3542](https://github.com/microsoft/fluentui-blazor/pull/3542))
- \[CounterBadge\] Fix `HorizontalPosition`/`VerticalPosition` not being used ([#3490](https://github.com/microsoft/fluentui-blazor/pull/3490))
- \[DataGrid\] Add method to close column options programmatically ([#3501](https://github.com/microsoft/fluentui-blazor/pull/3501))
- \[DataGrid\] Fix styling when using `Virtualize` with `DataGridDisplayMode.Table` ([#3497](https://github.com/microsoft/fluentui-blazor/pull/3497))
- \[DesignTheme\] Add support for setting/getting NeutralBaseColor ([#3530](https://github.com/microsoft/fluentui-blazor/pull/3530))
- \[DesignTheme\] Fix the accompanying web component ([#3516](https://github.com/microsoft/fluentui-blazor/pull/3516))
- \[DesignTokens\] Make `AccentBaseColor` and `NeutralBaseColor` work with WithDefault ([#3517](https://github.com/microsoft/fluentui-blazor/pull/3517))
- \[NumberField\] Fix invalid class styling ([#3531](https://github.com/microsoft/fluentui-blazor/pull/3531))
- \[Overlay\] Make default background color follow neutral base color ([#3537](https://github.com/microsoft/fluentui-blazor/pull/3537))
- \[Stack\] Add `Reversed` parameter ([#3505](https://github.com/microsoft/fluentui-blazor/pull/3505))

### Demo site and documentation
- \[Docs\] Add cookie consent ([#3507](https://github.com/microsoft/fluentui-blazor/pull/3507))
- \[Docs\] IconExplorer: Reset current page to 0 when searching ([#3536](https://github.com/microsoft/fluentui-blazor/pull/3536))
- \[Docs]\ Typo in Templates.md file. ([#3503](https://github.com/microsoft/fluentui-blazor/pull/3503))


### Icons and Emoji
- Update to Fluent UI System Icons 1.1.292 (changes since 1.1.278).

  We do not have an update on the individual new/changed icons at this time


## V4.11.6

### Components
- \[Autocomplete\] Apply invalid css to autocomplete ([#3417](https://github.com/microsoft/fluentui-blazor/pull/3417))
- \[DataGrid\] Add IGridSort interface, fixes sorting when using ItemsProvider ([#3460](https://github.com/microsoft/fluentui-blazor/pull/3460))
- \[DataGrid\] Fix error in handling GridTemplateColumns ([#3413](https://github.com/microsoft/fluentui-blazor/pull/3413))
- \[DataGrid\] Only add Selectable items when using Select All ([#3453](https://github.com/microsoft/fluentui-blazor/pull/3453))
- \[DataGrid\] Provide alternate way to refresh when working with remote data ([#3423](https://github.com/microsoft/fluentui-blazor/pull/3423))
- \[Search\] FluentSearch fix deletion if Disabled/Readonly ([#3433](https://github.com/microsoft/fluentui-blazor/pull/3433))
- \[InputFile\] Add @attributes ([#3441](https://github.com/microsoft/fluentui-blazor/pull/3441))
- \[Lists\] Fix Form validation happening too late ([#3468](https://github.com/microsoft/fluentui-blazor/pull/3468))
- \[Lists\] Fix Form validation issue ([#3466](https://github.com/microsoft/fluentui-blazor/pull/3466))
- \[Lists\] Do not have parameters only be set once ([#3457](https://github.com/microsoft/fluentui-blazor/pull/3457))
- \[Menu\] Add CheckedChanged EventCallback (fix 3390) ([#3414](https://github.com/microsoft/fluentui-blazor/pull/3414))
- \[Select\] Fix style when Multiple set/not set ([#3442](https://github.com/microsoft/fluentui-blazor/pull/3442))
- \[Toolbar\] Dispose of JS module properly ([#3470](https://github.com/microsoft/fluentui-blazor/pull/3470))

### Demo site and documentation
- \[Docs\] Anchor docs ([#3428](https://github.com/microsoft/fluentui-blazor/pull/3428))

### Icons and Emoji
- Update to Fluent UI System Icons 1.1.278 (changes since 1.1.273)

  **What's new (Name / Size(s) / Variant(s))**
  - Agents / 16, 28, 32, 48 / Filled & Regular
  - Alert Badge / 32 / Filled & Regular
  - Apps List / 32 / Filled & Regular
  - Apps List Detail / 32 / Filled & Regular
  - Arrow Circle Up Sparkle / 20, 24 / Filled & Regular
  - Arrow Counterclockwise Info / 20, 24, 28, 32, 48 / Filled & Regular
  - Arrow Square / 32 / Filled & Regular
  - Branch Request Closed / 16, 20 / Filled & Regular
  - Branch Request Draft / 16, 20 / Filled & Regular
  - Building Home / 32 / Filled & Regular
  - Calendar Clock / 32 / Filled & Regular
  - Call Rectangle Landscape / 16, 20, 24, 28 / Filled & Regular
  - Call Square / 16, 20, 24, 28 / Filled & Regular
  - Crown Subtract / 20 / Filled & Regular
  - Data Area / 20, 24, 32 / Filled & Regular
  - Data Line / 32 / Filled & Regular
  - Data Pie / 20, 24, 32 / Filled & Regular
  - Data Scatter / 20, 24, 32 / Filled & Regular
  - Data Trending / 20, 32 / Filled & Regular
  - Data Usage Sparkle / 20, 24 / Filled & Regular
  - Database Arrow Right / 16 / Filled & Regular
  - Document Arrow Right / 16 / Filled & Regular
  - Info Sparkle / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Lightbulb Checkmark / 24, 32 / Filled & Regular
  - List Bar / 24, 32 / Filled & Regular
  - Mail Clock / 32 / Filled & Regular
  - Mail Data Bar / 16, 20, 24 / Filled & Regular
  - Number Symbol Square / 32 / Filled
  - Number Symbol Square / 32 / Regular
  - People Sync / 24, 32 / Filled & Regular
  - Person Arrow Back / 16 / Filled & Regular
  - Person Heart / 32 / Filled & Regular
  - Person Key / 24, 32 / Filled & Regular
  - Person Sync / 24, 32 / Filled & Regular
  - Person Tentative / 32 / Filled & Regular
  - Ribbon Star / 32 / Filled & Regular
  - Send Clock / 32 / Filled & Regular
  - Skip Back 15 / 20, 24 / Filled & Regular
  - Skip Forward 15 / 20, 24 / Filled & Regular
  - Square Text Arrow Repeat All / 32 / Filled & Regular
  - Star Settings / 32 / Filled & Regular
  - Table Alt Text / 20, 24, 32 / Filled & Light & Regular
  - Table Cell Add / 16, 20, 24 / Filled & Regular
  - Temperature Degree Celsius / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Temperature Degree Fahrenheit / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Text Bullet List Square Sparkle / 32 / Filled & Regular
  - Text Paragraph Direction Left / 24 / Filled & Regular
  - Text Paragraph Direction Right / 24 / Filled & Regular
  - Video Multiple / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Window Text / 16, 24, 28 / Filled & Regular


  **What's updated (Name / Size(s) / Variant(s))**
  - Arrow Forward / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Arrow Reply All / 16, 20, 24, 28, 32, 48 / Filled
  - Chart Multiple / 16, 20, 24, 32 / Filled & Regular
  - Diversity / 16, 20, 24, 28, 48 / Filled & Regular
  - Guest / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Info / 24, 28 / Filled & Regular
  - Lock Closed Ribbon / 16, 20, 24, 28, 48 / Filled & Regular
  - Person Sync / 16, 20, 24, 28, 32, 48 / Filled & Regular

  **What's got color now (Name / Size(s) / Variant(s))**
  - Alert Badge / 16, 20, 24, 32 / Color
  - Alert Urgent / 16, 20, 24 / Color
  - Animal Paw Print / 16, 20, 24, 28, 32, 48 / Color
  - Apps List Detail / 20, 24, 32 / Color
  - Apps List / 20, 24, 32 / Color
  - Arrow Clockwise Dashes Settings / 16, 20, 24, 28, 32, 48 / Color
  - Arrow Clockwise Dashes / 16, 20, 24, 32 / Color
  - Arrow Square / 20, 24, 32 / Color
  - Arrow Sync / 16, 20, 24 / Color
  - Board / 16, 20, 24, 28 / Color
  - Book Database / 16, 20, 24, 32 / Color
  - Book Open / 16, 20, 24, 28, 32, 48 / Color
  - Book Star / 20, 24 / Color
  - Book / 16, 20, 24, 28, 32, 48 / Color
  - Bookmark / 16, 20, 24, 28, 32 / Color
  - Bot Sparkle / 16, 20, 24 / Color
  - Bot / 16, 20, 24 / Color
  - Building Government Search / 16, 20, 24, 32 / Color
  - Building Government / 16, 20, 24, 32 / Color
  - Building Home / 16, 20, 24, 32 / Color
  - Calendar Data Bar / 16, 20, 24, 28 / Color
  - Calendar Edit / 16, 20, 24, 32 / Color
  - Calendar Sync / 16, 20, 24 / Color
  - Certificate / 16, 20, 24, 32 / Color
  - Chart Multiple / 16, 20, 24, 32 / Color
  - Clipboard Task / 16, 20, 24 / Color
  - Clock / 16, 20, 24, 28, 32, 48 / Color
  - Cloud Words / 16, 20, 24, 28, 32, 48 / Color
  - Code / 16, 20, 24 / Color
  - Coin Multiple / 16, 20, 24, 28, 32, 48 / Color
  - Comment Multiple / 16, 20, 24, 28, 32 / Color
  - Comment / 16, 20, 24, 28, 32, 48 / Color
  - Contact Card / 16, 20, 24, 28, 32, 48 / Color
  - Content View / 16, 20, 24, 28, 32 / Color
  - Data Area / 20, 24, 32 / Color
  - Data Line / 20, 24, 32 / Color
  - Data Pie / 20, 24, 32 / Color
  - Data Scatter / 20, 24, 32 / Color
  - Data Trending / 16, 20, 24, 28, 32, 48 / Color
  - Database / 16, 20, 24, 32, 48 / Color
  - Diversity / 16, 20, 24, 28, 48 / Color
  - Document Edit / 16, 20, 24 / Color
  - Document Lock / 32 / Color
  - Document Text / 16, 20, 24, 28, 32, 48 / Color
  - Drafts / 16, 20, 24 / Color
  - Flag / 16, 20, 24, 28, 32, 48 / Color
  - Form / 20, 24, 28, 48 / Color
  - Gauge / 20, 24, 32 / Color
  - Gift Card / 16, 20, 24 / Color
  - Gift / 16, 20, 24 / Color
  - Guest / 16, 20, 24, 28, 32, 48 / Color
  - Heart / 16, 20, 24, 28, 32, 48 / Color
  - Image Off / 20, 24, 28, 32, 48 / Color
  - Image / 16, 20, 24, 28, 32, 48 / Color
  - Laptop / 16, 20, 24, 28, 32, 48 / Color
  - Layer Diagonal Person / 16, 20, 24 / Color
  - Lightbulb Checkmark / 20, 24, 32 / Color
  - Lightbulb / 16, 20, 24, 28, 32, 48 / Color
  - Link Multiple / 16, 20, 24 / Color
  - List Bar / 16, 20, 24, 32 / Color
  - Location Ripple / 16, 20, 24 / Color
  - Lock Closed / 16, 20, 24, 28, 32, 48 / Color
  - Lock Shield / 16, 20, 24, 28, 32, 48 / Color
  - Mail Alert / 16, 20, 24, 28, 32 / Color
  - Mail Clock / 16, 20, 24, 32 / Color
  - Megaphone Loud / 16, 20, 24, 28, 32 / Color
  - Molecule / 16, 20, 24, 28, 32, 48 / Color
  - News / 16, 20, 24, 28 / Color
  - Notebook Question Mark / 20, 24 / Color
  - Notebook / 16, 20, 24, 32 / Color
  - Number Symbol Square / 20, 24, 32 / Color
  - Options / 16, 20, 24, 28, 32, 48 / Color
  - Paint Brush / 16, 20, 24, 28, 32 / Color
  - Patient / 20, 24, 32 / Color
  - People List / 16, 20, 24, 28, 32 / Color
  - People Sync / 16, 20, 24, 28, 32 / Color
  - Person Feedback / 16, 20, 24, 28, 32, 48 / Color
  - Person Heart / 20, 24, 32 / Color
  - Person Key / 20, 24, 32 / Color
  - Person Tentative / 16, 20, 24, 32 / Color
  - Person Warning / 16, 20, 24, 28, 32, 48 / Color
  - Phone Laptop / 16, 20, 24, 32 / Color
  - Phone / 16, 20, 24, 28, 32, 48 / Color
  - Premium / 16, 20, 24, 28, 32 / Color
  - Puzzle Piece / 16, 20, 24, 28, 32, 48 / Color
  - Ribbon Star / 20, 24, 32 / Color
  - Ribbon / 16, 20, 24, 32 / Color
  - Send Clock / 20, 24, 32 / Color
  - Send / 16, 20, 24, 28, 32, 48 / Color
  - Settings / 16, 20, 24, 28, 32, 48 / Color
  - Share Android / 16, 20, 24, 32 / Color
  - Sport / 16, 20, 24 / Color
  - Star Settings / 20, 24, 32 / Color
  - Star / 16, 20, 24, 28, 32, 48 / Color
  - Table / 16, 20, 24, 28, 32, 48 / Color
  - Text Bullet List Square Sparkle / 16, 20, 24, 32 / Color
  - Text Bullet List Square / 16, 20, 24, 28, 32, 48 / Color
  - Toolbox / 16, 20, 24, 28, 32 / Color
  - Trophy / 16, 20, 24, 28, 32, 48 / Color
  - Weather Snowflake / 20, 24, 32, 48 / Color
  - Weather Sunny Low / 20, 24, 48 / Color
  - WiFi Warning / 20, 24 / Color
  - WiFi / 20, 24 / Color
  - Wrench Screwdriver / 20, 24, 32 / Color

## V4.11.5

### General
- \[General] Some miscellaneous changes ([#3353](https://github.com/microsoft/fluentui-blazor/pull/3353))

### Components

- \[Autocomplete] Fix the second unnecessary call ([#3367](https://github.com/microsoft/fluentui-blazor/pull/3367))
- \[Calendar] Add DisabledCheckAllDaysOfMonthYear ([#3351](https://github.com/microsoft/fluentui-blazor/pull/3351))
- \[DataGrid] Fix regression with `GridTemplateColumns` ([#3357](https://github.com/microsoft/fluentui-blazor/pull/3357))
- \[NumberField] Unsigned integer values and fixes ([#3373](https://github.com/microsoft/fluentui-blazor/pull/3373))
- \[Search] Fix non working datalist ([#3354](https://github.com/microsoft/fluentui-blazor/pull/3354))
- \[Select] Fix positioning when multiple is true ([#3380](https://github.com/microsoft/fluentui-blazor/pull/3380))

### Demo site and documentation
- \[Demo] Fix (most) printing issues when trying to print the page ([#3370](https://github.com/microsoft/fluentui-blazor/pull/3370))


## V4.11.4

### General
- Use latest .NET SDKs (8.0.406 / 9.0.200)
- Update NuGet packages
- Set `FluentComponentBase.Element` to a correct value for several elements ([#3222](https://github.com/microsoft/fluentui-blazor/pull/3222))

### Components
- \[Combobox\] Add EnableClickToClose ([#3186](https://github.com/microsoft/fluentui-blazor/pull/3186))
- \[Combobox\] Fix detachIndicatorClickHandler not found ([#3239](https://github.com/microsoft/fluentui-blazor/pull/3239))
- \[Combobox\] Fix loop when changing value ([#3300](https://github.com/microsoft/fluentui-blazor/pull/3300))
- \[DataGrid\] Add AutoItemsPerPage parameter and handling ([#3220](https://github.com/microsoft/fluentui-blazor/pull/3220))
- \[DataGrid\] Allow EntityFrameworkAsyncQueryExecutor configuration ([#3272](https://github.com/microsoft/fluentui-blazor/pull/3272))
- \[DataGrid\] Always re-evaluate GridTemplateColumns after collecting columns ([#3189](https://github.com/microsoft/fluentui-blazor/pull/3189))
- \[DataGrid\] Fix height not being set in specific situation ([#3182](https://github.com/microsoft/fluentui-blazor/pull/3182))
- \[DataGrid\] Fix issues related to Sortable not being set on a column ([#3310](https://github.com/microsoft/fluentui-blazor/pull/3310))
- \[DataGrid\] Fix more header edge cases ([#3316](https://github.com/microsoft/fluentui-blazor/pull/3316))
- \[DataGrid\] Fix popups being displays beneath header cells ([#3221](https://github.com/microsoft/fluentui-blazor/pull/3221))
- \[DataGrid\] Move down column action popup ([#3319](https://github.com/microsoft/fluentui-blazor/pull/3319))
- \[DataGrid\] Not displaying EmptyContent/LoadingContent at correct position when in multiline mode. ([#3188](https://github.com/microsoft/fluentui-blazor/pull/3188))
- \[DataGrid\] Remove padding for first column header ([#3286](https://github.com/microsoft/fluentui-blazor/pull/3286))
- \[DataGrid\] Small optimization to the number of times data needs to be refreshed ([#3265](https://github.com/microsoft/fluentui-blazor/pull/3265))
- \[DesignTokens\] Change generated constructors ([#3229](https://github.com/microsoft/fluentui-blazor/pull/3229))
- \[Dialog\] Add type safe way of working with IDialogContentComponent ([#3339](https://github.com/microsoft/fluentui-blazor/pull/3339))
- \[Dialog\] Fix the Customized Dialog tooltip ([#3241](https://github.com/microsoft/fluentui-blazor/pull/3241))
- \[InputFile\] Add OnFileCountExceeded callback ([#3205](https://github.com/microsoft/fluentui-blazor/pull/3205))
- \[Lists\] Add `ChangeOnEnterOnly` parameter to allow option focus change without selection ([#3263](https://github.com/microsoft/fluentui-blazor/pull/3263))
- \[Lists\] Trigger field changed when in multiple mode ([#3294](https://github.com/microsoft/fluentui-blazor/pull/3294))
- \[Tabs\] Fix missing bold highlighting in Firefox #3276 ([#3280](https://github.com/microsoft/fluentui-blazor/pull/3280))
- \[Tabs\] Fix Overflow getting out of sync with tab collection ([#3284](https://github.com/microsoft/fluentui-blazor/pull/3284))
- \[Tabs\] Use correct enum values when comparing size ([#3326](https://github.com/microsoft/fluentui-blazor/pull/3326))
- \[Toolbar\] Allow arrow keys to move cursor inside input fields ([#3200](https://github.com/microsoft/fluentui-blazor/pull/3200))

### Demo site and documentation
- \[Docs\] Asset Explorer - Change message before search ([#3291](https://github.com/microsoft/fluentui-blazor/pull/3291))
- \[Docs\] Some site changes ([#3236](https://github.com/microsoft/fluentui-blazor/pull/3236))

### Icons and Emoji
- Update to Fluent UI System Icons 1.1.273 (changes since 1.1.270)

  **What's new (Name / Size(s) / Variant(s))**
  - Design Ideas / 16, 20, 24, 28, 32, 48 / Color
  - Desk Multiple / 20, 24 / Filled & Regular
  - Fast Forward Circle / 24 / Color
  - Hand Multiple / 16, 20, 24, 28 / Filled & Regular
  - Image Add / 28, 32, 48 / Filled & Regular
  - Lightbulb Filament / 16, 20, 24, 28, 32, 48 / Color
  - Play Circle Hint Half / 20, 24 / Filled & Regular
  - Shield Arrow Right / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Slide Text Sparkle / 16, 20, 24, 28, 32, 48 / Color
  - Text List Abc Lowercase LTR / 20, 24 / Filled & Regular
  - Text List Abc Uppercase LTR / 20, 24 / Filled & Regular
  - Text List Roman Numeral Lowercase / 20, 24 / Filled & Regular
  - Text List Roman Numeral Uppercase / 20, 24 / Filled & Regular


  **What's updated (Name / Size(s) / Variant(s))**
  - Diversity / 16, 20, 24, 28, 48 / Filled & Regular
  - Guest Add / 20, 24 / Filled & Regular
  - Guest / 12, 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Image Add / 24 / Filled & Regular


## V4.11.3

### Components
- \[DataGrid\] Fix cell height issue and make header really sticky ([#3173](https://github.com/microsoft/fluentui-blazor/pull/3173))
- \[Tabs\] Remove height compensation ([#3149](https://github.com/microsoft/fluentui-blazor/pull/3149))

## V4.11.2

### Components
- \[DataGrid\] Add SingleSticky selection mode ([#3150](https://github.com/microsoft/fluentui-blazor/pull/3150))
- \[DataGrid\] Tweak `display: flex` and DataGridDisplayMode.Table ([#3156](https://github.com/microsoft/fluentui-blazor/pull/3156))
- \[DataGrid\] Make Empty/Loading row not respond to hover and `OnRowClick` ([#3166](https://github.com/microsoft/fluentui-blazor/pull/3166))

## V4.11.1

### Components
- \[Accordion\] Fix `Expanded` state not being set in `OnAccordionItemChange` ([#3092](https://github.com/microsoft/fluentui-blazor/pull/3092))
- \[Anchor\] Do not apply inline margin for hypertext by default ([#3131](https://github.com/microsoft/fluentui-blazor/pull/3131))
- \[DataGrid\] Header needs `display: flex` in certain scenarios, multiline-text class must not be added to header ([#3118](https://github.com/microsoft/fluentui-blazor/pull/3118))
- \[DataGrid\] Fix issue when scrolling horizontally and `Virtualize="true"` ([#3117](https://github.com/microsoft/fluentui-blazor/pull/3117))
- \[DataGrid\] Fix script being to eager on processing arrow keys ([#3091](https://github.com/microsoft/fluentui-blazor/pull/3091))
- \[DataGrid\] Make combination of `ResizableColumns` and `AutoFit` work ([#3098](https://github.com/microsoft/fluentui-blazor/pull/3098))
- \[DataGrid\] Restore `OnRowFocus` and `OnCellFocus` ([#3097](https://github.com/microsoft/fluentui-blazor/pull/3097))
- \[DataGrid\] Smoother resizing ([#3072](https://github.com/microsoft/fluentui-blazor/pull/3072))
- \[Dialog\] Add event before closing panel to allow validation ([#2614](https://github.com/microsoft/fluentui-blazor/pull/2614))
- \[DialogHeader\] Allow dialog title exclusion from tab index ([#3137](https://github.com/microsoft/fluentui-blazor/pull/3137))
- \[FluentSlider\] \[FluentNumberField\] Fix #2948 ([#3077](https://github.com/microsoft/fluentui-blazor/pull/3077))
- \[ListComponentBase\] Invoke `ValueChanged` when initially selecting an option ([#3105](https://github.com/microsoft/fluentui-blazor/pull/3105))
- \[ListComponentBase\] Fix `SelectedOptionsChanged` being Invoked twice ([#3119](https://github.com/microsoft/fluentui-blazor/pull/3119))
- \[Rating\] Add reset by keyboard option ([#3073](https://github.com/microsoft/fluentui-blazor/pull/3073))
- \[Stack\] Add `SpaceBetween` and `Stretch` to horizontal and vertical alignment options ([#3143](https://github.com/microsoft/fluentui-blazor/pull/3143))
- \[Templates\] Fix `[StreamRender]` error occurring in certain situations ([#3090](https://github.com/microsoft/fluentui-blazor/pull/3090))

### Demo site and documentation
- \[Docs\] Update DataGridMultiSelect blockquote sample ([#3139](https://github.com/microsoft/fluentui-blazor/pull/3139))

 
## V4.11.0

### Breaking changes and important notes
- The `DataGrid` **now uses HTML table based rendering**. A lot of changes have been made to the structure of the rendered content and the class names used. If you have been overriding grid classes, these probably no longer work and need to be changed in your own code. For more information, please see the DataGrid documentation page.
- As of version 4.11.0 of our Icons and Emoji packages, we are packaging each icon variant (filled, regular, etc.) and emoji category (animals, food, etc.) in its own assembly. **If you are updating to this version, you will need to make some minor updates to your code**. Please see the 'Icons and Emoji' section below.

### General
- \[General\] Ability to change service lifetime ([#2991](https://github.com/microsoft/fluentui-blazor/pull/2991))
- \[Unit Tests\] Fix the CodeCoverage script, using .NET9 only ([#3047](https://github.com/microsoft/fluentui-blazor/pull/3047))

### Components
- \[Autocomplete\] Fix the internal array when SelectedOptions is empty ([#2945](https://github.com/microsoft/fluentui-blazor/pull/2945))
- \[Button\] Fix showing hover on disabled button ([#2968](https://github.com/microsoft/fluentui-blazor/pull/2968))
- \[DataGrid\] Add OData Adaptor package ([#2938](https://github.com/microsoft/fluentui-blazor/pull/2938))
- \[DataGrid\] Alter rendering to use table elements ([#2664](https://github.com/microsoft/fluentui-blazor/pull/2664))
- \[DataGrid\] Page reload issue when using `SaveStateInUrl` ([#2987](https://github.com/microsoft/fluentui-blazor/pull/2987))
- \[DataGrid\] Respecting control state of Loading parameter ([#3064](https://github.com/microsoft/fluentui-blazor/pull/3064))
- \[DataGrid\] Save paging state in URL ([#2972](https://github.com/microsoft/fluentui-blazor/pull/2972))
- \[DatePicker\] Add `PopupHorizontalPosition` property ([#3001](https://github.com/microsoft/fluentui-blazor/pull/3001))
- \[DesignTheme\] DesignTheme no console error if no storage defined ([#2956](https://github.com/microsoft/fluentui-blazor/pull/2956))
- \[InputFile\] \[Docs\] Add null check on initialization ([#3062](https://github.com/microsoft/fluentui-blazor/pull/3062))
- \[InputFile\] Make it work icw a loading Button ([#2940](https://github.com/microsoft/fluentui-blazor/pull/2940))
- \[InputFile\] Set last modified date ([#3044](https://github.com/microsoft/fluentui-blazor/pull/3044))
- \[MessageBox\] Allow HTML markup in message ([#3010](https://github.com/microsoft/fluentui-blazor/pull/3010))
- \[Popover\] Go to the next focusable Element ([#3013](https://github.com/microsoft/fluentui-blazor/pull/3013))
- \[Rating\] Improve a11y support ([#2978](https://github.com/microsoft/fluentui-blazor/pull/2978))
- \[Tabs\] Only render Label if Header is null ([#2989](https://github.com/microsoft/fluentui-blazor/pull/2989))
- \[Templates\] Add new and update existing templates ([#2961](https://github.com/microsoft/fluentui-blazor/pull/2961))
- \[TextField\] Add support for additional field types ([#2971](https://github.com/microsoft/fluentui-blazor/pull/2971))
- \[Wizard\] Fix validation within GoToNextStepAsync method ([#2944](https://github.com/microsoft/fluentui-blazor/pull/2944))
- \[Wizard\] Unregister EditForms in FluentWizardStep when DeferredLoading is enabled ([#3065](https://github.com/microsoft/fluentui-blazor/pull/3065))

### Demo site and documentation
- \[Docs\] Add .NET Conf 2024 video ([#2984](https://github.com/microsoft/fluentui-blazor/pull/2984))
- \[Docs\] Add an animation of the Fluent UI Blazor logo ([#2985](https://github.com/microsoft/fluentui-blazor/pull/2985))
- \[Docs\] Add clarification for when Select is used with `Multiple="true"` ([#2997](https://github.com/microsoft/fluentui-blazor/pull/2997))
- \[Docs\] Embedding skeleton-test-pattern.svg file ([#2983](https://github.com/microsoft/fluentui-blazor/pull/2983))
- \[Docs\] Fix TreeView example ([#3035](https://github.com/microsoft/fluentui-blazor/pull/3035))

### Icons and Emoji
As of version 4.11.0 of our Icons and Emoji packages, we are packaging each icon variant (filled, regular, etc.) and emoji category (animals, food, etc.) in its own assembly. This became necessary due to the large number of icons and some compiler limitations. Nothing has changed on how the packages need to be installed or added to your project.

There is a just a small change needed to your imports file (or using statements).

To use the new versions in your (upgraded) projects, you need add the following @using statement to your _Imports.razor file:
Starting with v4.11.0 you need to add the following `@using` statement to your `_Imports.razor` file:
```razor
@using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;
@* add line below only if you are using the Emoji package *@
@using Emoji = Microsoft.FluentUI.AspNetCore.Components.Emoji
```

- Update to Fluent UI System Icons 1.1.270 (changes since 1.1.265)

  **What's new (Name / Size(s) / Variant(s))**
  - Animal Paw Print / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Arrow Clockwise Dashes / 28, 48 / Filled & Regular
  - Arrow Clockwise Dashes Settings / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Bot / 16, 28, 32, 48 / Filled & Regular
  - Bot Add / 16, 28, 32, 48 / Filled & Regular
  - Bot Sparkle / 16, 28, 32, 48 / Filled & Regular
  - Brain / 20, 24 / Filled & Regular
  - Brain Sparkle / 20 / Filled & Regular
  - Chat History / 20, 24, 28 / Filled & Regular
  - Chat Off / 16 / Filled & Regular
  - Circle Hint / 28, 32, 48 / Filled & Regular
  - Circle Hint Cursor / 16, 20, 24 / Filled & Regular
  - Circle Hint Dismiss / 16, 20, 24 / Filled & Regular
  - Circle Multiple Concentric / 16, 20, 24 / Filled & Regular
  - Circle Multiple Hint Checkmark / 20, 24, 28 / Filled & Regular
  - Circle Sparkle / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Connected / 24, 32 / Filled & Regular
  - Copy / 28 / Filled & Regular
  - Database Checkmark / 16, 20, 24 / Filled & Regular
  - Directions / 28, 32, 48 / Filled & Regular
  - Document / 24, 28, 32, 48 / Light
  - Document One Page Multiple / 16, 20, 24 / Filled & Regular
  - Document One Page Multiple Sparkle / 16, 20, 24 / Filled & Regular
  - Document Sparkle / 16, 20, 24, 28, 32, 48 / Filled & Light & Regular
  - Folder Open / 28 / Filled & Regular
  - Folder Open Down / 16, 20, 24, 28 / Filled & Regular
  - HD Off / 16, 20, 24 / Filled & Regular
  - Home Empty / 20, 24, 28 / Filled & Regular
  - Lightbulb / 32 / Light
  - Location Checkmark / 12, 16, 20, 24, 48 / Filled & Regular
  - Mail Inbox / 48 / Filled & Regular
  - Mail Inbox Person / 48 / Filled & Regular
  - Mail Read Briefcase / 20, 24 / Filled & Regular
  - Paint Bucket Brush / 16, 20, 24, 28 / Filled & Regular
  - Re Order Vertical / 16, 20, 24 / Filled & Regular
  - Row Child / 16, 20, 24, 28, 32 / Filled & Regular
  - Savings / 32 / Filled & Regular
  - Share Screen Start / 16 / Filled & Regular
  - Square Text Arrow Repeat All / 16, 20, 24 / Filled & Regular
  - Translate / 32 / Filled & Regular
  - Weather Moon / 32 / Filled & Light & Regular
  - Weather Sunny / 32 / Light

  **What's updated (Name / Size(s) / Variant(s))**
  - Bot Add / 24 / Filled & Regular
  - Brain Circuit / 20, 24 / Filled & Regular
  - Checkmark Circle / 16 / Filled & Regular
  - Checkmark Circle Warning / 16 / Filled & Regular
  - Checkmark Lock / 16 / Filled & Regular
  - Circle Hint / 16, 24 / Filled & Regular
  - Clock / 16 / Filled & Regular
  - Clock Bill / 16, 20, 24, 32 / Filled & Regular
  - Clock Lock / 16 / Filled & Regular
  - Diamond / 16, 20, 24, 28, 32, 48 / Filled & Regular
  - Directions / 16, 20, 24 / Filled & Regular
  - Document / 32, 48 / Filled & Regular
  - Document Lightning / 32 / Light
  - Document Signature / 32 / Light
  - People Community / 12, 16, 20, 24, 28, 32, 48 / Filled & Light & Regular
  - People Community Add / 20, 24, 28 / Filled & Regular
  - TextBox Align Center / 20 / Filled
  - TextBox More / 20 / Filled
  - TextBox Rotate 90 / 20 / Filled
  - TextBox Settings / 20 / Filled


- Emoji packages have been updated to the latest version of the Fluent Emoji collection.

## V4.10.4

### General
- \[General\] Supports .NET 9 GA version

### Components
- \[Accordion\] Fix RTL specific styling ([#2917](https://github.com/microsoft/fluentui-blazor/pull/2917))
- \[Autocomplete\] Fix Icon Titles (Dismiss and Search) and delete predefined element ([#2891](https://github.com/microsoft/fluentui-blazor/pull/2891))
- \[Autocomplete\] Fix the Autocomplete Clear button ([#2906](https://github.com/microsoft/fluentui-blazor/pull/2906))
- \[Calendar\] Fix the Calendar disabled day in RTL direction ([#2909](https://github.com/microsoft/fluentui-blazor/pull/2909))
- \[DataGrid\] Fix `SelectAll` after reloading data when the `Virtualize` is set ([#2915](https://github.com/microsoft/fluentui-blazor/pull/2915))
- \[DataGrid\] Remove optimalization check as it can break in certain scenarios ([#2875](https://github.com/microsoft/fluentui-blazor/pull/2875))
- \[KeyCode\] Add a new `StopRepeat` property ([#2908](https://github.com/microsoft/fluentui-blazor/pull/2908))
- \[MessageBar\] Implement ClearAfterNavigation on provider level ([#2919](https://github.com/microsoft/fluentui-blazor/pull/2919))
- \[Select\] Fix ValueChanged regression [#2923](https://github.com/microsoft/fluentui-blazor/issues/2923)
- \[Tab\] Use correct color for close icon ([#2922](https://github.com/microsoft/fluentui-blazor/issues/2922))
- \[TreeItem\] Only pass initially selected state to the web component ([#2916](https://github.com/microsoft/fluentui-blazor/pull/2916))

### Demo site and documentation
- \[Docs\] Update README with Blazor Hybrid workaround note ([#2892](https://github.com/microsoft/fluentui-blazor/pull/2892))

### Icons
- Update to Fluent UI System Icons 1.1.265 (changes since 1.1.261)

 **What's new (Name / Size(s) / Variant(s))**
 - Arrow Bounce / 12, 28, 48 / Filled & Regular
 - Arrow Clockwise Dashes / 28, 48 / Filled & Regular
 - Arrow Down Left / 12, 28 / Filled & Regular
 - Arrow Flow Diagonal Up Right / 12, 28, 48 / Filled & Regular
 - Arrow Up Right / 12, 28 / Filled & Regular
 - Arrow Up Right Dashes / 12, 20, 24, 28, 32, 48 / Filled & Regular
 - Arrow Wrap / 32 / Filled & Regular
 - Arrow Wrap Up To Down / 20, 32 / Filled & Regular
 - Coin Multiple / 16, 24 / Filled & Regular
 - Comment Badge / 16, 20, 24 / Filled & Regular
 - Data Usage / 28, 32, 48 / Filled & Regular
 - Data Usage Checkmark / 16, 20, 24, 28, 32, 48 / Filled & Regular
 - Document One Page Multiple / 16, 20, 24 / Filled & Regular
 - Document One Page Multiple / 16, 20, 24 / Filled & Regular
 - Document One Page Multiple Sparkle / 16, 20, 24 / Filled & Regular
 - Document One Page Multiple Sparkle / 16, 20, 24 / Filled & Regular
 - Line Horizontal 1 Dash Dot Dash / 20 / Filled & Regular
 - Line Horizontal 1 Dot / 20 / Filled & Regular
 - Line Horizontal 3 / 16, 24, 28, 32, 48 / Filled & Regular
 - Navigation / 28, 32, 48 / Filled & Regular
 - Pause Circle / 16 / Filled & Regular
 - Stack / 28, 48 / Filled & Regular
 - Stack Off / 16, 20, 24, 28, 32, 48 / Filled & Regular
 - Text Bullet List Square / 28 / Filled & Regular
 - TextBox / 20, 28, 32, 48 / Filled & Regular
 - TextBox Checkmark / 16, 20, 24, 28, 32, 48 / Filled & Regular


 **What's updated (Name / Size(s) / Variant(s))**
 - Add Circle / 16 / Filled & Regular
 - Arrow Clockwise Dashes / 16, 20, 24, 32 / Filled & Regular
 - Arrow Counterclockwise Dashes / 20, 24 / Filled & Regular
 - Coin Multiple / 20 / Filled & Regular
 - Subtract Circle Arrow Back / 16 / Filled & Regular
 - Subtract Circle Arrow Forward / 16 / Filled & Regular
 - Subtract Circle / 16 / Filled & Regular
 - TextBox / 16 / Filled

## V4.10.3

### Important note
- The main class name for the `FluentAppBar` component has been renamed from `nav-menu-container` to `fluent-appbar`. If you have custom CSS in your app targeting that class, you need to change that to you the new class name.

### General
- \[General\] Apply global color-scheme to reflect theme in use ([#2854](https://github.com/microsoft/fluentui-blazor/pull/2854))

### Components
- \[AppBar\] Make it work in horizontal orientation as well ([#2760](https://github.com/microsoft/fluentui-blazor/pull/2760))
- \[AutoComplete\] Add KeepOpen property ([#2829](https://github.com/microsoft/fluentui-blazor/pull/2829))
- \[Autocomplete\] Fix the Autocomplete search rendering on slow connections ([#2820](https://github.com/microsoft/fluentui-blazor/pull/2820))
- \[ComboBox\] Fix OnValueChanged being called multiple times ([#2855](https://github.com/microsoft/fluentui-blazor/pull/2855))
- \[DataGrid\] Fix resize in RTL mode ([#2843](https://github.com/microsoft/fluentui-blazor/pull/2843))
- \[Dialog\] Prevent tooltip from being shown on load ([#2856](https://github.com/microsoft/fluentui-blazor/pull/2856))
- \[FileInput\] Fix uploading the same file twice by drag&drop ([#2865](https://github.com/microsoft/fluentui-blazor/pull/2865))
- \[Label\] Add CustomColor parameter and implementation ([#2828](https://github.com/microsoft/fluentui-blazor/pull/2828))
- \[Menu\/Tooltip] Fixed incorrectly previously ([#2790](https://github.com/microsoft/fluentui-blazor/pull/2790))
- \[MenuItem\] Add KeepOpen parameter ([#2852](https://github.com/microsoft/fluentui-blazor/pull/2852))
- \[MenuProvider\] Check whether the FluentMenuProvider is included ([#2793](https://github.com/microsoft/fluentui-blazor/pull/2793))
- \[MessageBox\] Add settable primary action text for Show... methods ([#2808](https://github.com/microsoft/fluentui-blazor/pull/2808))
- \[NumberField\] Fix looping error ([#2807](https://github.com/microsoft/fluentui-blazor/pull/2807))
- \[Select\] Fix multiple issues ([#2840](https://github.com/microsoft/fluentui-blazor/pull/2840))
- \[Slider\] Make label respond to `Disabled` state ([#2796](https://github.com/microsoft/fluentui-blazor/pull/2796))
- \[Splitter\] Fix trimming issue ([#2859](https://github.com/microsoft/fluentui-blazor/pull/2859))
- \[Stack\] Added 'Stretch' horizontal alignment option ([#2800](https://github.com/microsoft/fluentui-blazor/pull/2800))
- \[Templates\] Fix typo 'paceholder' ([#2801](https://github.com/microsoft/fluentui-blazor/pull/2801))
- \[TreeView\] Fix OnSelectedChange when using Items ([#2811](https://github.com/microsoft/fluentui-blazor/pull/2811))

### Demo site and documentation
- \[Docs\] Make MenuProvider message more prominent ([#2792](https://github.com/microsoft/fluentui-blazor/pull/2792))

### Icons
- Update to Fluent UI System Icons 1.1.261 (changes since 1.1.260) plus a fix for not using the right colors in the new 'Color icons.

 **What's new (Name / Size(s) / Variant(s))**
 - Arrow Down Right / 16, 20, 24, 32, 48 / Filled & Regular
 - Arrow Repeat All / 28, 48 / Filled & Regular
 - Attach / 28, 48 / Filled & Regular
 - Calendar Mention / 16 / Filled & Regular
 - Calendar Person / 16, 20, 32 / Filled & Regular
 - Comment Multiple Mention / 16, 20 / Filled & Regular
 - Document Text / 28, 32, 48 / Filled & Regular
 - Equal Circle / 16 / Filled & Regular
 - Folder Document / 16, 20, 24, 28 / Filled & Regular
 - Mail Inbox Person / 16, 20, 32 / Filled & Regular
 - Mail Inbox / 32 / Filled & Regular
 - Mail Read Multiple / 32 / Light
 - Plug Connected / 28, 32, 48 / Filled & Regular

 **What's updated (Name / Size(s) / Variant(s))**
 - Calendar Mention / 20 / Filled & Regular
 - Comment Mention / 16, 20 / Filled & Regular
 - Document Mention / 16, 20 / Filled & Regular
 - Document One Page Multiple / 16, 20, 24 / Filled & Regular

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
- \[TreeView\] prevent runtime errors in change handling ([#2776](https://github.com/microsoft/fluentui-blazor/pull/2776))

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
- \[GitHub\] Update the actions/upload-artifact to v4 ([#2650](https://github.com/microsoft/fluentui-blazor/pull/2650))

## Accessibility
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
- \[JavaScript Caching\] Do not add version number to lib.module.js file ([#2572](https://github.com/microsoft/fluentui-blazor/pull/2572))

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
- \[Examples\] Add 2024 Olympics data and let examples use that

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
