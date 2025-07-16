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

## v4.9.3

### Components
- \[General\] Inject LibraryConfiguring into DesignToken ([#2426](https://github.com/microsoft/fluentui-blazor/pull/2426))
- \[Accordion\] Add Id ([#2420](https://github.com/microsoft/fluentui-blazor/pull/2420))
- \[Templates\] Replace faulty `Appearance.Filled` with `Appearance.Accent` for buttons ([#2436](https://github.com/microsoft/fluentui-blazor/pull/2436))

### Demo site and documentation
- \[Docs\] Fix broken GitHub link ([#2442](https://github.com/microsoft/fluentui-blazor/pull/2442))


## v4.9.2
### Components
- \[General\] JavaScript Cache management ([#2388](https://github.com/microsoft/fluentui-blazor/pull/2388))
- \[DataGrid\] Avoid excessive calling of reinitialize method in JavaScript ([#2403](https://github.com/microsoft/fluentui-blazor/pull/2403))
- \[DatePicker\] Fix default position for RTL layout ([#2372](https://github.com/microsoft/fluentui-blazor/pull/2372))
- \[MenuButton\] Fix icon size ([#2374](https://github.com/microsoft/fluentui-blazor/pull/2374))
- \[MultiSplitter\] Fix RTL direction ([#2380](https://github.com/microsoft/fluentui-blazor/pull/2380))
- \[Overflow\] Add `Fixed` parameter ([#2393](https://github.com/microsoft/fluentui-blazor/pull/2393))
- \[Overflow\] Add `Fixed` enumeration ([#2401](https://github.com/microsoft/fluentui-blazor/pull/2401))
- \[Search\] Add `AutoComplete` parameter ([#2397](https://github.com/microsoft/fluentui-blazor/pull/2397))
- \[Select\] Fix logic when setting new option ([#2384](https://github.com/microsoft/fluentui-blazor/pull/2384))
- \[SortableList\] Adding Properties `FromListId` and `ToListId` ([#2385](https://github.com/microsoft/fluentui-blazor/pull/2385))
- \[SplashScreen\] Pass parameters.Modal to `ShowSplashScreen...` methods ([#2398](https://github.com/microsoft/fluentui-blazor/pull/2398))
- \[Wizard\] Add `GoToStepAsync` method ([#2383](https://github.com/microsoft/fluentui-blazor/pull/2383))
- \[Wizard\] Fix validation when `@bind-Value` is set ([#2364](https://github.com/microsoft/fluentui-blazor/pull/2364))

### Demo site and documentation
- \[Docs\] Better column distribution for homepage ([#2360](https://github.com/microsoft/fluentui-blazor/pull/2360))
- \[Docs\] Improve API Documentation performance ([#2377](https://github.com/microsoft/fluentui-blazor/pull/2377))
- \[Docs\] Fix some typos ([#2400](https://github.com/microsoft/fluentui-blazor/pull/2400))

### Icons
- Update to Fluent UI System Icons 1.1.249
    
    **What's new (Name / Size(s) / Variant(s))**
    - Arrow Download / 32 / Light
    - Color Fill Accent / 32 / Light
    - Edit Lock / 16, 20, 24 / Filled & Regular
    - Layout Row Two Settings / 20, 24, 28, 32 / Filled & Light & Regular
    - Molecule / 32 / Light
    - Person Board / 20 / Filled & Regular
    - Person Square Checkmark / 16, 20, 24 / Filled & Regular
    - Presence Tentative / 10, 12, 16, 20, 24 / Regular
    - Shield Add / 28, 32, 48 / Filled & Regular
    - Shield Checkmark / 32 / Filled & Regular
    - Shield Task / 32 / Filled & Regular
    - Toolbox / 32 / Filled & Light & Regular
    - Warning Lock Open / 16, 20, 24 / Filled & Regular
 
    **What's updated (Name / Size(s) / Variant(s))**
    - Sound Wave Circle / 20 / Filled & Regular
    - Vehicle Motorcycle / 16, 20, 24, 28, 32, 48 / Filled & Regular

## v4.9.1
### Components
- \[DataGrid\] Make `Grid` in `ColumnBase` protected ([#2342](https://github.com/microsoft/fluentui-blazor/pull/2342))
- \[DataGrid\] Several issues addressed ([#2347](https://github.com/microsoft/fluentui-blazor/pull/2347))
- \[DatePicker\] Add 'PickerMonthChanged' event and 'DaysTemplate' parameter ([#2336](https://github.com/microsoft/fluentui-blazor/pull/2336))
- \[Multi Splitter\] Fix the Splitter OnMouseMove ([#2333](https://github.com/microsoft/fluentui-blazor/pull/2333))

### Demo site and documentation
- \[Docs\] Fix Tabs Dynamic example  ([#2341](https://github.com/microsoft/fluentui-blazor/pull/2341))
- \[Docs\] Move Date and Autocomplete components to Forms section ([#2330](https://github.com/microsoft/fluentui-blazor/pull/2330))
- \[Docs\] Update homepage ([#2324](https://github.com/microsoft/fluentui-blazor/pull/2324))
- \[Docs\] Update the Variant selector in Icon Explorer ([#2331](https://github.com/microsoft/fluentui-blazor/pull/2331))


## v4.9.0
### Demo site and documentation
- \[Docs\] Add keyhandler for '/' to jump to search box
- \[Docs\] Add icon preview in API documentation ([#2284](https://github.com/microsoft/fluentui-blazor/pull/2284))
- \[Docs\] Added RTL section to CodeSetup.md ([#2242](https://github.com/microsoft/fluentui-blazor/pull/2242))
- \[Docs\] Improve API documentation by showing default icon and string values ([#2269](https://github.com/microsoft/fluentui-blazor/pull/2269))
- \[Docs\] Improve API documentation menu button ([#2270](https://github.com/microsoft/fluentui-blazor/pull/2270))
- \[Docs\] Fix same typo 5 places ([#2216](https://github.com/microsoft/fluentui-blazor/pull/2216))
- \[Demo\] Fixes link for Time picker in demo projects ([#2246](https://github.com/microsoft/fluentui-blazor/pull/2246))

### Components
- \[Rating\] **New component** ([#2258](https://github.com/microsoft/fluentui-blazor/pull/2258))
- \[AppBar\] Fix regression with icon accent color ([#2318](https://github.com/microsoft/fluentui-blazor/pull/2318))
- \[Autocomplete\] Accessibility: Scrolling not working with keyboard ([#2221](https://github.com/microsoft/fluentui-blazor/pull/2221))
- \[AutoComplete\] Allow focusable Badge dismiss buttons (accessibility) ([#2272](https://github.com/microsoft/fluentui-blazor/pull/2272))
- \[Autocomplete\] Fix AriaLabel ([#2303](https://github.com/microsoft/fluentui-blazor/pull/2303))
- \[Autocomplete\] Fix ReadOnly and Disable properties ([#2291](https://github.com/microsoft/fluentui-blazor/pull/2291))
- \[Combobox\] Set value if selection is cleared ([#2307](https://github.com/microsoft/fluentui-blazor/pull/2307))
- \[Combobox, Select\] Fix the Placeholder attribute ([#2311](https://github.com/microsoft/fluentui-blazor/pull/2311))
- \[DataGrid\] Add OnCellClick event and SelectColumn.SelectFromEntireRow property ([#2252](https://github.com/microsoft/fluentui-blazor/pull/2252))
- \[DataGrid\] Add WCAG 2.2 single-click column resize capability ([#2238](https://github.com/microsoft/fluentui-blazor/pull/2238))
- \[DataGrid\] Make `PropertyColumn` use `DisplayAttribute` value for enum ([#2304](https://github.com/microsoft/fluentui-blazor/pull/2304))
- \[DataGrid\] Refactoring ColumnBase events ([#2298](https://github.com/microsoft/fluentui-blazor/pull/2298))
- \[DataGrid\] Revert setting height on DataGrid header cell again. Leads to issues on Safari
- \[DatePicker\] Change OnSelectedDateAsync logic ([#2233](https://github.com/microsoft/fluentui-blazor/pull/2233))
- \[DatePicker\] Fix unable to set when value is null ([#2241](https://github.com/microsoft/fluentui-blazor/pull/2241))
- \[DesignTheme\] Add try catch ([#2204](https://github.com/microsoft/fluentui-blazor/pull/2204))
- \[DesignToken\] Change `FillColor` and `NeutralLayer...` types  ([#2266](https://github.com/microsoft/fluentui-blazor/pull/2266))
- \[Dialog\] Fix failing tests ([#2283](https://github.com/microsoft/fluentui-blazor/pull/2283))
- \[DialogService\] Fix UpdateDialogAsync to refresh parameters and content ([#2310](https://github.com/microsoft/fluentui-blazor/pull/2310))
- \[Lists\] Pass `Name` parameter ([#2305](https://github.com/microsoft/fluentui-blazor/pull/2305))
- \[Lists\] Add aria-selected to fluent-option ([#2316](https://github.com/microsoft/fluentui-blazor/pull/2316))
- \[Multisplitter\] Fix width panels ([#2218](https://github.com/microsoft/fluentui-blazor/pull/2218))
- \[MultiSplitter\] Prevent semicolons from being displayed. ([#2226](https://github.com/microsoft/fluentui-blazor/pull/2226))
- \[NavMenu\] Revert fix for when prerender set to false ([#2293](https://github.com/microsoft/fluentui-blazor/pull/2293))
- \[NumberField\] Add sbyte type ([#2308](https://github.com/microsoft/fluentui-blazor/pull/2308))
- \[Overflow\] Add refresh method, add VisibleOnLoad parameter ([#2236](https://github.com/microsoft/fluentui-blazor/pull/2236))
- \[ProfileMenu\] Fix the initials inner the popup panel ([#2294](https://github.com/microsoft/fluentui-blazor/pull/2294))
- \[Slider\] Fix getting caught in update loop ([#2265](https://github.com/microsoft/fluentui-blazor/pull/2265))
- \[Slider\] Refactor code after PR #2265 ([#2287](https://github.com/microsoft/fluentui-blazor/pull/2287))
- \[TimePicker\] Add null check for empty string ([#2245](https://github.com/microsoft/fluentui-blazor/pull/2245))

### Miscellaneous
- Add CssBuilder.ValidateClassNames  ([#2255](https://github.com/microsoft/fluentui-blazor/pull/2255))
- 2 icons used in library were using wrong variant ([#2260](https://github.com/microsoft/fluentui-blazor/pull/2260))



### Icons
- Introduction of a new 'Light' variant of icons. For now a set of 153 icons in size 32 has been added.

- Update to Fluent UI System Icons 1.1.247
    
    **What's new (Name / Size(s) / Variant(s))**
    - Accessibility More / 16, 20, 24 / Filled & Regular
    - Battery 0 / 28, 32 / Filled & Regular
    - Battery 1 / 28, 32 / Filled & Regular
    - Battery 10 / 28, 32 / Filled & Regular
    - Battery 2 / 28, 32 / Filled & Regular
    - Battery 3 / 28, 32 / Filled & Regular
    - Battery 4 / 28, 32 / Filled & Regular
    - Battery 5 / 28, 32 / Filled & Regular
    - Battery 6 / 28, 32 / Filled & Regular
    - Battery 7 / 28, 32 / Filled & Regular
    - Battery 8 / 28, 32 / Filled & Regular
    - Battery 9 / 28, 32 / Filled & Regular
    - Battery Charge / 28, 32 / Filled & Regular
    - Calendar Sparkle / 32 / Light
    - Chat / 32 / Light
    - Coin Stack / 16, 20, 24 / Filled & Regular
    - Database Arrow Up / 16 / Filled & Regular
    - Game Chat / 20 / Filled & Regular
    - Layout Row Two Focus Top Settings / 20, 28, 32 / Filled
    - Layout Row Two Focus Top Settings / 32 / Light
    - Layout Row Two Focus Top / 28 / Filled
    - Paint Brush / 12 / Filled & Regular
    - Panel Right / 12 / Filled & Regular
    - People Edit / 32 / Filled & Regular
    - People Edit / 32 / Light
    - Person Home / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Person Mail / 32 / Filled & Regular
    - Person Mail / 32 / Light
    - Puzzle Piece / 12 / Filled & Regular
    - Teaching / 20 / Filled & Regular
 
     **What's updated (Name / Size(s) / Variant(s))**
     - Arrow Forward Down Lightning / 20, 24 / Filled & Regular
     - Notebook Lightning / 20, 24 / Filled & Regular

- Update to Fluent UI System Icons 1.1.245
    
    **What's new (Name / Size(s) / Variant(s))**
    - Arrow Collapse All / 16 / Filled & Regular
    - Arrow Expand All / 16, 20, 24 / Filled & Regular
    - Chat Arrow Back Down / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Desktop Arrow Down / 32 / Filled & Regular
    - Edit Line Horizontal 3 / 20, 24 / Filled & Regular
    - Gift Open / 32 / Filled & Regular
    - Prompt / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Search Sparkle / 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Slide Text Call / 16, 20, 24, 28, 48 / Filled & Regular
    - Slide Text Cursor / 20, 24 / Filled & Regular    
    - Vehicle Motorcycle / 16, 20, 24, 28, 32, 48 / Filled & Regular

     **What's updated (Name / Size(s) / Variant(s))**
    - Arrow Collapse All / 20, 24 / Filled & Regular

## 4.8.1
- Add an new `Light` value to the `IconVariant` enum to enable publishing new icon packages with these icons

## v4.8.0

### Breaking change
- \[Option\] Change Option<TType>.Text type to `string?` ([#2063](https://github.com/microsoft/fluentui-blazor/pull/2063)). See also discussion [#2062](https://github.com/microsoft/fluentui-blazor/discussions/2062)

### Demo site and documentation 
- Add DataGridTableScroll demo ([#2098](https://github.com/microsoft/fluentui-blazor/pull/2098))
- Adds office color table to Design Tokens page ([#2073](https://github.com/microsoft/fluentui-blazor/pull/2073))
- Cleans up more code snippets ([#2173](https://github.com/microsoft/fluentui-blazor/pull/2173))
- Cleanup DialogService Page ([#2171](https://github.com/microsoft/fluentui-blazor/pull/2171))
- Fix Page Title typo - Project Setup ([#2054](https://github.com/microsoft/fluentui-blazor/pull/2054))
- Fixes minor typo in DesignTokens.md ([#2071](https://github.com/microsoft/fluentui-blazor/pull/2071))
- Removes FluentPersona DemoSection from AutoComplete page ([#2135](https://github.com/microsoft/fluentui-blazor/pull/2135))
- Toast examples - TimeOut setting in milliseconds ([#2025](https://github.com/microsoft/fluentui-blazor/pull/2025))

### Components
- \[AppBar\] Introduce IAppBarItem to enable better extensibility ([#2083](https://github.com/microsoft/fluentui-blazor/pull/2083))
- \[Autocomplete\] Accessibility - Add "x of y" aria-label when item is selected ([#2019](https://github.com/microsoft/fluentui-blazor/pull/2019))
- \[Autocomplete\] Accessibility - Add aria-selected on FluentOption ([#2018](https://github.com/microsoft/fluentui-blazor/pull/2018))
- \[Autocomplete\] Accessibility - Update style when "forced-colors: active" ([#2109](https://github.com/microsoft/fluentui-blazor/pull/2109))
- \[Autocomplete\] Add `ImmediateDelay` parameter ([#2052](https://github.com/microsoft/fluentui-blazor/pull/2052))
- \[Autocomplete\] Add SelectValueOnTab attribute ([#2110](https://github.com/microsoft/fluentui-blazor/pull/2110))
- \[Autocomplete\] Set the height automatically ([#2045](https://github.com/microsoft/fluentui-blazor/pull/2045))
- \[Card\] Support AreaRestricted for MinimalStyle as well ([#2170](https://github.com/microsoft/fluentui-blazor/pull/2170))
- \[ComboBox\] Fix bind-SelectedOption is null when using bind-SelectedOption:after ([#2102](https://github.com/microsoft/fluentui-blazor/pull/2102))
- \[DataGrid\] Allow programmatic sorting of columns by name or index ([#2156](https://github.com/microsoft/fluentui-blazor/pull/2156))
- \[DataGrid\] Check rowelements when correcting column options ([#2117](https://github.com/microsoft/fluentui-blazor/pull/2117))
- \[DataGrid\] Fix LoadingContent height when Virtualize is true ([#2188](https://github.com/microsoft/fluentui-blazor/pull/2188))
- \[DataGrid\] Prevent drag handle from disappearing when column width made too small ([#2059](https://github.com/microsoft/fluentui-blazor/pull/2059))
- \[DataGrid\] SelectColumn - Add ClearSelection ([#2164](https://github.com/microsoft/fluentui-blazor/pull/2164))
- \[DataGrid\] SelectColumn - Allow customization of SelectAll header ([#2106](https://github.com/microsoft/fluentui-blazor/pull/2106))
- \[DataGrid\] SelectColumn - Make it work with ItemsProvider ([#2060](https://github.com/microsoft/fluentui-blazor/pull/2060))
- \[DatePicker, Calendar\] Fix not updating correctly when used with EditContext ([#2047](https://github.com/microsoft/fluentui-blazor/pull/2047))
- \[DesignToken\] Add WithDefault overload ([#2159](https://github.com/microsoft/fluentui-blazor/pull/2159))
- \[DialogService\] Add missing classes to avoid Trimming exceptions ([#2099](https://github.com/microsoft/fluentui-blazor/pull/2099))
- \[Header\] Add Id to the container header tag ([#2125](https://github.com/microsoft/fluentui-blazor/pull/2125))
- \[Icon\] Minor cleanup and clarification ([#2038](https://github.com/microsoft/fluentui-blazor/pull/2038))
- \[InputBase\] Adds FieldIdentifier parameter ([#2114](https://github.com/microsoft/fluentui-blazor/pull/2114))
- \[InputBase\] Use `Debouncer` instead of `PeriodicTimer` for debouncing `ValueChanged` handler with `ImmediateDelay`. ([#2042](https://github.com/microsoft/fluentui-blazor/pull/2042))
- \[KeyCode\] Add KeyUp event ([#2122](https://github.com/microsoft/fluentui-blazor/pull/2122))
- \[Layout\] Add Id to the container div ([#2123](https://github.com/microsoft/fluentui-blazor/pull/2123))
- \[Layout\] Add Orientation support ([#2096](https://github.com/microsoft/fluentui-blazor/pull/2096))
- \[Lists\] Make list components inherit from InputBase ([#2118](https://github.com/microsoft/fluentui-blazor/pull/2118))
- \[MarkdownSection\] Adds border around code snippets ([#2039](https://github.com/microsoft/fluentui-blazor/pull/2039))
- \[MarkdownSection\] Optimize to minimize number of calls to OnContentConverted ([#2092](https://github.com/microsoft/fluentui-blazor/pull/2092))
- \[Misc\] Add the ability for CssBuilder to properly Add multiple classes without duplicates ([#2104](https://github.com/microsoft/fluentui-blazor/pull/2104))
- \[Misc\] Code cleanup work ([#2184](https://github.com/microsoft/fluentui-blazor/pull/2184))
- \[Misc\] Package versions refactoring ([#2131](https://github.com/microsoft/fluentui-blazor/pull/2131))
- \[MultiSplitter\] Remove experimental label and make fixed panel size work ([#2196](https://github.com/microsoft/fluentui-blazor/pull/2196))
- \[NavMenu\] Enhance working in mobile view ([#2183](https://github.com/microsoft/fluentui-blazor/pull/2183))
- \[NavMenu\] Fix double click needed for expanding/collapsing child elements when prerendering is turned off ([#2133](https://github.com/microsoft/fluentui-blazor/pull/2133))
- \[NavMenu\] Fix FluentNavLink to support CustomColor ([#2116](https://github.com/microsoft/fluentui-blazor/pull/2116))
- \[Pipeline\] Add a task to publish the preview package in a special feed ([#2094](https://github.com/microsoft/fluentui-blazor/pull/2094))
- \[ProfileMenu\] Add `StartTemplate` and `EndTemplate` parameters ([#2010](https://github.com/microsoft/fluentui-blazor/pull/2010))
- \[Progress\] Fix height in Indeterminate mode by using InvariantCulture ([#2120](https://github.com/microsoft/fluentui-blazor/pull/2120))
- \[Templates\] Use correct autocomplete values when registering new account ([#2064](https://github.com/microsoft/fluentui-blazor/pull/2064))
- \[Templates\] Use 'double provider' fix in more places
- \[TreeView\] Add `Items` and `LazyLoadItems` properties ([#1945](https://github.com/microsoft/fluentui-blazor/pull/1945))
- \[Wizard\] Fix the Wizard Left position in RTL mode ([#2026](https://github.com/microsoft/fluentui-blazor/pull/2026))

### Icons
- Update to Fluent UI System Icons 1.1.242

    **What's new (Name / Size(s) / Variant(s))**
    - Breakout Room / 32 / Filled & Regular
    - Card UI Portrait Flip / 16, 20, 24 / Filled & Regular
    - Cursor / 28, 32 / Filled & Regular
    - Layout Row Two / 28, 48 / Filled & Regular
    - Notepad Sparkle / 16, 20, 24, 28, 32 / Filled & Regular
    - Paint Brush Subtract / 16, 20, 24, 28, 32 / Filled & Regular
    - Paint Brush / 28 / Filled & Regular
    - Play Circle Sparkle / 16, 20, 24 / Filled & Regular
    - Replay / 16, 20, 24, 28, 32 / Filled & Regular
    - Send Person / 16, 20, 24 / Filled & Regular
    - Square Dovetail Joint / 12, 16, 20, 24, 28, 32, 48 / Filled & Regular
    - Table Cursor / 16, 20, 24 / Filled & Regular
    - Transparency Square / 20, 24 / Filled & Regular
 
     **What's updated (Name / Size(s) / Variant(s))**
    - Notepad / 32 / Filled & Regular
    - Replay / 20 / Filled & Regular
   
- Update to Fluent UI System Icons 1.1.239

    **What's new (Name / Size(s) / Variant(s))**
    - Arrow Turn Right / 16 / Filled & Regular
    - Chart Multiple / 16 / Filled & Regular
    - Column / 24 / Filled & Regular
    - Data Pie / 16 / Filled & Regular
    - Layout Column Two Focus Left / 32 / Filled
    - Layout Column Two Focus Right / 32 / Filled
    - Layout Column Two / 32 / Filled & Regular
    - Layout Row Two Focus Top / 32 / Filled
    - Layout Row Two / 32 / Filled & Regular
    - Mail Copy / 32 / Filled & Regular
    - Paint Brush Sparkle / 20, 24 / Filled & Regular
    - People Community / 12 / Filled & Regular
    - Person Board / 12 / Filled & Regular
    - Person Tentative / 16, 20, 24 / Filled & Regular
    - Tab Desktop Search / 16, 20, 24 / Filled & Regular
    - Table Sparkle / 20, 24 / Filled & Regular
 
    **What's updated (Name / Size(s) / Variant(s))**
    - Chart Multiple / 20, 24 / Filled & Regular
    - Column Edit / 24 / Filled & Regular
    - Data Pie / 24 / Filled & Regular
    
## v4.7.2

### Demo site and documentation 
- \[Docs\] New Video and PageTitle ([#1925](https://github.com/microsoft/fluentui-blazor/pull/1925))

### Components
- \[AppBar\] Allow for omitting Href on AppBarItems and don't show active status when Href is null or not specified ([#1976](https://github.com/microsoft/fluentui-blazor/pull/1976))
- \[Autocomplete\] Accessibility: Tab key to go to the Close Button ([#2007](https://github.com/microsoft/fluentui-blazor/pull/2007))
- \[Badge\] Do not use a div (block element) in an inline element ([#1921](https://github.com/microsoft/fluentui-blazor/pull/1921))
- \[DataGrid\] Add ShowHover parameter and implement row hover effect ([#1939](https://github.com/microsoft/fluentui-blazor/pull/1939))
- \[DataGrid\] Apply ItemSize (CSS height) to cells when grid is virtualized.  ([#1936](https://github.com/microsoft/fluentui-blazor/pull/1936))
- \[DataGrid\] Change column width when using generated value  ([#1955](https://github.com/microsoft/fluentui-blazor/pull/1955))
- \[DataGrid\] Give empty row an id ([#2001](https://github.com/microsoft/fluentui-blazor/pull/2001))
- \[DataGrid\] Multiselect feature, adding a `SelectColumn` ([#1952](https://github.com/microsoft/fluentui-blazor/pull/1952))
- \[DataGrid\] Revert setting height on DataGrid header cell. Was not necessary and lead to issues on Safari
- \[InputLabel\] Add Orientation parameter  ([#1994](https://github.com/microsoft/fluentui-blazor/pull/1994))
- \[Lists\] Also set Value (and InternalValue) when SelectedOption is set ([#1970](https://github.com/microsoft/fluentui-blazor/pull/1970))
- \[MessageBar\] Supply a default value for the message intent when using MessageOptions ([#1993](https://github.com/microsoft/fluentui-blazor/pull/1993))
- \[NavMenu\] Fix keyboard navigation ([#1950](https://github.com/microsoft/fluentui-blazor/pull/1950))
- \[NavMenu\] Make sure text is shown when in mobile view in SSR. 
- \[ProfileMenu\] Remove ProfileMenu from demo site header and change examples ([#1924](https://github.com/microsoft/fluentui-blazor/pull/1924))
- \[Tabs\] Not using ClassValue and StyleValue when rendering ([#1931](https://github.com/microsoft/fluentui-blazor/pull/1931))
- \[Tabs\] Prevent error 'An item with the same key has already been added ([#2006](https://github.com/microsoft/fluentui-blazor/pull/2006))


### Miscellaneous
- \[Reboot\] Add body class (to body element declaration)
- \[Templates\] Change css so body-content height is set and scrollable ([#1981](https://github.com/microsoft/fluentui-blazor/pull/1981))
- \[Templates\] Change header gutter to keep content out of nav icon area when width < 600px

## v4.7.1

### Components
- \[Wizard\] Add StepSequence attribute ([#1909](https://github.com/microsoft/fluentui-blazor/pull/1909))
- \[DataGrid\] Fix GridTemplateColumns initialization was done in the wrong place (related to new Width option for columns

## v4.7.0

### Breaking change
- \[Extensions\] Add a namespace for extension methods to prevent possible naming clashes ([#1776](https://github.com/microsoft/fluentui-blazor/pull/1776))

### Demo site and documentation 
- \[Docs\] Update Progress stroke examples dropdowns ([#1835](https://github.com/microsoft/fluentui-blazor/pull/1835))
- \[Docs\] Update KeyCodeGlobalExample.razor.css to fix dark mode visuals ([#1834](https://github.com/microsoft/fluentui-blazor/pull/1834))
- \[Docs\] Update Highlighter example to use FluentTextField instead of input ([#1833](https://github.com/microsoft/fluentui-blazor/pull/1833))
- \[Demo\] Fix the Profile Menu position ([#1841](https://github.com/microsoft/fluentui-blazor/pull/1841))
- \[Demo\] Clears demo site cache on startup if new version ([#1839](https://github.com/microsoft/fluentui-blazor/pull/1839))

### Components
- \[AppBar\] Apply role requirements ([#1871](https://github.com/microsoft/fluentui-blazor/pull/1871))
- \[Calendar\] Use provided Culture for calendar operations ([#1862](https://github.com/microsoft/fluentui-blazor/pull/1862))
- \[DataGrid\] Add remove sort capability on columns ([#1826](https://github.com/microsoft/fluentui-blazor/pull/1826))
- \[DataGrid\] Add column Width parameter ([#1902](https://github.com/microsoft/fluentui-blazor/pull/1902))
- \[DataGrid\] Add SortBy parameter support for PropertyColumn ([#1877](https://github.com/microsoft/fluentui-blazor/pull/1877))
- \[DataGrid\] Extend GridSort possibilities ([#1860](https://github.com/microsoft/fluentui-blazor/pull/1860))
- \[DatePicker\] Enhance for month /year selection  ([#1903](https://github.com/microsoft/fluentui-blazor/pull/1903))
- \[Grid\] Add AdaptiveRendering property ([#1899](https://github.com/microsoft/fluentui-blazor/pull/1899))
- \[Input\] NotifyFieldChanged is called twice for all FluentInputBase derived components ([#1846](https://github.com/microsoft/fluentui-blazor/pull/1846))
- \[MenuButton\] Add ChildContent so items can be supplied manually ([#1848](https://github.com/microsoft/fluentui-blazor/pull/1848))
- \[Paginator\] spelling issue in FluentPaginator.razor.cs ([#1829](https://github.com/microsoft/fluentui-blazor/pull/1829))
- \[Popover\] adds FixedPlacement parameter ([#1894](https://github.com/microsoft/fluentui-blazor/pull/1894))
- \[PullToRefresh\] Add DragThreshold to PullToRefresh ([#1858](https://github.com/microsoft/fluentui-blazor/pull/1858))
- \[Select\] Allow value to be set when component is disabled ([#1892](https://github.com/microsoft/fluentui-blazor/pull/1892))
- \[Slider\] Fixes thumb redraw issues (Fix for #1836) ([#1873](https://github.com/microsoft/fluentui-blazor/pull/1873))
- \[Wizard\] Fix the Wizard bullet number style ([#1905](https://github.com/microsoft/fluentui-blazor/pull/1905))

### Miscellaneous
- \[Aspire\] Fix #3364 by adding IKeyCodeListner and handler ([#1866](https://github.com/microsoft/fluentui-blazor/pull/1866))
- \[Pipeline\] Add Code Coverage Report ([#1861](https://github.com/microsoft/fluentui-blazor/pull/1861))

### Icons
- Update to Fluent UI System Icons 1.1.237 

	**What's new (Name / Size(s) / Variant(s))**
	- Book / 48 / Filled & Regular
	- Camera Arrow Up / 16, 20, 24 / Filled & Regular
	- Chat Settings / 16 / Filled & Regular
	- Circle Highlight / 20, 24 / Filled & Regular
	- Circle Hint / 24 / Filled & Regular
	- Circle Shadow / 20, 24 / Filled & Regular
	- Content View / 16 / Filled & Regular
	- Double Tap Swipe Down / 16 / Filled & Regular
	- Double Tap Swipe Up / 16 / Filled & Regular
	- Flash Sparkle / 16 / Filled & Regular
	- Location Ripple / 12 / Filled & Regular
	- Search Square / 16 / Filled & Regular
	- Settings Chat / 16 / Filled & Regular
	- Share Multiple / 16, 20, 24 / Filled & Regular
	- Slide Play / 20, 24 / Filled & Regular
 
	**What's updated (Name / Size(s) / Variant(s))**
	- Book Add / 28 / Filled & Regular
	- Book Contacts / 20, 24, 28, 32 / Filled & Regular
	- Book / 28 / Filled & Regular   

## V4.6.1

### Demo site and documentation 
- \[Demo & docs\] Fix documentation error ([#1767](https://github.com/microsoft/fluentui-blazor/pull/1767))
- \[Demo & docs\] Home page cleanup ([#1763](https://github.com/microsoft/fluentui-blazor/pull/1763))
- \[Demo & docs\] Removes caching of markdown files for Server demo app ([#1822](https://github.com/microsoft/fluentui-blazor/pull/1822))
- \[Demo & docs\] Fix footer styling ([#1778](https://github.com/microsoft/fluentui-blazor/pull/1778))
- \[Demo & docs\] CSS files improvements ([#1807](https://github.com/microsoft/fluentui-blazor/pull/1807))
- \[Demo & docs\] The empty CSS rule in site.css has been removed ([#1809](https://github.com/microsoft/fluentui-blazor/pull/1809))

### Components
- \[AppBar\] Add Count parameter and facilitate OnClick without navigation ([#1790](https://github.com/microsoft/fluentui-blazor/pull/1790))
- \[CounterBadge\] Add ShowWhen, Dot, and VerticalPosition ([#1786](https://github.com/microsoft/fluentui-blazor/pull/1786))
- \[KeyCode, AnchoredRegion, Popover\] Add key navigation in AnchoredRegion / Popup ([#1800](https://github.com/microsoft/fluentui-blazor/pull/1800))
- \[NavMenu\] Show child items via `FluentMenu` when collapsed ([#1730](https://github.com/microsoft/fluentui-blazor/pull/1730))
- \[Persona\] Add capability for the name to appear before or after initials. ([#1750](https://github.com/microsoft/fluentui-blazor/pull/1750))
- \[ProfileMenu\] TopCorner property ([#1795](https://github.com/microsoft/fluentui-blazor/pull/1795))
- \[SplashScreen\] Fix ESC on SplashScreen ([#1811](https://github.com/microsoft/fluentui-blazor/pull/1811))

### Miscellaneous
- \[Misc\] Update to latest NuGet packages, SDK and Web Components script
- \[Templates\] CSS fixes and use specific package versions ([#1797](https://github.com/microsoft/fluentui-blazor/pull/1797))
- \[Templates\] Fixed CSS styling of the Blazor Fluent UI template AND fixed broken links in readme.md ([#1768](https://github.com/microsoft/fluentui-blazor/pull/1768))

### Icons
- Update to Fluent UI System Icons 1.1.234 ([#1823](https://github.com/microsoft/fluentui-blazor/pull/1823))

	**What's new (Name / Size(s) / Variant(s))**
	- Apps Settings / 16, 20 / Filled & Regular
	- Apps Shield / 16, 20 / Filled & Regular
	- Arrow Upload / 32 / Filled & Regular
	- Calendar Edit / 32 / Filled & Regular
	- Data Bar Vertical Arrow Down / 16, 20, 24 / Filled & Regular
	- Haptic Strong / 16, 20, 24 / Filled & Regular
	- Haptic Weak / 16, 20, 24 / Filled & Regular
	- Hexagon Sparkle / 20, 24 / Filled & Regular
	- Mail Edit / 32 / Filled & Regular
	- Password Clock / 48 / Filled & Regular
	- Password Reset / 48 / Filled & Regular
	- Password / 24, 32, 48 / Filled & Regular
	- People Eye / 16, 20 / Filled & Regular
	- Pin Globe / 16, 20 / Filled & Regular
	- Run / 28, 32, 48 / Filled & Regular
	- Tab Group / 16, 20, 24 / Filled & Regular

	**What's updated (Name / Size(s) / Variant(s))**
	- Arrow Upload / 24 / Filled & Regular
	- Calendar Edit / 16, 20, 24 / Filled & Regular
	- Mail Read / 20 / Filled & Regular
	- Password / 24 / Filled & Regular
	- Run / 16, 20, 24 / Filled & Regular

## V4.6.0

### Demo site, documentation and miscellaneous
- \[Demo & docs\] Add InputFile 'known issues' section ([#1680](https://github.com/microsoft/fluentui-blazor/pull/1680))
- \[Demo & docs\] Corrects spelling and grammar in templates page ([#1716](https://github.com/microsoft/fluentui-blazor/pull/1716))
- \[Demo & docs\] Fix Wizard link in navigation menu ([#1660](https://github.com/microsoft/fluentui-blazor/pull/1660))
- \[Demo & docs\] Minor corrections to CONTRIBUTING.md ([#1681](https://github.com/microsoft/fluentui-blazor/pull/1681))
- \[MarkdownSection\] Fixes border not showing in Markdown tables ([#1721](https://github.com/microsoft/fluentui-blazor/pull/1721))
- \[MarkdownSection\] Adds code highlighting ([#1737](https://github.com/microsoft/fluentui-blazor/pull/1737))
- \[MarkdownSection\] Fixes and enhancements ([#1751](https://github.com/microsoft/fluentui-blazor/pull/1751))
- \[Misc\] Add required .csproj settings for generating snupkg packages ([#1675](https://github.com/microsoft/fluentui-blazor/pull/1675))
- \[Misc\] Add ToColorHex extension to Swatch ([#1691](https://github.com/microsoft/fluentui-blazor/pull/1691))

### Components
- \[Accordion\] Add expanded value to custom event handler ([#1689](https://github.com/microsoft/fluentui-blazor/pull/1689))
- \[AppBarItem\] Add OnClick event callback ([#1698](https://github.com/microsoft/fluentui-blazor/pull/1698))
- \[Button\] Avoid padding on loading spinner when no text is shown ([#1714](https://github.com/microsoft/fluentui-blazor/pull/1714))
- \[InputFile\] Replace OnInitializedAsync with OnAfterRenderAsync ([#1661](https://github.com/microsoft/fluentui-blazor/pull/1661))
- \[KeyCode\] Allow content to avoid using the Anchor property ([#1743](https://github.com/microsoft/fluentui-blazor/pull/1743))
- **[KeyCodeProvider]** Add a global service to capture keystrokes ([#1740](https://github.com/microsoft/fluentui-blazor/pull/1740))
- \[MenuButton\] Make the menu anchored to the button so can float ([#1676](https://github.com/microsoft/fluentui-blazor/pull/1676))
- \[Pagination\] Add Disabled parameter ([#1713](https://github.com/microsoft/fluentui-blazor/pull/1713))
- \[Persona\] Manage the empty Name ([#1710](https://github.com/microsoft/fluentui-blazor/pull/1710))
- **[ProfileMenu]**  Add a new `FluentProfileMenu` ([#1705](https://github.com/microsoft/fluentui-blazor/pull/1705))
- **[PullToRefresh]** Add a new `FluentPullToRefresh` ([#1679](https://github.com/microsoft/fluentui-blazor/pull/1679))
- \[Wizard\] Add the ability to automatically validate an EditForm ([#1663](https://github.com/microsoft/fluentui-blazor/pull/1663))


- Update Fluent UI System Icons to 1.1.233

  **What's new (Name / Size(s) / Variant(s))**
	- Classification / 32 / Filled & Regular
	- Document Target / 20, 24, 32 / Filled & Regular
	- Emoji Meme / 16, 20, 24 / Filled & Regular
	- Hand Point / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Mail Read Briefcase / 48 / Filled & Regular
	- People Subtract / 20, 24, 32 / Filled & Regular
	- Person Alert Off / 16, 20, 24, 32 / Filled & Regular
	- Shopping Bag Add / 16 / Filled & Regular
	- Spatula Spoon /

## V4.6.1

### Demo site and documentation 
- \[Demo & docs\] Fix documentation error ([#1767](https://github.com/microsoft/fluentui-blazor/pull/1767))
- \[Demo & docs\] Home page cleanup ([#1763](https://github.com/microsoft/fluentui-blazor/pull/1763))
- \[Demo & docs\] Removes caching of markdown files for Server demo app ([#1822](https://github.com/microsoft/fluentui-blazor/pull/1822))
- \[Demo & docs\] Fix footer styling ([#1778](https://github.com/microsoft/fluentui-blazor/pull/1778))
- \[Demo & docs\] CSS files improvements ([#1807](https://github.com/microsoft/fluentui-blazor/pull/1807))
- \[Demo & docs\] The empty CSS rule in site.css has been removed ([#1809](https://github.com/microsoft/fluentui-blazor/pull/1809))

### Components
- \[AppBar\] Add Count parameter and facilitate OnClick without navigation ([#1790](https://github.com/microsoft/fluentui-blazor/pull/1790))
- \[CounterBadge\] Add ShowWhen, Dot, and VerticalPosition ([#1786](https://github.com/microsoft/fluentui-blazor/pull/1786))
- \[KeyCode, AnchoredRegion, Popover\] Add key navigation in AnchoredRegion / Popup ([#1800](https://github.com/microsoft/fluentui-blazor/pull/1800))
- \[NavMenu\] Show child items via `FluentMenu` when collapsed ([#1730](https://github.com/microsoft/fluentui-blazor/pull/1730))
- \[Persona\] Add capability for the name to appear before or after initials. ([#1750](https://github.com/microsoft/fluentui-blazor/pull/1750))
- \[ProfileMenu\] TopCorner property ([#1795](https://github.com/microsoft/fluentui-blazor/pull/1795))
- \[SplashScreen\] Fix ESC on SplashScreen ([#1811](https://github.com/microsoft/fluentui-blazor/pull/1811))

### Miscellaneous
- \[Misc\] Update to latest NuGet packages, SDK and Web Components script
- \[Templates\] CSS fixes and use specific package versions ([#1797](https://github.com/microsoft/fluentui-blazor/pull/1797))
- \[Templates\] Fixed CSS styling of the Blazor Fluent UI template AND fixed broken links in readme.md ([#1768](https://github.com/microsoft/fluentui-blazor/pull/1768))

### Icons
- Update to Fluent UI System Icons 1.1.234 ([#1823](https://github.com/microsoft/fluentui-blazor/pull/1823))

	**What's new (Name / Size(s) / Variant(s))**
	- Apps Settings / 16, 20 / Filled & Regular
	- Apps Shield / 16, 20 / Filled & Regular
	- Arrow Upload / 32 / Filled & Regular
	- Calendar Edit / 32 / Filled & Regular
	- Data Bar Vertical Arrow Down / 16, 20, 24 / Filled & Regular
	- Haptic Strong / 16, 20, 24 / Filled & Regular
	- Haptic Weak / 16, 20, 24 / Filled & Regular
	- Hexagon Sparkle / 20, 24 / Filled & Regular
	- Mail Edit / 32 / Filled & Regular
	- Password Clock / 48 / Filled & Regular
	- Password Reset / 48 / Filled & Regular
	- Password / 24, 32, 48 / Filled & Regular
	- People Eye / 16, 20 / Filled & Regular
	- Pin Globe / 16, 20 / Filled & Regular
	- Run / 28, 32, 48 / Filled & Regular
	- Tab Group / 16, 20, 24 / Filled & Regular

	**What's updated (Name / Size(s) / Variant(s))**
	- Arrow Upload / 24 / Filled & Regular
	- Calendar Edit / 16, 20, 24 / Filled & Regular
	- Mail Read / 20 / Filled & Regular
	- Password / 24 / Filled & Regular
	- Run / 16, 20, 24 / Filled & Regular

## V4.6.0

### Demo site, documentation and miscellaneous
- \[Demo & docs\] Add InputFile 'known issues' section ([#1680](https://github.com/microsoft/fluentui-blazor/pull/1680))
- \[Demo & docs\] Corrects spelling and grammar in templates page ([#1716](https://github.com/microsoft/fluentui-blazor/pull/1716))
- \[Demo & docs\] Fix Wizard link in navigation menu ([#1660](https://github.com/microsoft/fluentui-blazor/pull/1660))
- \[Demo & docs\] Minor corrections to CONTRIBUTING.md ([#1681](https://github.com/microsoft/fluentui-blazor/pull/1681))
- \[MarkdownSection\] Fixes border not showing in Markdown tables ([#1721](https://github.com/microsoft/fluentui-blazor/pull/1721))
- \[MarkdownSection\] Adds code highlighting ([#1737](https://github.com/microsoft/fluentui-blazor/pull/1737))
- \[MarkdownSection\] Fixes and enhancements ([#1751](https://github.com/microsoft/fluentui-blazor/pull/1751))
- \[Misc\] Add required .csproj settings for generating snupkg packages ([#1675](https://github.com/microsoft/fluentui-blazor/pull/1675))
- \[Misc\] Add ToColorHex extension to Swatch ([#1691](https://github.com/microsoft/fluentui-blazor/pull/1691))

### Components
- \[Accordion\] Add expanded value to custom event handler ([#1689](https://github.com/microsoft/fluentui-blazor/pull/1689))
- \[AppBarItem\] Add OnClick event callback ([#1698](https://github.com/microsoft/fluentui-blazor/pull/1698))
- \[Button\] Avoid padding on loading spinner when no text is shown ([#1714](https://github.com/microsoft/fluentui-blazor/pull/1714))
- \[InputFile\] Replace OnInitializedAsync with OnAfterRenderAsync ([#1661](https://github.com/microsoft/fluentui-blazor/pull/1661))
- \[KeyCode\] Allow content to avoid using the Anchor property ([#1743](https://github.com/microsoft/fluentui-blazor/pull/1743))
- **[KeyCodeProvider]** Add a global service to capture keystrokes ([#1740](https://github.com/microsoft/fluentui-blazor/pull/1740))
- \[MenuButton\] Make the menu anchored to the button so can float ([#1676](https://github.com/microsoft/fluentui-blazor/pull/1676))
- \[Pagination\] Add Disabled parameter ([#1713](https://github.com/microsoft/fluentui-blazor/pull/1713))
- \[Persona\] Manage the empty Name ([#1710](https://github.com/microsoft/fluentui-blazor/pull/1710))
- **[ProfileMenu]**  Add a new `FluentProfileMenu` ([#1705](https://github.com/microsoft/fluentui-blazor/pull/1705))
- **[PullToRefresh]** Add a new `FluentPullToRefresh` ([#1679](https://github.com/microsoft/fluentui-blazor/pull/1679))
- \[Wizard\] Add the ability to automatically validate an EditForm ([#1663](https://github.com/microsoft/fluentui-blazor/pull/1663))


- Update Fluent UI System Icons to 1.1.233

  **What's new (Name / Size(s) / Variant(s))**
	- Classification / 32 / Filled & Regular
	- Document Target / 20, 24, 32 / Filled & Regular
	- Emoji Meme / 16, 20, 24 / Filled & Regular
	- Hand Point / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Mail Read Briefcase / 48 / Filled & Regular
	- People Subtract / 20, 24, 32 / Filled & Regular
	- Person Alert Off / 16, 20, 24, 32 / Filled & Regular
	- Shopping Bag Add / 16 / Filled & Regular
	- Spatula Spoon / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Accessibility Error / 20, 24 / Filled & Regular
	- Accessibility Question Mark / 20, 24 / Filled & Regular
	- Arrow Down Exclamation / 24 / Filled & Regular
	- Arrow Sort Up Lines / 16, 20, 24 / Filled & Regular
	- Arrow Up Exclamation / 16, 20, 24 / Filled & Regular
	- Bench / 20, 24 / Filled & Regular
	- Building Lighthouse / 28 / Filled & Regular
	- Clock Bill / 16, 20, 24, 32 / Filled & Regular
	- Data Usage Settings / 16, 24 / Filled & Regular
	- Data Usage / 16 / Filled & Regular
	- Edit Person / 20, 24 / Filled & Regular
	- Highway / 20, 24 / Filled & Regular
	- Laptop Person / 20, 24, 48 / Filled & Regular
	- Location Ripple / 16, 20, 24 / Filled & Regular
	- Mail Arrow Double Back / 32 / Filled & Regular
	- Mail Briefcase / 48 / Filled & Regular
	- People Add / 32 / Filled & Regular
	- Person Alert / 32 / Filled & Regular
	- Road / 20, 24 / Filled & Regular
	- Save / 32 / Filled & Regular
	- Tab Desktop Multiple Sparkle / 16, 20, 24 / Filled & Regular
	- Tab Desktop Multiple / 24 / Filled & Regular
	- Vehicle Tractor / 20, 24 / Filled & Regular
 
  **What's updated (Name / Size(s) / Variant(s))**
	- Classification / 20, 24 / Filled & Regular
	- Emoji Add / 20 / Filled & Regular
	- Emoji Edit / 20 / Filled & Regular
	- Emoji Sparkle / 20 / Filled & Regular
	- Emoji / 20 / Filled & Regular
	- Accessibility Checkmark / 24 / Filled & Regular
	- Arrow Down Exclamation / 16, 20 / Filled & Regular
	- Arrow Sort Down Lines / 16, 20, 24 / Filled & Regular
	- Building Lighthouse / 48 / Filled & Regular
	- Calendar Video / 20, 24, 28 / Filled & Regular
	- Options / 16, 28, 32 / Filled & Regular
	- Person Alert / 16, 20, 24 / Filled & Regular
	- Tab Desktop Multiple Bottom / 24 / Filled 

## V4.5.0
From now on we will just list the PRs that have been merged. The related issues can be found by looking at the PR details on GitHub.

Titles have been altered and sorted here to provide a bit more uniformity.

### Demo site, documentation and miscellaneous
* [Demo & docs\] Add global search bar ([#1583](https://github.com/microsoft/fluentui-blazor/pull/1583))
* [Demo & docs\] Add video on Demo site ([#1586](https://github.com/microsoft/fluentui-blazor/pull/1586))
* [Demo & docs\] Migrate Demo Search to use FluentAutocomplete ([#1599](https://github.com/microsoft/fluentui-blazor/pull/1599))
* [Demo & docs\] Nuget badge links to nuget ([#1529](https://github.com/microsoft/fluentui-blazor/pull/1529))
* [Demo & docs\] Update NavMenuPage.razor - corrects grammar ([#1518](https://github.com/microsoft/fluentui-blazor/pull/1518))
* [Misc\] Rider files not ignored ([#1649](https://github.com/microsoft/fluentui-blazor/pull/1649))
* [Misc\] Update devcontainer to Dotnet 8 ([#1630](https://github.com/microsoft/fluentui-blazor/pull/1630))

### Components
* [AppBar\] Adding .NET Aspire's AppBar ([#1527](https://github.com/microsoft/fluentui-blazor/pull/1527))
* [Autocomplete\] Add IconDismiss and IconSearch ([#1573](https://github.com/microsoft/fluentui-blazor/pull/1573))
* [Autocomplete\] Add 'required' binding to the autocomplete label ([#1543](https://github.com/microsoft/fluentui-blazor/pull/1543))
* [Autocomplete\] Fix Backspace usage ([#1544](https://github.com/microsoft/fluentui-blazor/pull/1544))
* [Autocomplete\] Fix the left-right navigation (v3) ([#1491](https://github.com/microsoft/fluentui-blazor/pull/1491))
* [Autocomplete\] Add autofocus to Autocomplete & Combobox [#1650](https://github.com/microsoft/fluentui-blazor/pull/1650)
* [Autocomplete\] Add Virtualization [#1647](https://github.com/microsoft/fluentui-blazor/pull/1647)
* [Button\] Update the Button custom style for BackgroundColor and Color properties ([#1603](https://github.com/microsoft/fluentui-blazor/pull/1603))
* [Card\] Add MinimalStyle property ([#1595](https://github.com/microsoft/fluentui-blazor/pull/1595))
* [Combobox\] Allow FluentCombobox to be cleared from code ([#1613](https://github.com/microsoft/fluentui-blazor/pull/1613))
* [Combobox\] Fix 1485 by overriding SetParametersAsync ([#1506](https://github.com/microsoft/fluentui-blazor/pull/1506))
* [Combobox\] Re-use/re-purpose FluentTextField script for FluentCombobox (browser autocomplete) ([#1629](https://github.com/microsoft/fluentui-blazor/pull/1629))
* [DatagGrid\] Remove Expression from TooltipText ([#1635](https://github.com/microsoft/fluentui-blazor/pull/1635))
* [DataGrid\] Adds a Filtered property and visual indicator to PropretyColumn in FluentDataGrid ([#1625](https://github.com/microsoft/fluentui-blazor/pull/1625))
* [DataGrid\] Fix #1582 by adding pointercancel and pointerleave event listeners ([#1591](https://github.com/microsoft/fluentui-blazor/pull/1591))
* [DataGrid\] Fix #1616 by adding a try..catch block ([#1637](https://github.com/microsoft/fluentui-blazor/pull/1637))
* [DataGrid\] Fix Loading issues ([#1512](https://github.com/microsoft/fluentui-blazor/pull/1512))
* [DataGrid\] Fix OnRowwFocus and sorting for DataGrid ([#1577](https://github.com/microsoft/fluentui-blazor/pull/1577))
* [DataGrid\] Use specific ids for rows and cells ([#1480](https://github.com/microsoft/fluentui-blazor/pull/1480))
* [DatePicker\] Add DateOnly and TimeOnly extensions  ([#1500](https://github.com/microsoft/fluentui-blazor/pull/1500))
* [DatePicker\] Allow to select the existing selected month ([#1545](https://github.com/microsoft/fluentui-blazor/pull/1545))
* [DesignTheme\] Fix the Random color annoys other fillers ([#1475](https://github.com/microsoft/fluentui-blazor/pull/1475))
* [Dialog\] Allows showing a dialog by only providing a RenderFragment. ([#1496](https://github.com/microsoft/fluentui-blazor/pull/1496))
* [Grid\] Fix the Grid "external margins" ([#1646](https://github.com/microsoft/fluentui-blazor/pull/1646))
* [Icons\] Fix Icon color using `ColorWith` and a `CustomColor` attribute ([#1539](https://github.com/microsoft/fluentui-blazor/pull/1539))
* [Icons\] Update Fluent UI System Icons to v1.1.230 ([#1648](https://github.com/microsoft/fluentui-blazor/pull/1648))
* [Icons\] Update to Fluent UI System Icons 1.1.227 ([#1513](https://github.com/microsoft/fluentui-blazor/pull/1513))
* [Lists\] Small performance update for rendering list items ([#1476](https://github.com/microsoft/fluentui-blazor/pull/1476))
* [Menu\] Add Threshold attributes ([#1644](https://github.com/microsoft/fluentui-blazor/pull/1644))
* [Menu\] Several fixes ([#1574](https://github.com/microsoft/fluentui-blazor/pull/1574))
* [MenuButton\] Changes and additions ([#1602](https://github.com/microsoft/fluentui-blazor/pull/1602))
* [MessageBar\] Allow the ability to hide the dismissal button ([#1495](https://github.com/microsoft/fluentui-blazor/pull/1495))
* [NavMenu\] Fix FluentNavMenu.razor.js for non SSR ([#1560](https://github.com/microsoft/fluentui-blazor/pull/1560))
* [NavMenu\] Render out Id for FluentNavLink + add test ([#1580](https://github.com/microsoft/fluentui-blazor/pull/1580))
* [NumberField\] Fix #1531 by using web components current-value instead of value. ([#1576](https://github.com/microsoft/fluentui-blazor/pull/1576))
* [Overflow\] Better samples and better selector detection ([#1645](https://github.com/microsoft/fluentui-blazor/pull/1645))
* [Overflow\] Misc work ([#1523](https://github.com/microsoft/fluentui-blazor/pull/1523))
* [Pagination\] Fix #1596 (very long title, see description) ([#1606](https://github.com/microsoft/fluentui-blazor/pull/1606))
* [Paginator\] Remove background color ([#1503](https://github.com/microsoft/fluentui-blazor/pull/1503))
* [Persona\] Add support for icons ([#1546](https://github.com/microsoft/fluentui-blazor/pull/1546))
* [Persona\] Fix bug in FluentPersona when the names contains more than one space. ([#1623](https://github.com/microsoft/fluentui-blazor/pull/1623))
* [Script\] Fix #1652 by also checking for OperationCanceledException ([#1653](https://github.com/microsoft/fluentui-blazor/pull/1653))
* [SortableList\] Fix typo/spelling errors for Sortable List Page ([#1600](https://github.com/microsoft/fluentui-blazor/pull/1600))
* [SplashScreen\] Add WaitingMilliseconds and UpdateLabels ([#1570](https://github.com/microsoft/fluentui-blazor/pull/1570))
* [Splitter\] Fix adoptedStyleSheets is frozen in earlier versions by @CV-souryu ([#1557](https://github.com/microsoft/fluentui-blazor/pull/1557))
* [Splitter\] Update SplitPanels.ts ([#1520](https://github.com/microsoft/fluentui-blazor/pull/1520))
* [Tabs\] Add LoadingContent parameter to FluentTab ([#1587](https://github.com/microsoft/fluentui-blazor/pull/1587))
* [Templates\] Makes code created from fluentblazorwasm template neater ([#1501](https://github.com/microsoft/fluentui-blazor/pull/1501))
* [Templates\] Minor clean up ([#1488](https://github.com/microsoft/fluentui-blazor/pull/1488))
* [Tooltip\] Add the HideTooltipOnCursorLeave property ([#1571](https://github.com/microsoft/fluentui-blazor/pull/1571))
* [ValidationMessage\] Adds FieldIdentifier parameter ([#1489](https://github.com/microsoft/fluentui-blazor/pull/1489))
* [Wizard\] Fix ValueChanged never trigerred in FluentWizard ([#1538](https://github.com/microsoft/fluentui-blazor/pull/1538))


## V4.4.1
- Issue [#1462](https://github.com/microsoft/fluentui-blazor/issues/1462): FluentMessageBar onclick bug and not using Link?.Target
- Issue [#1461](https://github.com/microsoft/fluentui-blazor/issues/1461): fluent-data-grid rendered after rows in manual FluentDataGrid
- Issue [#1450](https://github.com/microsoft/fluentui-blazor/issues/1450): FluentSlider's two-way data binding on Value Property does not update appropriately
- Issue [#1449](https://github.com/microsoft/fluentui-blazor/issues/1449): [Autocomplete\] Fix Disabled item using keyboard and re-enables the ability to click on an element
- Issue [#1448](https://github.com/microsoft/fluentui-blazor/issues/1448): two key strokes required to change item in FluentSelect
- Issue [#1444](https://github.com/microsoft/fluentui-blazor/issues/1444): FluentDataGrid 4.4.0 not rendering in default fluentblazor template using StreamRendering and SSR
- Issue [#1442](https://github.com/microsoft/fluentui-blazor/issues/1442): Fix fluentblazor template 4.4.0
- Issue [#1437](https://github.com/microsoft/fluentui-blazor/issues/1437): UI does not leave Loading animation if result has 0 items in FluentDataGrid
- Issue [#1436](https://github.com/microsoft/fluentui-blazor/issues/1436): Profile NavMenu: Labels in Template
- Issue [#1433](https://github.com/microsoft/fluentui-blazor/issues/1433): FluentDataGrid show no data in version 4.4.0
- Issue [#1425](https://github.com/microsoft/fluentui-blazor/issues/1425): add OnClick EventCallback to FluentAnchor
- Issue [#1424](https://github.com/microsoft/fluentui-blazor/issues/1424): Not setting current-value for fluent-number-field

## V4.4.0
New: SortableList component
New: (experimental) MultiSplitter component
New: KeyCode component
Updated: DataGrid - Loading and LoadingContent parameters
- Issue [#1421](https://github.com/microsoft/fluentui-blazor/issues/1421): @code-block inside FluentDataGrid gets called three times instead of once 
- Issue [#1391](https://github.com/microsoft/fluentui-blazor/issues/1391): fix: External Authentication in Middleware
- Issue [#1358](https://github.com/microsoft/fluentui-blazor/issues/1358): fix: value not set in FluentSelect while using keyboard
- Issue [#1350](https://github.com/microsoft/fluentui-blazor/issues/1350): fix: "i.addEventListener is not a function" in "Microsoft.FluentUI.AspNetCore.Components.lib.module.js"
- Issue [#1344](https://github.com/microsoft/fluentui-blazor/issues/1344): fix: Keyboard navigation does not trigger value nor option change in Listbox 
- Issue [#1335](https://github.com/microsoft/fluentui-blazor/issues/1335): Consider removing the PageScript component from the public API (renamed to FluentPageScript)
- Issue [#1333](https://github.com/microsoft/fluentui-blazor/issues/1333): FluentSwitch inside FluentTab does not work anymore (since 4.3.0)
- Issue [#1328](https://github.com/microsoft/fluentui-blazor/issues/1328): feat: Allow SVG Icon viewBox to be set via configuration instead of reusing icon size feature
- Issue [#1327](https://github.com/microsoft/fluentui-blazor/issues/1327): fix: FluentLayout expenders stop working after UI updates
- Issue [#1317](https://github.com/microsoft/fluentui-blazor/issues/1317): feat: add loading indicator to DataGrid feature
- Issue [#1311](https://github.com/microsoft/fluentui-blazor/issues/1311): fix: Problems with validation border color / validation classes in FluentDatePicker / FluentAutocomplete
- Issue [#1292](https://github.com/microsoft/fluentui-blazor/issues/1292): Accessibility issue in Fluent AutoComplete
- Issue [#1255](https://github.com/microsoft/fluentui-blazor/issues/1255): fix: confirmation message box throws exception in production in Azure Static Web Apps
- Issue [#1182](https://github.com/microsoft/fluentui-blazor/issues/1182): FluentDesignTheme - Assertion failed - heap is currently locked when changing theme
- PR [#1426](https://github.com/microsoft/fluentui-blazor/pull/1426): Remove value and current-value for fluent-switch
- PR [#1424](https://github.com/microsoft/fluentui-blazor/pull/1424): Not setting current-value for fluent-number-field
- PR [#1404](https://github.com/microsoft/fluentui-blazor/pull/1404): [FluentFileInput\] adds a "disabled" property to the FluentFileInput component 
- PR [#1380](https://github.com/microsoft/fluentui-blazor/pull/1380): [Grid\] Fix breakpoints not working with fractional pixels
- PR [#1372](https://github.com/microsoft/fluentui-blazor/pull/1372): Lists related refactoring and maintenance 
- Demo: Tweak version in footer so it shows Git commit hash in truncated form
- Demo Fix positioning of hamburger menu in mobile view


- Update Fluent UI System Icons to 1.1.226
	**What's new (Name / Size(s) / Variant(s))**
	- Building Lighthouse / 24, 32, 48 / Filled & Regular
	- Calendar Link / 24, 28 / Filled & Regular
	- Calendar Video / 24, 28 / Filled & Regular
	- Cookies / 16, 28, 32, 48 / Filled & Regular
	- Hard Drive / 28, 48 / Filled & Regular
	- Laptop Settings / 20, 24, 32 / Filled & Regular
	- Laptop / 32 / Filled & Regular
	- People Audience / 32 / Filled & Regular
	- Shopping Bag Add / 20, 24 / Filled & Regular
	- Street Sign / 20, 24 / Filled & Regular
	- Video Link / 24, 28 / Filled & Regular

	**What's updated (Name / Size(s) / Variant(s))**
	- Cube / 12 / Filled & Regular
	- Laptop Multiple / 24 / Filled & Regular
	- Laptop / 24, 28 / Filled & Regular
	- Prohibited Multiple / 28 / Filled & Regular

- Update Fluent UI System Icons to 1.1.225

	**What's new (Name / Size(s) / Variant(s))**
	- Flowchart / 16, 32 / Filled & Regular
	- Layer Diagonal Person / 24 / Filled & Regular
	- Layer Diagonal / 24 / Filled & Regular
	- Poll Off / 16, 20, 24, 32 / Filled & Regular
	- Rectangle Landscape Sparkle / 48 / Filled & Regular
	- Rectangle Landscape Sync Off / 16, 20, 24, 28 / Filled & Regular
	- Rectangle Landscape Sync / 16, 20, 24, 28 / Filled & Regular
	- Seat Add / 16, 20, 24 / Filled & Regular
	- Seat / 16, 20, 24 / Filled & Regular
	- Speaker / 16, 20, 24 / Filled & Regular
	- Text Edit Style Character Ga / 32 / Filled & Regular
	- Window Ad / 24 / Filled & Regular
	- Wrench Settings / 20, 24 / Filled & Regular
	
	**What's updated (Name / Size(s) / Variant(s))**
	- Add Circle / 32 / Filled & Regular
	- Arrow Clockwise Dashes / 16, 20, 24, 32 / Filled & Regular
	- Arrow Clockwise / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Arrow Counterclockwise Dashes / 20, 24 / Filled & Regular
	- Arrow Counterclockwise / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Arrow Reply All / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Arrow Reply Down / 16, 20, 24 / Filled & Regular
	- Arrow Reply / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Calendar Clock / 24 / Filled & Regular
	- Calendar Info / 16 / Filled & Regular
	- Calendar Lock / 16 / Filled & Regular
	- Calendar Person / 16 / Filled & Regular
	- Calendar Phone / 16 / Filled & Regular
	- Calendar Question Mark / 16 / Filled & Regular
	- Calendar Reply / 16 / Filled & Regular
	- Calendar Search / 16 / Filled & Regular
	- Calendar Settings / 16 / Filled & Regular
	- Calendar Shield / 16 / Filled & Regular
	- Calendar Star / 16 / Filled & Regular
	- Calendar Sync / 16 / Filled & Regular
	- Chat Add / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Cloud Archive / 28, 48 / Filled & Regular
	- Cloud Arrow Down / 28, 48 / Filled & Regular
	- Cloud Arrow Up / 28, 48 / Filled & Regular
	- Cloud Beaker / 28, 48 / Filled & Regular
	- Cloud Checkmark / 28, 48 / Filled & Regular
	- Cloud Cube / 28, 48 / Filled & Regular
	- Cloud Dismiss / 28, 48 / Filled & Regular
	- Cloud Error / 28, 48 / Filled & Regular
	- Cloud Off / 28, 48 / Filled & Regular
	- Cloud Sync / 28, 48 / Filled & Regular
	- Cloud Words / 28, 48 / Filled & Regular
	- Cloud / 28, 48 / Filled & Regular
	- Comment Multiple Checkmark / 16, 20, 24, 28 / Filled & Regular
	- Comment Multiple Link / 16, 20, 24, 28, 32 / Filled & Regular
	- Comment Multiple / 16, 20, 24, 28, 32 / Filled & Regular
	- Credit Card Clock / 24 / Filled & Regular
	- Document Search / 20, 24 / Filled & Regular
	- Document Text Clock / 24 / Filled & Regular
	- Flag Clock / 20, 24 / Filled & Regular
	- Flag Off / 20 / Filled & Regular
	- Flag / 20 / Filled & Regular
	- Globe Clock / 24 / Filled & Regular
	- History Dismiss / 20, 24, 28, 32, 48 / Filled & Regular
	- History / 16, 20, 24, 28, 32, 48 / Filled & Regular
	- Lock Closed Key / 16, 20, 24 / Filled & Regular
	- Mail Clock / 24 / Filled & Regular
	- Person Clock / 24 / Filled & Regular
	- Prohibited Multiple / 16, 20, 24 / Filled & Regular
	- Rectangle Landscape Sparkle / 16, 20, 24, 28, 32 / Filled & Regular
	- Scan Person / 16, 20, 24, 28, 48 / Filled & Regular
	- Search Info / 24 / Filled & Regular
	- Search / 24, 28 / Filled & Regular
	- Send Clock / 24 / Filled & Regular
	- Text Edit Style Character A / 32 / Filled
	- Vehicle Car Profile LTR Clock / 24 / Filled & Regular
	- Video Person Clock / 24 / Filled & Regular
	- Video Person Sparkle Off / 20, 24 / Filled & Regular
	- Video Person Sparkle / 16, 20, 24, 28, 48 / Filled & Regular
	- Window Ad Off / 20 / Filled
	- Window Ad / 20 / Filled
	- Window Console / 20 / Filled
	- Window Dev Edit / 16, 20 / Filled & Regular
	- Window Dev Tools / 16, 20, 24 / Filled & Regular
	- Window Edit / 16 / Filled & Regular
	- Window Shield / 16, 20, 24 / Filled & Regular
	- Wrench / 24 / Filled & Regular

## V4.3.1
- Issue [#1282](https://github.com/microsoft/fluentui-blazor/issues/1282): Looping behaviour after update to 4.3.0 / FluentDesignTheme
- Issue [#1283](https://github.com/microsoft/fluentui-blazor/issues/1283): Fix Combobox and Select width property
- Issue [#1294](https://github.com/microsoft/fluentui-blazor/issues/1294): Issue upgrading from 4.2.1 to 4.3.0; better fix for #1205 en #1225
- Issue [#1305](https://github.com/microsoft/fluentui-blazor/issues/1305): ComboBox component resetting bound object to null when custom content is entered
- Issue [#1314](https://github.com/microsoft/fluentui-blazor/issues/1314): Updating FluentTabs ActiveTabId doesn't rerender if data is entered in contained FluentTextInput

- \[Splitter\] Add parameter to show/hide bar handle
- \[Tabs\] Add GotToTabAsync method

## V4.3
- New: FluentWizard component

- Issue [#1116](https://github.com/microsoft/fluentui-blazor/issues/1116): Create FieldIdentifier when no ValueExpression set
- Issue [#1121](https://github.com/microsoft/fluentui-blazor/issues/1121): FluentProgress - Add Width, Stroke, Color and BackgroundColor attributes
- Issue [#1125](https://github.com/microsoft/fluentui-blazor/issues/1125): FluentProgress - Update Stroke enumeration
- Issue [#1132](https://github.com/microsoft/fluentui-blazor/issues/1132): Some FluentTab changes
- Issue [#1138](https://github.com/microsoft/fluentui-blazor/issues/1138): Fix: Do not render percent sign for indeterminate ProgressToast
- Issue [#1140](https://github.com/microsoft/fluentui-blazor/issues/1140): FluentIcon - Add Unit Tests
- Issue [#1141](https://github.com/microsoft/fluentui-blazor/issues/1141): Update initializersLoader.webview.js
- Issue [#1144](https://github.com/microsoft/fluentui-blazor/issues/1144): List components - Each item must be instantiated (cannot be null)
- Issue [#1146](https://github.com/microsoft/fluentui-blazor/issues/1146): FluentButton Loading - Fix button when style is applied
- Issue [#1149](https://github.com/microsoft/fluentui-blazor/issues/1149): ListComponentBase - maintain consistency between SelectedOption and Value
- Issue [#1155](https://github.com/microsoft/fluentui-blazor/issues/1155): Use GlobalState, use LocalizationDirection, less var’s
- Issue [#1156](https://github.com/microsoft/fluentui-blazor/issues/1156): FluentDesignTheme - Add OnLoaded event
- Issue [#1157](https://github.com/microsoft/fluentui-blazor/issues/1157): FluentDesignTheme - Check if LocalStorage is available
- Issue [#1158](https://github.com/microsoft/fluentui-blazor/issues/1158): Make NavGroup work with enhanced navigation and SSR
- Issue [#1161](https://github.com/microsoft/fluentui-blazor/issues/1161): Create separate ts file for PageScript Add SSR project from template for validation purposes (other projects githubbe added later)
- Issue [#1163](https://github.com/microsoft/fluentui-blazor/issues/1163): FluentDesignTheme - Storage color overload correction
- Issue [#1165](https://github.com/microsoft/fluentui-blazor/issues/1165): Finih implementation of NavMenu SSR support
- Issue [#1168](https://github.com/microsoft/fluentui-blazor/issues/1168): Tabs tablist will forever expand despite the tab containers width
- Issue [#1169](https://github.com/microsoft/fluentui-blazor/issues/1169): Fix: Matched FluentNavLink renders with background that does not have rounded corners
- Issue [#1172](https://github.com/microsoft/fluentui-blazor/issues/1172): FluentOverflow - Resize when a new element is included or excluded
- Issue [#1173](https://github.com/microsoft/fluentui-blazor/issues/1173): Replace type="image/png" with type="image/x-icon"in templates
- Issue [#1174](https://github.com/microsoft/fluentui-blazor/issues/1174): Forgot to replace svg with icon in templates
- Issue [#1177](https://github.com/microsoft/fluentui-blazor/issues/1177): Docs: fix outdated w3.org link
- Issue [#1184](https://github.com/microsoft/fluentui-blazor/issues/1184): Docs: fix typo
- Issue [#1185](https://github.com/microsoft/fluentui-blazor/issues/1185): Fix: Collapse button doesn’t work correctly in Fluent Blazor Web App template
- Issue [#1189](https://github.com/microsoft/fluentui-blazor/issues/1189): fix Combobox z-index stack order issue
- Issue [#1191](https://github.com/microsoft/fluentui-blazor/issues/1191): Fix #1185: NavMenu Collapse button behavior in SRR only woks once
- Issue [#1194](https://github.com/microsoft/fluentui-blazor/issues/1194): Fix: FluentTextField inside FluentTabs, pressing Enter re-renders FluentTab’s content
- Issue [#1205](https://github.com/microsoft/fluentui-blazor/issues/1205): FluentButton submit does not work outside the EditForm
- Issue [#1211](https://github.com/microsoft/fluentui-blazor/issues/1211): FluentWizard - New component
- Issue [#1214](https://github.com/microsoft/fluentui-blazor/issues/1214): FluentSliderLabel - Update the sub-label MaxWidth style
- Issue [#1215](https://github.com/microsoft/fluentui-blazor/issues/1215): FluentTextField ignores DataList property
- Issue [#1223](https://github.com/microsoft/fluentui-blazor/issues/1223): DialogService - Add a fake instance of DialogEventArgs
- Issue [#1225](https://github.com/microsoft/fluentui-blazor/issues/1225): Fix #1205 FluentButton submit does not work outside the EditForm
- Issue [#1227](https://github.com/microsoft/fluentui-blazor/issues/1227): Fix: FluentTreeItem in that span that renders Text proprty is not conditional and adds unnecessary space when gap githubproperty is applied
- Issue [#1226](https://github.com/microsoft/fluentui-blazor/issues/1226): FluentAutocomplete does not have an ElementReference
- Issue [#1229](https://github.com/microsoft/fluentui-blazor/issues/1229): Add NavMenuWidth parameter to FluentMainLayout
- Issue [#1232](https://github.com/microsoft/fluentui-blazor/issues/1232): Several work items combined in a PR to not push to dev directly
- Issue [#1233](https://github.com/microsoft/fluentui-blazor/issues/1233): PageScript - Move script visibility inside the web component
- Issue [#1234](https://github.com/microsoft/fluentui-blazor/issues/1234): Templates - Fix spelling and use FluentValidationSummary
- Issue [#1241](https://github.com/microsoft/fluentui-blazor/issues/1241): Fix: FluentNavLink OnClick not working
- Issue [#1245](https://github.com/microsoft/fluentui-blazor/issues/1245): Fix: dragging the splitter bar is inverted with RTL enabled in fluent splitter
- Issue [#1250](https://github.com/microsoft/fluentui-blazor/issues/1250): Fix: Pagination arrows should reverse in RTL mode
- Issue [#1268](https://github.com/microsoft/fluentui-blazor/issues/1268): Feat: add "invalid" css-class to/in Default Date Picker field if invalid
- Issue [#1274](https://github.com/microsoft/fluentui-blazor/issues/1274): Fix: [Width less than 250px does not take effect\] in [FluentCombobox and FluentSelect]
- Issue [#1276](https://github.com/microsoft/fluentui-blazor/issues/1276): FluentTabs shows badge although all tabs are visible
- \[DialogService\] Fix trimming on production error 
- Demo site: menu structure overhaul
- Add solution with Template generated projects

### Known issues
- FluentDesignTheme: `Assertion failed` error in console. See [#1182](https://github.com/microsoft/fluentui-blazor/issues/1182) for more information. We are still working on a fix.
- Confirmation message box (and possibly other dialogs) are not working correctly in WebAssembly trimmed projects. See [#1255](https://github.com/microsoft/fluentui-blazor/issues/1255) for more information. 

## V4.2.1
- NavMenu and NevMenuGroups can now expand/collpase in SSR mode
- Added PageScript component (See [Static Server Rendeing on MS Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/static-server-rendering?view=aspnetcore-8.0))
- Move `initializersLoader.webview.js` back into `wwwroot/js` so it gets published in the package again.
- Updates and fixes for FluentDesignTheme: add `OnLoaded`, check if LocalStorage is available 
- `FluentProgress`: Add `Width`, `Stroke`, `Color` and `BackgroundColor` attributes
- `FluentTab`: Add `Visible` parameter 
- Fix [#1160](https://github.com/microsoft/fluentui-blazor/issues/1160): FluentDesignTheme custom color not honored and assertion failure.
- Fix [#1116](https://github.com/microsoft/fluentui-blazor/issues/1116): Create FieldIdentifier when no ValueExpression set 
- Fix [#1138](https://github.com/microsoft/fluentui-blazor/issues/1138): Do not render percent sign for indeterminate ProgressToast
- Fix [#1144](https://github.com/microsoft/fluentui-blazor/issues/1144): [List components\] Each item must be instantiated (cannot be null).
- Fix [#1146](https://github.com/microsoft/fluentui-blazor/issues/1146): Loading button with styles issue
- Fix [#1149](https://github.com/microsoft/fluentui-blazor/issues/1149): [List components\] maintain consistency between SelectedOption and Value when Multiple is false
- Demo site: Search for icons in all sizes

## V4.2.0
- New: FluentDesignTheme - An easy-to-use component to set a theme and accent color.
- New: FluentValidationMessage and Required indicator for input components
- JavaScript tools project system integration 
- Enhanced: NavMenu
- Enhanced: InputFile
- Enhanced: List components
- Enhanced: Grid: Hiding elements and `OnBreakpointEnter` event callback
- Enhanced: Checkbox
- Enhanced: Splitter
- Updated Templates: responsive NavMenu and other improvements
- Fix [#1057](https://github.com/microsoft/fluentui-blazor/issues/1057): Can't show Fluent UI Blazor dialog on published Blazor webasm app
- Fix [#1070](https://github.com/microsoft/fluentui-blazor/issues/1070): Style not applied for FluentPaginator + added `PaginationTextTemplate` parameter 
- Update Fluent UI System Icons to 1.1.224:

- **What's new (Name / Size(s) / Variant(s))**
- Arrow Clockwise Dashes / 16, 32 / Filled & Regular
- Building Swap / 16, 20, 24, 32, 48 / Filled & Regular
- Certificate / 32 / Filled & Regular
- Clipboard Brush / 16, 20, 24, 28, 32 / Filled & Regular
- Cloud Beaker / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Cloud Cube / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Contract Up Right / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Document Data Lock / 16, 20, 24, 32 / Filled & Regular
- Glance Horizontal Sparkles / 20 / Filled & Regular
- Layout Cell Four Focus Bottom Left / 16, 20, 24 / Filled
- Layout Cell Four Focus Bottom Right / 16, 20, 24 / Filled
- Layout Cell Four Focus Top Left / 16, 20, 24 / Filled
- Layout Cell Four Focus Top Right / 16, 20, 24 / Filled
- Layout Cell Four / 16, 20, 24 / Filled & Regular
- Layout Column Four Focus Center Left / 16, 20, 24 / Filled
- Layout Column Four Focus Center Right / 16, 20, 24 / Filled
- Layout Column Four Focus Left / 16, 20, 24 / Filled
- Layout Column Four Focus Right / 16, 20, 24 / Filled
- Layout Column Four / 16, 20, 24 / Filled & Regular
- Layout Column One Third Left / 16, 20, 24 / Filled & Regular
- Layout Column One Third Right Hint / 16, 20, 24 / Filled & Regular
- Layout Column One Third Right / 16, 20, 24 / Filled & Regular
- Layout Column Three Focus Center / 16, 20, 24 / Filled
- Layout Column Three Focus Left / 16, 20, 24 / Filled
- Layout Column Three Focus Right / 16, 20, 24 / Filled
- Layout Column Three / 16, 20, 24 / Filled & Regular
- Layout Column Two Focus Left / 16, 20, 24 / Filled
- Layout Column Two Focus Right / 16, 20, 24 / Filled
- Layout Column Two Split Left Focus Bottom Left / 16, 20, 24 / Filled
- Layout Column Two Split Left Focus Right / 16, 20, 24 / Filled
- Layout Column Two Split Left Focus Top Left / 16, 20, 24 / Filled
- Layout Column Two Split Left / 16, 20, 24 / Filled & Regular
- Layout Column Two Split Right Focus Bottom Right / 16, 20, 24 / Filled
- Layout Column Two Split Right Focus Left / 16, 20, 24 / Filled
- Layout Column Two Split Right Focus Top Right / 16, 20, 24 / Filled
- Layout Column Two Split Right / 16, 20, 24 / Filled & Regular
- Layout Column Two / 16, 20, 24 / Filled & Regular
- Layout Row Four Focus Bottom / 16, 20, 24 / Filled
- Layout Row Four Focus Center Bottom / 16, 20, 24 / Filled
- Layout Row Four Focus Center Top / 16, 20, 24 / Filled
- Layout Row Four Focus Top / 16, 20, 24 / Filled
- Layout Row Four / 16, 20, 24 / Filled & Regular
- Layout Row Three Focus Bottom / 16, 20, 24 / Filled
- Layout Row Three Focus Center / 16, 20, 24 / Filled
- Layout Row Three Focus Top / 16, 20, 24 / Filled
- Layout Row Three / 16, 20, 24 / Filled & Regular
- Layout Row Two Focus Bottom / 16, 20, 24 / Filled
- Layout Row Two Focus Top / 16, 20, 24 / Filled
- Layout Row Two Split Bottom Focus Bottom Left / 16, 20, 24 / Filled
- Layout Row Two Split Bottom Focus Bottom Right / 16, 20, 24 / Filled
- Layout Row Two Split Bottom Focus Top / 16, 20, 24 / Filled
- Layout Row Two Split Bottom / 16, 20, 24 / Filled & Regular
- Layout Row Two Split Top Focus Bottom / 16, 20, 24 / Filled
- Layout Row Two Split Top Focus Top Left / 16, 20, 24 / Filled
- Layout Row Two Split Top Focus Top Right / 16, 20, 24 / Filled
- Layout Row Two Split Top / 16, 20, 24 / Filled & Regular
- Layout Row Two / 16, 20, 24 / Filled & Regular
- Location Target Square / 16, 20, 24, 32 / Filled & Regular
- Resize / 16, 28, 32, 48 / Filled & Regular
- Select All Off / 16 / Filled & Regular
- Select All On / 16 / Filled & Regular
- Share Android / 16, 32 / Filled & Regular
- Text Arrow Down Right Column / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Text Effects Sparkle / 20, 24 / Filled & Regular
- Whiteboard Off / 16, 20, 24 / Filled & Regular
- Whiteboard / 16 / Filled & Regular
 
**What's updated (Name / Size(s) / Variant(s))**
- Contract Down Left / 28 / Filled & Regular
- Resize / 20, 24 / Filled & Regular
- Select All Off / 20, 24 / Filled & Regular
- Select All On / 20, 24 / Filled & Regular


## V4.1.1
- Fix [#939](https://github.com/microsoft/fluentui-blazor/issues/939): Add `OptionTemplate` for `FluentCombobox`, `FluentListBox` and `FluentSelect` 
- Fix [#1040](https://github.com/microsoft/fluentui-blazor/issues/1040): `FluentNumberField` can be changed when `ReadOnly`
- FluentNavMenu: Add `Tooltip` parameter to `FluentNavMenuGroup` (fallback to `Title`) and `FluentNavLink`
- FluentNavMenu: Expand the menu when collapsed and a navitem is clicked 
- FluentInputFile: Add ProgressTemplate and a bindable ProgressPercent attribute

## V4.1.0
- FluentCalendar: Add new views to select month/year 
- FluentCheckbox: Add tri-state support
- FluentAccordionItem: Add HeaderTemplate parameter
- FluentSplitter: Add Panel1MinSize, Panel2MinSize and BarSize parameter
- FluentSplitter: Make resizing always use proportional values
- FluentSplitter: Add support for collapsing panel 2 (right/bottom), add `OnCollapsed`, `OnExpanded` and  `OnResized` event callbacks
- Fix using checkbox, switch, slider being used in `EditForm` not getting set on first few clicks
- Fix Web App template to correctly handle NavMenu based on rendermode and interactivity choices
- Move CacheStorageAccessor and StaticAssetService to Demo.Shared project as these are specific to the demo site and not the library
- Update Fluent UI System Icons to 1.1.223 (no overview of what's new/changed this time)
- Demo: Move ErrorBoundary into DemoSection component so page keeps working when only one section fails
- Fix miscellaneous demo errors caused by trimming

## V4.0.0
- FluentAccordionItem: Add `HeaderTemplate` parameter

## V4.0.0-rc.3
- Undo change template interactivity default to None. Default is now Server (same as regular Blazor template)
- FluentSplitter: Add support for collapsing panel 2 (right/bottom), add `OnCollapsed`, `OnExpanded` and  `OnResized` event callbacks
- `variables.css` is imported in the automatically loaded library's css now

## V4.0.0-rc.2
- Fix initial Highlight style after refresh was not right
- Fix [#918](https://github.com/microsoft/fluentui-blazor/issues/918): .NET 8 initializers + cleanup
- Fix [#923](https://github.com/microsoft/fluentui-blazor/issues/923): WebAssembly template errors
- Fix [#924](https://github.com/microsoft/fluentui-blazor/issues/924): Call SetLuminance after theme change
- Fix [#925](https://github.com/microsoft/fluentui-blazor/issues/925): Fic W3C links in demo site
- Fix [#926](https://github.com/microsoft/fluentui-blazor/issues/926): Work on Dialog height
- Remove Card from Home page in the WebAssembly template
- Change template interactivity default to None (ie SSR mode)
- Adjust FluentNavMenu to work with interactivity change
- Add favicon to demo sites and templates
- Move CacheStorageAccessor and StaticAssetService to Demo.Shared project as these are specific to the demo site and not the library
- Fix WhatsNew archive

## V4.0.0-rc.1
- This version is for .NET 8 **only**. (use `Microsoft.Fast.Components.FluentUI` when using .NET 6 or 7)
- Update all input component to use `ReadOnly` instead of a mix of possible spellings
- Update demo site styling (nav menu and body now scroll independently)
- Support DisabledDateFunc in FluentDatePicker 
- Removed the FluentCodeEditor component because it lacked features and has no Fluent design aspects. You can use the [BlazorMonaco](https://github.com/serdarciplak/BlazorMonaco) component as a replacement
- Fix [#911](https://github.com/microsoft/fluentui-blazor/issues/911): Column resizing in DataGrid breaks when column to narrow
- Fix [#891](https://github.com/microsoft/fluentui-blazor/issues/891): JSDisconnectedException in FluentOverflow
- Fix [#861](https://github.com/microsoft/fluentui-blazor/issues/861): Needed extra `empty-content-cell` class 
- FluentCard: Add Width and Height + docs/examples
- FluentToast: Timeout is now in milliseconds
- FluentToastContainer renamed to FluentToastProvider
- FluentMessageBarContainer renamed to FluentMessageBarProvider

## V4.0.0-preview.2
- Functionally equivalent to v3.2.2
- Replace `AfterBindValue` with native `@bind-Value:after`
- Fix missed or incorrectly replaced namespace errors 
- Update Fluent UI System icons to 1.1.221

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

## V4.0.0-preview.1
- Change root namespace to `Microsoft.FluentUI.AspNetCore.Components`
