---
title: Overlay
route: /Overlay
---

# Overlay

Overlays are used to temporarily overlay screen content to focus a dialog, progress or other information/interaction.

## Default

{{ OverlayDefault }}

## Timed

A timed overlay that hides after being shown for 3 seconds

{{ OverlayTimed }}

## Transparent overlay

Overlay with a transparent background

{{ OverlayTransparent }}

## Background color

Overlay with a custom background color

{{ OverlayBackgroundColor }}

## Full screen

Overlay which takes up the whole screen.

{{ OverlayFullScreen }}

## Interactive
By using the `Interactive` and `InteractiveExceptId` properties, only the targeted element will not close the FluentOverlay panel. The user can click anywhere else to close the FluentOverlay.
In this example, the FluentOverlay will only close when the user clicks outside the white zone and the user can increment the counter before to close the Overlay.

{{ OverlayInteractive }}

## API FluentOverlay

{{ API Type=FluentOverlay }}
