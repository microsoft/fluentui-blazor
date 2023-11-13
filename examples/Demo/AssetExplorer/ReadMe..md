# Assets Explorer

This is a simple Blazor site app that allows you to explore the assets (Icons and Emojis) of the Fluent UI Blazor library.

## Web.Config

To allow an external site to embed your site in an iframe,
you need to add a [Content-Security-Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/frame-ancestors) item
the following to your ´web.config` file:

```xml
...
<system.webServer>
  <httpProtocol>
    <customHeaders>
      <remove name="X-Frame-Options" />
      <add name="Content-Security-Policy" value="frame-ancestors 'self' https://localhost:7026 https://localhost:7062 https://fluentui-blazor.net https://www.fluentui-blazor.net" />
    </customHeaders>
  </httpProtocol>
</system.webServer>
...
```