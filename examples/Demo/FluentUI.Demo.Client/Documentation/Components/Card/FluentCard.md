---
title: Card
route: /Card
icon: ContactCard
---

# Card

A **FluentCard** is a container that holds information and actions related to a single concept or object, like a document or a contact.

Cards can give information prominence and create predictable patterns.
While they're very flexible, it's important to use them consistently for particular use cases across experiences.

By default, each card is of `role="group"`.

## Appearance

Cards can have different styles depending on the situation and where it is placed.

{{ CardAppearanceExample }}

## Shadow

Cards can have shadows to create a sense of depth and separation from the background.

{{ CardShadowExample }}

## Clickable

Adding a `OnClick` handler to a card will make it clickable, which is useful for navigation or actions.

{{ CardClickable }}

## Examples

### Default

A card is composed of regular components, such as avatars, text and buttons, laid out inside a `FluentCard`.

{{ CardExampleDefault }}

### Composition

Cards can be composed with other components to build rich elements for a page.

{{ CardExampleTemplatePowerpoint }}

### Filled appearance

{{ CardExampleFilledAppearance }}

### Selectable

Cards can be made selectable by toggling a local state on click and reflecting it visually, for example with a border and a checkmark indicator.

To position the checkbox in the card's top-right corner, the `FluentCard` must have `position: relative;` so that it establishes the containing block 
for the checkbox. The `FluentCheckbox` can then use `Style="position: absolute; top: 8px; right: 8px;"` to position itself 8 pixels 
from the card's top and right edges.

{{ CardExampleSelectable }}

### Card with click event

This card has a root click event that performs the `Open` action.

{{ CardExampleWithAction }}

### Linked Card

When a card doesn't have a separate button within its contents, it usually makes the most sense for the card to become 
the additional interactive element (a link in this example).

{{ CardExampleWithLink }}

## API FluentCard

{{ API Type=FluentCard }}