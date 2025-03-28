---
title: Tabs
route: /Tabs
---

# Tabs

A **FluentTabs** allows people to switch between categories of related information without going to different pages.
They’re are ideal for dividing content-heavy pages into distinct but related categories that are easier to process
and require less scrolling.
**FluentTabs** can also be used for navigation between a small set of closely related, frequently accessed pages.

For navigation beyond closely related categories, use a [FluentLink](/link) instead.
To initiate an action, use a [FluentButton](/button) instead.

## Default

{{ TabsDefault }}

## Customized

The header of each tab can be customized using the `HeaderTemplate` parameter.

{{ TabsCustomized }}

## Deferred

In some situations, the content of a tab may be expensive to load.
In these cases, you can use the `Deferred` parameter to load the content only when the tab is selected.
This can improve performance and reduce the initial load time of your application.

By default, a progress indicator is shown while the content is being loaded.
You can customize the progress indicator by using the `LoadingTemplate` parameter.

In the following example, the `Deferred` parameter is set to `true` for Tab two.
This tab will be loaded after 3 seconds of processing (to simulate a long running process).

{{ TabsDeferred }}
