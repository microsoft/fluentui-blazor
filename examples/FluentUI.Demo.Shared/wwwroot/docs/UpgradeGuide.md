## When using icons or emoji in version 2
There are a couple of changes that need to be done to upgrade from v2 to v3 when using icons/emoji:

### Changes in `.csproj` file
For version 2 you needed to add a `<ProjectGroup>` similar to below to your project file:
```xml
<PropertyGroup>
	<PublishFluentIconAssets>true</PublishFluentIconAssets>
	<FluentIconSizes>10,12,16,20,24,28,32,48</FluentIconSizes>
	<FluentIconVariants>Filled,Regular</FluentIconVariants>
	<PublishFluentEmojiAssets>true</PublishFluentEmojiAssets>
	<FluentEmojiGroups>Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places</FluentEmojiGroups>
	<FluentEmojiStyles>Color,Flat,HighContrast</FluentEmojiStyles>
</PropertyGroup>
```
This whole group should be **removed** from your project file. See the [Project setup](https://www.fluentui-blazor.net/ProjectSetup)
for instructions on how to work with icons and emoji in version 3.

### Changes in `Program.cs`
The `AddFluentUIComponents()` service collection extension needs to be changed. 
The `options.IconConfiguration` and `options.EmojiConfiguration` lines are no longer necessary and should be removed.

```csharp
builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = {see remark below};   
});
```

The `options.HostingModel` setting is used to determine the type of project you are building. Choose a value from the `BlazorHostingModel` enumeration which reflects your type of project.

### Changes in code
**ALL** occurences of <`<FluentIcon>` and `<FluentEmoji>` should be checked and changed to work with the new format.

For icons the new format is like:
 
```razor
<FluentIcon Icon="@(Icons.Regular.Size24.Save)" />
```

> Names are structured as follows: `Icons.[IconVariant].[IconSize].[IconName]`.

For emoji the new format is:

```razor
<FluentEmoji Emoji="@(Emojis.PeopleBody.Color.Default.Artist)" />
```

> Names are structured as follows: `Emojis.[EmojiGroup].[EmojiStyle].[EmojiSkintone].[EmojiName]`.


## Using the library from a RCL
For version 2 you had to add the following to your project file:
```xml
<Target Name="DisableAnalyzers" BeforeTargets="CoreCompile">
	<ItemGroup>
		<Analyzer Remove="@(Analyzer)" Condition="'%(Filename)' == 'Microsoft.Fast.Components.FluentUI.Configuration'" />
	</ItemGroup>
</Target>
```
This is no longer necessary. The code should be **removed** from your project file
```xml