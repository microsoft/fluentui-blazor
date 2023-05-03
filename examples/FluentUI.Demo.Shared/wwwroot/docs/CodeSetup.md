## Configuring the `AddFluentUIComponents()` service collection extension
 
You get run-time checks when using the `<FluentIcon>` and `<FluentEmoji>` components on the referenced assets actually being available. 
To enable this, the `AddFluentUIComponents()` service collection extension needs some configuration.
 
Services and configuration classed have been added to the library for this. You do not need to specify the configuration in code yourself. 
A source generator has been added that reads the settings from the project file and adds the necessary code at compile time. That way,
settings made in the project file and the source code are always kept in sync.
 
The two lines that need to be added to the `Program.cs` file are:

```csharp
LibraryConfiguration config = new(ConfigurationGenerator.GetIconConfiguration(), ConfigurationGenerator.GetEmojiConfiguration());
builder.Services.AddFluentUIComponents(config);
```


*If you're running your application on Blazor Server, make sure a default HttpClient is available by adding the following:*

```csharp
builder.Services.AddHttpClient();
```

*before adding the `AddFluentUIComponents()` code*