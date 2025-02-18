---
title: RatingDisplay
route: /rating-display
---

# Rating display

**RatingDisplay** is used to communicate user sentiment.
By default, it shows rating as filled stars out of 5, as well as a text displaying the average value and the aggregate number of ratings.

## Best practices

**Do**
- Always display the value of the `FluentRatingDisplay`.
- Display the total number of ratings if known.
- Use the `FluentRatingDisplay `to represent only one thing.

**Don't**
- Display an empty `FluentRatingDisplay`.
- Display a `FluentRatingDisplay` with no value.

## Default

{{ DisplayRatingDefault }}

## Value

The `Value` controls the number of filled stars, and is written out next to the `FluentRatingDisplay`. The number of filled stars is rounded to the nearest half-star.

{{ DisplayRatingValue }}

## Count
You can specify the total number of ratings being displayed with the `Count`. The number will be formatted with a thousands separator according to the user's locale.

{{ DisplayRatingCount }}

## Compact
You can specify a compact `FluentRatingDisplay` with `Compact`.

{{ DisplayRatingCompact }}

## Max
You can specify the number of elements in the `FluentRatingDisplay` with the `Max` prop.

{{ DisplayRatingMax }}

## Shape
You can pass in a custom icon to the `FluentRatingDisplay` component using the icon prop.

{{ DisplayRatingShape }}

## Appearances

You can specify the appearance of the `FluentRatingDisplay` with the `Color` prop and the `Size`.

{{ DisplayRatingAppearance }}

## API FluentRatingDisplay

{{ API Type=FluentRatingDisplay}}


