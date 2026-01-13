---
title: Overlay
route: /Overlay
icon: CursorHover
---

# Overlay

The Overlays are used to temporarily overlay screen content to focus a dialog, progress or other information/interaction.

When the `FullScreen` parameter is set to `false`, the overlay will only cover the content area of its parent container.

## Default

Full screen overlay with a default background color (`var(--colorBackgroundOverlay)`) and opacity of 40%.

{{ OverlayDefault }}

## Detailed Usage

The overlay can be customized to suit various use cases, including:

- **Full-Screen Overlay**: By setting the `FullScreen` property to `true`, the overlay will cover the entire screen.
- **Interactive Overlay**: The `Interactive` property allows user interactions with the overlay content.
- **Custom Close Modes**: The `CloseMode` property can be set to control how the overlay can be closed (e.g., by clicking outside of it or only by code).

{{ OverlayDetailed }}

## API FluentOverlay

{{ API Type=FluentOverlay }}
