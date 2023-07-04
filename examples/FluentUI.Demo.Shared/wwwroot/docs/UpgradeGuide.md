# ToDo

## When using icons or emoji in version 2
For version 2 you had to add a `<ProjectGroup>` similar to below to your project file:
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