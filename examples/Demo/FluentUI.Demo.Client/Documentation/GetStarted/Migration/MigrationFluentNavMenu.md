---
title: Migration FluentNavMenu and FluentNavMenuTree
route: /Migration/NavMenu
hidden: true
---

- ### Components removed and replaced 

  `FluentNavMenu`, `FluentNavGroup`, `FluentNavLink` have been **removed** in V5.
  `FluentNavMenuTree`, `FluentNavMenuGroup`, `FluentNavMenuLink` (already obsolete in V4) are also removed.

  Use the new `FluentNav` component system as replacement.

- ### Component mapping

  | V4 Component | V5 Component |
  |-------------|-------------|
  | `FluentNavMenu` | `FluentNav` |
  | `FluentNavGroup` | `FluentNavCategory` |
  | `FluentNavLink` | `FluentNavItem` |
  | `FluentNavMenuTree` | `FluentNav` |
  | `FluentNavMenuGroup` | `FluentNavCategory` |
  | `FluentNavMenuLink` | `FluentNavItem` |
  | *(none)* | `FluentNavSectionHeader` *(new)* |

- ### FluentNavMenu → FluentNav

  | V4 Property | V5 Property | Change |
  |-------------|-------------|--------|
  | `Title` | — | **Removed** |
  | `Width` (`int?`) | `Width` (`string?`) | Type changed (int → string CSS value) |
  | `Collapsible` | — | **Removed** — use layout-level hamburger |
  | `CollapsedChildNavigation` | — | **Removed** |
  | `Expanded` / `ExpandedChanged` | — | **Removed** — handled at layout level |
  | `Margin` | — | **Removed** from nav container |
  | `CustomToggle` | — | **Removed** |
  | `ExpanderContent` | — | **Removed** |

- ### FluentNavGroup → FluentNavCategory

  | V4 Property | V5 Property | Change |
  |-------------|-------------|--------|
  | `Title` | `Title` | Same |
  | `Icon` | `IconRest` / `IconActive` | Split into rest/active variants |
  | `Expanded` / `ExpandedChanged` | `Expanded` / `ExpandedChanged` | Same |
  | `HideExpander` | — | **Removed** |
  | `MaxHeight` | — | **Removed** |
  | `Gap` | — | **Removed** |
  | `ExpandIcon` | — | **Removed** |
  | `TitleTemplate` | — | **Removed** |
  | `Href` | — | **Removed** — categories are not links |

- ### FluentNavLink → FluentNavItem

  | V4 Property | V5 Property | Change |
  |-------------|-------------|--------|
  | `Href` | `Href` | Same |
  | `Icon` / `IconColor` / `CustomColor` | `IconRest` / `IconActive` | Renamed; color handled differently |
  | `Target` (`string?`) | `Target` (`LinkTarget?`) | Changed to typed enum |
  | `Disabled` | `Disabled` | Same |
  | `Match` | `Match` | Same |
  | `ActiveClass` | `ActiveClass` | Same |
  | `OnClick` | `OnClick` | Same |
  | `Tooltip` | `Tooltip` | Same |
  | `ForceLoad` | — | **Removed** |
  | `CustomToggleId` | — | **Removed** |

- ### Migration example

  ```xml
  <!-- V4 -->
  <FluentNavMenu Width="250" Collapsible="true" Title="Navigation">
      <FluentNavGroup Title="Pages" Icon="@(new Icons.Regular.Size20.Document())"
                      Expanded="true">
          <FluentNavLink Href="/" Icon="@(new Icons.Regular.Size20.Home())"
                         Match="NavLinkMatch.All">Home</FluentNavLink>
          <FluentNavLink Href="/about">About</FluentNavLink>
      </FluentNavGroup>
  </FluentNavMenu>

  <!-- V5 -->
  <FluentNav Width="250px">
      <FluentNavCategory Title="Pages" IconRest="@(new Icons.Regular.Size20.Document())"
                         Expanded="true">
          <FluentNavItem Href="/" IconRest="@(new Icons.Regular.Size20.Home())"
                         Match="NavLinkMatch.All">Home</FluentNavItem>
          <FluentNavItem Href="/about">About</FluentNavItem>
      </FluentNavCategory>
  </FluentNav>
  ```
