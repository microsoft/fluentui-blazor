This folder contains projects created by using  the [Fluent UI Blazor  Templates](https://www.nuget.org/packages/Microsoft.FluentUI.AspNetCore.Templates). These projects are meant for experimentation, bug solving, etc. 
and offer an alternative to running the demo sites to see the Fluent UI Blazor components in action.

## Creating the projects
The `GenerateProjectsForTemplateValidation` PowerShell script will create a folder structure with projects that you can test/run/debug. The script needs 2 parameters to be specified:
    - `--interactivity` 
    - `--auth`

For `interactivity`, the following values can be provided:
- `none`, generate SSR project(s)
- `server`, , generate project(s) for rendermode=server
- `webassembly` , generate project(s) for rendermode=webassembly
- `auto`, generate project(s) for rendermode=auto
- `all`, generate projects for all rendermodes 

For `auth`, the following values can be provided:
- `none`, generate project(s) with no authentication set up
- `individual`, generate project(s) with individual authentication set up
- `all`, generate project(s) with both no authentication and individual authentication

Running the script with both parameters set to `all` will create the following folder structure:

- IndividualAccounts
    - A-SSR
    - B-Server
        - Global
        - PerPage
    - C-Wasm
        - Global
        - PerPage
    - D-Auto
        - Global
        - PerPage
        - NoSamplePages
- NoAuthentication
    - A-SSR
    - B-Server
        - Global
        - PerPage
    - C-Wasm
        - Global
        - PerPage
    - D-Auto
        - Global
        - PerPage
        - NoSamplePages

## Deleting or re-creating projects
The script is setup to force (re-)creation of folder structure and projects (depending on the parameters specified) when run. You can therefore make any desired change , do experiments, etc and 'reset' to the start situation easily. We advise to **not** change the solution file but just work with the folders and script to make changes.

## Running a generated project
In Visual Studio, use the `Set as Startup Project` option in the Solution Explorer to mark a project as the one you want to test/run/validate. When there are 2 projects in a folder, select the project **without** the `.Client` extension as the startup project.



