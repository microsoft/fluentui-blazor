# Set up your machine to use the latest FluentUI-Blazor package

These instructions will get you set up with the latest build of **FluentUI-Blazor**.

Each time a commit is pushed into the `main` ou `dev`, the **Core** package is published on a special NuGet repository.
Install the latest [Visual Studio 2022 Preview version](https://visualstudio.microsoft.com/vs/preview/) for the tooling.

**This package is a preliminary version and are not intended for production use.
It is intended to be used to test the latest feature and bug fix.** 

If you just want the last final release of **FluentUI-Blazor**, the packages are on [NuGet.org](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Components).

## Add necessary NuGet feed

The latest builds are pushed to a special feed, which you need to add:
```sh
dotnet nuget add source --name dotnet8 https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json
```

As usual this will add the feed to any existing NuGet.config in the directory or above,
or else in the global NuGet.config. See [configuring NuGet behavior](https://learn.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior) to read more about that.

Alternatively, if you are using Visual Studio, you can [Install and manage packages in Visual Studio](https://learn.microsoft.com/nuget/consume-packages/install-use-packages-visual-studio#package-sources)
and add the feed `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json` there.

## Documentation

The documentation for this preliminary version is available at [https://preview.fluentui-blazor.net/](https://preview.fluentui-blazor.net/).

## Contributing

If you would like to contribute to this project, please read the document [contributing.md](contributing.md)
