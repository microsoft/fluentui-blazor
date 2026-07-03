---
title: Migration FluentBreadcrumb
route: /Migration/Breadcrumb
hidden: true
---

- ### Component removed 

  `FluentBreadcrumb` and `FluentBreadcrumbItem` have been **removed** in V5.
  There is no direct replacement component.

- ### V4 components

  - `FluentBreadcrumb` — container with `ChildContent`.
  - `FluentBreadcrumbItem` — individual item with `Href`, `Target`, `Appearance`, `Download`,
    `Hreflang`, `Ping`, `Referrerpolicy`, `Rel`, `Type`, `ChildContent`.

- ### Migration strategy

  Build a custom breadcrumb using `FluentStack` with `FluentLink` elements and separator icons:

  ```xml
  <!-- V4 -->
  <FluentBreadcrumb>
      <FluentBreadcrumbItem Href="/">Home</FluentBreadcrumbItem>
      <FluentBreadcrumbItem Href="/products">Products</FluentBreadcrumbItem>
      <FluentBreadcrumbItem>Current Page</FluentBreadcrumbItem>
  </FluentBreadcrumb>

  <!-- V5 — Custom breadcrumb -->
  <FluentStack Orientation="Orientation.Horizontal" HorizontalGap="4px">
      <FluentLink Href="/">Home</FluentLink>
      <FluentIcon Value="@(new Icons.Regular.Size12.ChevronRight())" />
      <FluentLink Href="/products">Products</FluentLink>
      <FluentIcon Value="@(new Icons.Regular.Size12.ChevronRight())" />
      <FluentText>Current Page</FluentText>
  </FluentStack>
  ```
