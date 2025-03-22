---
title: Select
route: /List/Select
---

# Select

The **FluentSelect** component allows one option to be selected from multiple options.

View the [Usage Guidance](https://fluent2.microsoft.design/components/web/react/select/usage).

## Default

When no item is selected, a **Placeholder** can be used to describe
what should be done. If an item is selected, the user cannot deselect it.

{{ SelectDefault }}

## Customize the items

You can customize the items in the **FluentSelect** component by using Lambda expressions.
The following example shows how to customize the items:
- `OptionText`: This function is used to customize the text of the option. <br />
   In the following example, the **FirstName** is used.
- `OptionValue`: This function is used to customize the value of the option. <br />
   In the following example, the **Id** is returned.
- `OptionDisabled`: This function is used to define the disabled options. <br />
   In the following example, the third element is disabled.

In the following example, the preselected item is defined by the **Value** parameter,
(third item in this example).

{{ SelectCustomized }}

## Enum values

You can use an **Enum** to create a list of options.
In this example, we are using an **OptionTemplate** to display a colored block
in front of each item. Due to the **OptionTemplate**, the user must click
on the checkbox to select an item.

{{ SelectEnum }}

## Multiple

Use the **Multiple** parameter to enable multiple selections.
Selected items are bound to the **SelectedItems** property.

{{ SelectMultiple }}

## Manual FluentOption

You can use a list of **FluentOption** to create a manual list.

{{ SelectManual }}

## API FluentSelect

{{ API Type=FluentSelect }}

{{ API Type=FluentOption }}

## Migrating to v5

TODO
