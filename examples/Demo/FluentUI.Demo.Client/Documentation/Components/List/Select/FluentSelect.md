---
title: Select
route: /List/Select
---

# Select

The **FluentSelect** component allows one option to be selected from a list of options.

View the [Usage Guidance](https://fluent2.microsoft.design/components/web/react/select/usage).

## Default

A **Placeholder** can be used to give the user a hint of what can be selected.
If an item is selected, the user cannot deselect it.
If you want to allow the user to deselect the item, you can add an empty item
as shown in the Customized example below.

{{ SelectDefault }}

## Appearance

You can change the appearance of the **FluentSelect** component by using the **appearance** and/or **Size** parameters.

{{ SelectAppearance }}

## Customize the items

You can customize the items in the **FluentSelect** component by using Lambda expressions.
The following example shows how to customize the items:
- `OptionText`: This function is used to customize the text of the option.  
   In the following example, the **FirstName** is used.
- `OptionValue`: This function is used to customize the value of the option.  
   In the following example, the **Id** is returned.
- `OptionDisabled`: This function is used to define the disabled options.  
   In the following example, the third element is disabled.

In the following example, the preselected item is defined by the **Value** parameter,
(third item in this example).

{{ SelectCustomized }}

## OptionTemplate and Enum values

You can use an **Enum** to create a list of options.
In this example, we are using an **OptionTemplate** to display a colored block
in front of each item. Due to the **OptionTemplate**, the user must click
on the checkbox to select an item.

{{ SelectEnum }}

## Multiple

Use the **Multiple** parameter to enable multiple selections.
Selected items are bound to the **SelectedItems** property.

{{ SelectMultiple }}

## Disabled and ReadOnly

{{ SelectDisabledReadOnly }}

> The **FluentSelect** component does not support the `ReadOnly` attribute.
> The `Disabled` attribute is used to prevent user interaction with the list.

## Manual FluentOption

You can use a list of **FluentOption** to create a manual list.

{{ SelectManual }}

## Many items

You can use this component with a large number of items.
But all items are rendered in the HTML code and loaded at once.
If you have a very large number of items, you should use the **FluentAutocomplete** component.

{{ SelectManyItems }}

## API FluentSelect

{{ API Type=FluentSelect }}

## API FluentOption

{{ API Type=FluentOption }}

## Migrating to v5

TODO
