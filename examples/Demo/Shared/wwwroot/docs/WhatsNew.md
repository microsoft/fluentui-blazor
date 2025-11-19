## V4.13.2

### General
- [Chore] Use NET 10 GA SDK in ([#4279](https://github.com/microsoft/fluentui-blazor/pull/4279))
- [Chore] Fix CodeQL in ([#4315](https://github.com/microsoft/fluentui-blazor/pull/4315))
- [Chore] Configure Dependabot settings in ([#4320](https://github.com/microsoft/fluentui-blazor/pull/4320))
- [Templates] Fix .net10 web app rename issue in ([#4310](https://github.com/microsoft/fluentui-blazor/pull/4310))
- [Templates] Fix Client projects in Web App templates in ([#4328](https://github.com/microsoft/fluentui-blazor/pull/4328))
- [Templates] Adjust _Imports include condition in ([#4329](https://github.com/microsoft/fluentui-blazor/pull/4329))

### Components
- [AccordionItem] Add HeadingTooltip parameter in ([#4306](https://github.com/microsoft/fluentui-blazor/pull/4306))
- [Dialog] Fix error when module hasn't finished loading in ([#4324](https://github.com/microsoft/fluentui-blazor/pull/4324))
- [Grid] Use more presice values for width percentages in ([#4299](https://github.com/microsoft/fluentui-blazor/pull/4299))
- [SortableList] Handle JS errors in a better way in ([#4286](https://github.com/microsoft/fluentui-blazor/pull/4286))

### Icons and Emoji
- Update to Fluent UI System Icons 1.1.315
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.

--------------

## V4.13.1

### General
- \[General\] Fix ADO pipelines ([#4227](https://github.com/microsoft/fluentui-blazor/pull/4227))  
- \[Templates\] Fix .NET 10 Web App template ([#4242](https://github.com/microsoft/fluentui-blazor/pull/4242))  

### Components
- \[AutoComplete\] Override FocusAsync in FluentAutoComplete ([#4230](https://github.com/microsoft/fluentui-blazor/pull/4230))  
- \[Combobox\] Fix presetting option ([#4255](https://github.com/microsoft/fluentui-blazor/pull/4255))  
- \[Menu\] Add null checks for modules on DisposeAsync ([#4249](https://github.com/microsoft/fluentui-blazor/pull/4249))  
- \[Menu\] Fix dispose error ([#4256](https://github.com/microsoft/fluentui-blazor/pull/4256))  
- \[Menu\] More fixes for dispose error ([#4258](https://github.com/microsoft/fluentui-blazor/pull/4258))  

### Icons and Emoji
- Update to Fluent UI System Icons 1.1.311
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.

-------------
## V4.13.0

### General
- \[General\] Add ReDoS protection for regex matching and nit fixes. ([#4124](https://github.com/microsoft/fluentui-blazor/pull/4124))
- \[General\] Re-add net10.0 TFM and update to latest SDKs/NuGet packages ([#4141](https://github.com/microsoft/fluentui-blazor/pull/4141))
- \[General\] Update GH Actions ([#4016](https://github.com/microsoft/fluentui-blazor/pull/4016))
- \[General\] Workflow maintenance ([#4130](https://github.com/microsoft/fluentui-blazor/pull/4130))
- \[General\] Add es metadata ([#4131](https://github.com/microsoft/fluentui-blazor/pull/4131))
- \[General\] Add PDB config ([#4079](https://github.com/microsoft/fluentui-blazor/pull/4079))
- \[General\] CodeQL - Update .NET SDK ([#4199](https://github.com/microsoft/fluentui-blazor/pull/4199))
- \[General\] Pipeline - Add PublishSymbols ([#4076](https://github.com/microsoft/fluentui-blazor/pull/4076))
- \[General\] Update codeql-analysis.yml ([#4075](https://github.com/microsoft/fluentui-blazor/pull/4075))
- \[General\] Update README.md ([#4065](https://github.com/microsoft/fluentui-blazor/pull/4065))
- \[General\] Update VSCode settings and add demo server configuration ([#4115](https://github.com/microsoft/fluentui-blazor/pull/4115))

### Components
- \[AppBar\] Add TryGetValue and Apps.ContainsKey ([#4146](https://github.com/microsoft/fluentui-blazor/pull/4146))
- \[Autocomplete\] Add a ShowProgressIndicator parameter + Breaking change ([#4042](https://github.com/microsoft/fluentui-blazor/pull/4042))
- \[AutoComplete\] Invalid aria-controls reference in Autocomplete popup rendering ([#4117](https://github.com/microsoft/fluentui-blazor/pull/4117))
- \[AutoComplete\] Override FocusAsync in FluentAutoComplete ([#4230](https://github.com/microsoft/fluentui-blazor/pull/4230))
- \[DataGrid\] Add MinWidth parameter to ColumnBase  ([#4112](https://github.com/microsoft/fluentui-blazor/pull/4112))
- \[DataGrid\] Add public Columns property to DataGridRow and Column property to DataGridCell for easier column access ([#4036](https://github.com/microsoft/fluentui-blazor/pull/4036))
- \[DataGrid\] Asynchronous IQueryable based loading and error handling UI feedback ([#4177](https://github.com/microsoft/fluentui-blazor/pull/4177))
- \[DataGrid\] Fix invalid optimization when using Virtualize icw ItemsProvider ([#4172](https://github.com/microsoft/fluentui-blazor/pull/4172))
- \[DataGrid\] Fix potential RTL error ([#4048](https://github.com/microsoft/fluentui-blazor/pull/4048))
- \[DataGrid\] Make resize menu work in 'Table' mode ([#4116](https://github.com/microsoft/fluentui-blazor/pull/4116))
- \[DataGrid\] Optimize style for rendering cell ([#4178](https://github.com/microsoft/fluentui-blazor/pull/4178))
- \[DatePicker & TimePicker\] Fix FocusAsync throwing null exception ([#4167](https://github.com/microsoft/fluentui-blazor/pull/4167))
- \[DatePicker\] Allow to prevent scrolling on focus ([#4168](https://github.com/microsoft/fluentui-blazor/pull/4168))
- \[DesignTheme\] Only write data-theme when mode is changed ([#4105](https://github.com/microsoft/fluentui-blazor/pull/4105))
- \[Dialog\] Add way to specify header title typography ([#4132](https://github.com/microsoft/fluentui-blazor/pull/4132))
- \[Dialog\] Return focus when dialog is closed to element focused before open ([#4095](https://github.com/microsoft/fluentui-blazor/pull/4095))
- \[Drag&Drop\] Add stopPropagation to DropZone ([#4045](https://github.com/microsoft/fluentui-blazor/pull/4045))
- \[FluentKeyCode\] Fix DisposeAsync issue in MAUI ([#4088](https://github.com/microsoft/fluentui-blazor/pull/4088))
- \[Inputs\] Fix wrong active size for mobile devices ([#4122](https://github.com/microsoft/fluentui-blazor/pull/4122))
- \[Internal\] Update CodeQL ([#4210](https://github.com/microsoft/fluentui-blazor/pull/4210))
- \[List\] Add an `OptionTitle` attribute ([#4147](https://github.com/microsoft/fluentui-blazor/pull/4147))
- \[List\] Fix for default selection options ([#4135](https://github.com/microsoft/fluentui-blazor/pull/4135))
- \[MenuButton\] Accessibility fixes and allow custom button content ([#4093](https://github.com/microsoft/fluentui-blazor/pull/4093))
- \[MenuButton\] Add `UseMenuService` parameter to configure Menu ([#4219](https://github.com/microsoft/fluentui-blazor/pull/4219))
- \[MenuButton\] Finalize work on a11y fixes and custom button content ([#4221](https://github.com/microsoft/fluentui-blazor/pull/4221))
- \[MenuButton\] Pass through Class ([#4038](https://github.com/microsoft/fluentui-blazor/pull/4038))
- \[MenuItem\] trigger OnClick on FluentMenuItem when enter is pressed onkeydown ([#4032](https://github.com/microsoft/fluentui-blazor/pull/4032))
- \[NavMenu\] Fix display of submenu in collapsed state ([#4171](https://github.com/microsoft/fluentui-blazor/pull/4171))
- \[NumberField\] Make it work with data-enhance in SSR mode ([#4181](https://github.com/microsoft/fluentui-blazor/pull/4181))
- \[Paginator\] Fix for not applying custom classes ([#4207](https://github.com/microsoft/fluentui-blazor/pull/4207))
- \[ProfileMenu\] Toggle open on enter/space key press ([#4104](https://github.com/microsoft/fluentui-blazor/pull/4104))
- \[Property Column\] Remove boxing when formatting the property. ([#4070](https://github.com/microsoft/fluentui-blazor/pull/4070))
- \[Templates\] Updates and add .NET 10 versions ([#4025](https://github.com/microsoft/fluentui-blazor/pull/4025))
- \[Toast\] Add Class & Style properties ([#4170](https://github.com/microsoft/fluentui-blazor/pull/4170))
- \[Toast\] Change DynamicDependency  ([#4106](https://github.com/microsoft/fluentui-blazor/pull/4106))

### Demo site and documentation
- \[Docs\] Add documentation for nested drag & drop containers ([#4050](https://github.com/microsoft/fluentui-blazor/pull/4050))
- \[Docs\] Add extra info for list selection. ([#4192](https://github.com/microsoft/fluentui-blazor/pull/4192))
- \[Docs\] Update NodeJS version and fix note ([#4083](https://github.com/microsoft/fluentui-blazor/pull/4083))
- \[Docs\] Update README.md ([#4021](https://github.com/microsoft/fluentui-blazor/pull/4021))
- \[Docs\] Use https instead of http for GitHub link ([#4121](https://github.com/microsoft/fluentui-blazor/pull/4121))


### Icons and Emoji
- \[Icons\] Allow GetInstance to find icons regardless of casing ([#4179](https://github.com/microsoft/fluentui-blazor/pull/4179))
- Update to Fluent UI System Icons 1.1.311
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.
--------------
## V4.12.1

### General
- \[General\] Add file header to all .cs files ([#3946](https://github.com/microsoft/fluentui-blazor/pull/3946))
- \[General\] Update the issue template ([#3995](https://github.com/microsoft/fluentui-blazor/pull/3995))
- \[General\] Update to latest SDKs and NuGet packages ([#4006](https://github.com/microsoft/fluentui-blazor/pull/4006))

### Components
- \[Autocomplete\] Fix keyboard usage in Single mode ([#3930](https://github.com/microsoft/fluentui-blazor/pull/3930))
- \[AutoComplete\] Only render placeholder attribute when the parameter is provided ([#3919](https://github.com/microsoft/fluentui-blazor/pull/3919))
- \[DataGrid\] Add IsFixed parameter ([#3927](https://github.com/microsoft/fluentui-blazor/pull/3927))
- \[DataGrid\] Add parameter to configure full column resize ([#3903](https://github.com/microsoft/fluentui-blazor/pull/3903))
- \[DataGrid\] Fix header not being sticky in certain situations ([#3890](https://github.com/microsoft/fluentui-blazor/pull/3890))
- \[DataGrid\] Make SelectColumn respond to programmatic changes in grid data ([#3949](https://github.com/microsoft/fluentui-blazor/pull/3949))
- \[DataGrid\] Set correct initial height for resize indicator ([#3965](https://github.com/microsoft/fluentui-blazor/pull/3965))
- \[MAUI\] Add DynamicDependency to DesignToken constructor ([#4005](https://github.com/microsoft/fluentui-blazor/pull/4005))
- \[NavLink\] Track active state for menu items with an OnClick handler ([#3999](https://github.com/microsoft/fluentui-blazor/pull/3999))
- \[NavLink\] Fix keyboard navigation not working for FluentNavLink inside a FluentNavGroup ([#3998](https://github.com/microsoft/fluentui-blazor/pull/3998))
- \[Select\] Use correct CSS variable for placeholder color ([#3941](https://github.com/microsoft/fluentui-blazor/pull/3941))
- \[SortableList\] Extend sortable list parameters ([#3891](https://github.com/microsoft/fluentui-blazor/pull/3891))
- \[Tabs\] Implement proper tab sizing ([#4002](https://github.com/microsoft/fluentui-blazor/pull/4002))
- \[Templates\] Updates to Aspire and WebAssembly templates ([#3945](https://github.com/microsoft/fluentui-blazor/pull/3945))
- \[TimePicker\] Fix the Time format when another culture is used, add TimeDisplay enum ([#3961](https://github.com/microsoft/fluentui-blazor/pull/3961))
- \[Toast\] Add DynamicDependency to prevent trimming errors ([#3994](https://github.com/microsoft/fluentui-blazor/pull/3994))

### Demo site and documentation
- \[Docs\] Add documentation for possible placeholder autocomplete conflicts ([#3923](https://github.com/microsoft/fluentui-blazor/pull/3923))
- \[Docs\] ComponentBase page add MultiLine to DataGrid ([#3983](https://github.com/microsoft/fluentui-blazor/pull/3983))
- \[Docs\] Fix mismatch in accordion docs between the code and the description. ([#3932](https://github.com/microsoft/fluentui-blazor/pull/3932))
- \[Docs\] Fix typo in AppBar demo ([#3924](https://github.com/microsoft/fluentui-blazor/pull/3924))
- \[Docs\] Remove Autofocus parameter to prevent scrolling ([#3922](https://github.com/microsoft/fluentui-blazor/pull/3922))
- \[Docs\] Update Card example ([#3909](https://github.com/microsoft/fluentui-blazor/pull/3909))
 
### Icons and Emoji
- Update to Fluent UI System Icons 1.1.306.
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.

-------------
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


## Before v4.12.0
For versions before 4.12, see the [What's New? (v4.0 - v4.11.9)](/WhatsNew-Before412) page.

## Archives
For versions before 4.0, see [What's new? (archives)](/WhatsNew-Archive) page.
