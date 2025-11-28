---
title: Migrating FluentSelect
route: /Migration/Select
hidden: true
---

### Appearance 💥
  The `Appearance` property has been updated to use the `ListAppearance` enum
    instead of `Appearance` enum.

    `ListAppearance` enum has the following values:
    - `FilledLighter`
    - `FilledDarker`
    - `Outline`
    - `Transparent`

### New properties

- `LabelPosition`, sets the element’s label position relative to the input.
- `LabelWidth`, sets the width of the label.
- `Margin`, sets the margin of the component.
- `Message`, sets the message displayed below the component.
- `MessageCondition`, sets the condition to display the message.
- `MessageIcon`, sets the icon displayed next to the message.
- `MessageState`, sets a value that affects the display style of the message.
- `MessageTemplate`, sets a custom template for the message.
- `OptionSelectedComparer`, sets a function to compare whether two options are considered equal for selection purposes.
- `Padding`, sets the padding of the component.

### Changed properties💥

- `Value` is now generic instead of type `string?`
- `ValueExpression` is now generic instead of type `Expression<Func<string>>?`
- `Disabled` is now of type `bool?` instead of `bool`. This requires the user to define `Disabled="true"` instead of just `Disabled`.

### Removed properties💥

- `ChangeOnEnterOnly`
- `Embedded`
- `Field`
- `Immediate`
- `ImmediateDelay`
- `Open`
- `OptionComparer` use `OptionSelectedComparer` instead.
- `OptionTitle`
- `Position`
- `SelectedOption` use `Value` instead.
- `SelectedOptionExpression`
- `SelectedOptions` use `SelectedItems` instead.
- `SelectedOptionsExpression`
- `Title`
- `SelectedOptionChanged` use `ValueChanged` instead.
- `SelectedOptionsChanged` use `SelectedItemsChanged` instead.
