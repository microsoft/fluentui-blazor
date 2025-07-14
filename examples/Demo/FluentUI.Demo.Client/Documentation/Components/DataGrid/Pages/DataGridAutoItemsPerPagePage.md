---
title: Auto items per page
route: /DataGrid/AutoItemsPerPage
---

# Auto items per page

The example and code below show what you need to get auto items per page functionality for the pagination of a datagrid.

Resize the page vertically to see the number of rows being displayed per page adapt to the available height.

The `AutoItemsPerPage` parameter must be set to true and obviously `Pagination` must be used as well for this to work.

Also, the DataGrid **container** must have styling that makes it automatically adapt to the available height.

An example of how that can be done for this demo site layout is shown in the `<style>` section below

```css
<style>

    #datagrid-container {
        height: calc(100% - 3rem);
        min-height: 8rem;
        overflow-x: auto;
        overflow-y: hidden;
    }

    article {
        min-height: 32rem;
        max-height: 80dvh;
    }

    .demo-section-content {
        height: calc(100% - 10rem);
    }

    .demo-section-example {
        min-height: 135px !important;
        height: 100%;
    }

    fluent-tabs {
        height: 100%;
    }

    #tab-example-autoitemsperpage-panel {
        height: 100% !important;
        max-height: calc(100% - 2rem) !important;
    }
</style>
```

{{ DataGridAutoItemsPerPage Files=Code:DataGridAutoItemsPerPage.razor;CustomCSS.css:CustomCSS.css}}

