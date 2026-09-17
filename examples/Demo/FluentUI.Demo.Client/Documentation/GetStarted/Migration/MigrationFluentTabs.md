---
title: Migration FluentTabs
route: /Migration/Tabs
hidden: true
---

- ### FluentTabs changes

  #### Changed properties

  | V4 Property | V5 Property | Change |
  |-------------|-------------|--------|
  | `Size` (`TabSize?`) | `Size` (`TabsSize?`) | Enum renamed |
  | `ActiveTabId` (`string`) | `ActiveTabId` (`string?`) | Now nullable |
  | `Orientation` (`Orientation`) | `Orientation` (`Orientation?`) | Now nullable |

  #### Removed properties 
  - `OnTabSelect` (`EventCallback<FluentTab>`) — use `ActiveTabChanged` instead.
  - `OnTabClose` (`EventCallback<FluentTab>`) — no built-in close/dismiss UI in V5.
  - `OnTabChange` (`EventCallback<FluentTab>`) — use `ActiveTabChanged` instead.
  - `ShowClose` (`bool`) — removed, no close button support.
  - `ShowActiveIndicator` (`bool`) — always shown in V5.

  #### New properties
  - `Appearance` (`TabsAppearance?`)
  - `Disabled` (`bool`)
  - `ActiveTab` (`FluentTab?`) — direct reference to the active tab.
  - `ActiveTabChanged` (`EventCallback<FluentTab?>`) — replaces `OnTabSelect`/`OnTabChange`.

- ### FluentTab changes

  #### Renamed properties 

  | V4 Property | V5 Property |
  |-------------|-------------|
  | `Label` (`string`) | `Header` (`string?`) |
  | `Header` (`RenderFragment?`) | `HeaderTemplate` (`RenderFragment?`) |
  | `LabelClass` (`string?`) | `HeaderClass` (`string?`) |
  | `LabelStyle` (`string?`) | `HeaderStyle` (`string?`) |
  | `Icon` (`Icon?`) | `IconStart` (`Icon?`) |
  | `LoadingContent` (`RenderFragment?`) | `LoadingTemplate` (`RenderFragment?`) |

  #### Removed properties 
  - `Label` — use `Header` instead.
  - `LabelChanged` (`EventCallback<string>`) — no longer editable inline.
  - `LabelEditable` (`bool`) — inline label editing is no longer supported.
  - `Content` (`RenderFragment?`) — use `ChildContent` for all content.

  #### New properties
  - `IconColor` (`Color?`) — icon color override.
  - `Tooltip` (`string?`)

- ### Migration example

  ```xml
  <!-- V4 -->
  <FluentTabs Size="TabSize.Small" @bind-ActiveTabId="activeId"
              OnTabSelect="@HandleSelect" ShowClose="true" OnTabClose="@HandleClose">
      <FluentTab Label="Tab 1" Icon="@(new Icons.Regular.Size16.Home())">
          <Content>Tab 1 content</Content>
      </FluentTab>
      <FluentTab Label="Tab 2">
          <Content>Tab 2 content</Content>
      </FluentTab>
  </FluentTabs>

  <!-- V5 -->
  <FluentTabs Size="TabsSize.Small" @bind-ActiveTabId="activeId"
              ActiveTabChanged="@HandleSelect">
      <FluentTab Header="Tab 1" IconStart="@(new Icons.Regular.Size16.Home())">
          Tab 1 content
      </FluentTab>
      <FluentTab Header="Tab 2">
          Tab 2 content
      </FluentTab>
  </FluentTabs>
  ```
