# Set up your machine to use the latest Fluent UI Blazor package

These instructions will get you set up with the **newest (potentially unstable!)** version of the Fluent UI Blazor package.

Each time a commit is pushed into the `main` or `dev` branches, the **Core** package is published to a special NuGet feed (not hosted on NuGet).


**This package is a preliminary version and are not intended for production use.
It is intended to be used to test the latest feature and bug fix.** 

If you just want the latest **released** version, the packages are on [NuGet.org](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components).

## Add necessary NuGet feed

The newest builds are pushed to a special feed, which you need to add in Visual Studio or through:
```sh
dotnet nuget add source --name dotnet9 https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9/nuget/v3/index.json
```

This will add the feed to any existing NuGet.config in the current directory or above, or else
in the global NuGet.config. See [configuring NuGet behavior](https://learn.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior) to read more about that.

If you are using Visual Studio, you can [Install and manage packages in Visual Studio](https://learn.microsoft.com/nuget/consume-packages/install-use-packages-visual-studio#package-sources)
and add the feed `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9/nuget/v3/index.json` there.

**Although the feed name is dotnet9, the package still contains .NET 8 DLL and can be used in projects targeting that version.**
## Documentation

Documentation and examples for this preliminary version are available at [https://preview.fluentui-blazor.net/](https://preview.fluentui-blazor.net/).

## Contributing

If you would like to contribute to this project, please read the document [contributing.md](contributing.md)
