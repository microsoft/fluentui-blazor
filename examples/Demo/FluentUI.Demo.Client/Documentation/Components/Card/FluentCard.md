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

{{ CardExampleSelectable }}

### With Action

When giving a card a top-level click handler, it's important to ensure the same action can be done by a button or link within the card.
This keeps the action accessible to screen reader, touch screen reader, keyboard, and voice control users.

{{ CardExampleWithAction }}

## API FluentCard

{{ API Type=FluentCard }}