---
title: Spacer
route: /Spacer
---

# Spacer
The purpose of this component is to generate space between other components. You can either create
a space with a set width or create a space with a flexible width.

## Default
By default, the spacer will set a flex growth of '1' to fill any space it can compared to other elements in the flex container.
{{ SpacerDefault }}

## Vertical
If the containing element has a flex-direction of column, it is not necessary to specify the Orientation to 'Vertical', but it is a good practice to do so.
{{ SpacerVertical }}

## Horizontal with size parameter
{{ SpacerHorizontalPixels }}

## Vertical with size parameter
If setting the size parameter, keep in mind that if you want a 'Vertical' spacer, you need to set the `Orientation` parameter to `Vertical`, cause the component needs to know which dimension to set.
{{ SpacerVerticalPixels }}

## Migrating to v5
{{ INCLUDE File=MigrationFluentSpacer }}

##  API FluentSpacer

{{ API Type=FluentSpacer }}
