---
title: NumberInput
route: /NumberInput
icon: NumberSymbol
---

# NumberInput

The **FluentNumberInput** component enables a user to enter and edit numeric values.
It supports both integer and decimal numbers, with configurable `Min`, `Max`, and `Step` constraints.

**Value** can be one of the following **Numeric Types**:
`byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`.

## Numeric Types

The `TValue` generic parameter defines the type of the value. You must specify it explicitly.
For **integer** types (`int`, `long`, …), the step defaults to whole numbers.
For **decimal** types (`float`, `double`, `decimal`), set the `Step` parameter to control precision.

> Note: When using `float`, suffix literal values with `f` (e.g. `Step="0.1f"`).
> When using `decimal`, suffix with `m` (e.g. `Step="0.001m"`).

{{ NumberInputTypes }}

## Culture and Decimal Separator

The `FluentNumberInput` component always uses the **invariant culture** (dot `.` as decimal separator)
for internal value formatting and parsing. This matches the HTML standard: `<input type="number">`
always transmits values with a dot, even when the browser displays a comma for the user's locale.

This means:
- French users see `1,5` in the browser but the component correctly reads `1.5`
- No configuration is needed — it works across all cultures automatically
- To **display** the value in the user's culture, use `ToString("N2")` with `CultureInfo.CurrentCulture`

{{ NumberInputCulture }}

## Appearance

The visual style can be changed by setting the `Appearance` and `Size` properties.

{{ NumberInputAppearance }}

## States

A number input can be in different states: `Disabled`, `ReadOnly`, and `Required`.

{{ NumberInputStates }}

## Min, Max, and Step

Use `Min`, `Max`, and `Step` to constrain the allowed values and increment.

{{ NumberInputMinMaxStep }}

## Prefix and Suffix

Use the `StartTemplate` and `EndTemplate` properties to add a prefix or suffix to the number input,
such as a currency symbol or unit.

{{ NumberInputPrefixSuffix }}

## Immediate Mode

Set `Immediate="true"` to update the bound value on every keystroke instead of on blur.
Use `ImmediateDelay` to debounce rapid input.

{{ NumberInputImmediate }}

## API FluentNumberInput

{{ API Type=FluentNumberInput<int> }}
