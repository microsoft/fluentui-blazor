using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class MonacoDefaultExample : FluentComponentBase
{
    string Language = "";
    string Code = "";
    bool IsDarkMode = true;
    
    void Display_CSharp()
    {
        Language = "csharp";
        Code = """
                    using System;
                    
                    void Main()
                    {
                        Console.WriteLine("Hello World");
                    }
                    """;
    }

    void Display_JavaScript()
    {
        Language = "javascript";
        Code = """
                    function main() {
                        console.log("Hello World");
                    }
                    """;
    }

    void Display_Json()
    {
        Language = "json";
        Code = """
                    {
                        name: "Hello World",
                        age: 25
                    }
                    """;
    }

    void Display_Razor()
    {
        Language = "razor";
        Code = """
                @* Remember to replace the namespace below with your own project's namespace. *@
                @namespace FluentUI.Demo.Shared

                @inherits FluentComponentBase
                @implements IAsyncDisposable
                @inject AccentBaseColor AccentBaseColor
                @inject IJSRuntime JSRuntime;

                <div>
                    <FluentButton @ref="button" id="@idButton" Appearance="Appearance.Accent" aria-haspopup="true" aria-expanded="@toggle" @onclick=ToggleMenu @onkeydown=OnKeyDown>
                        Select Brand Color
                        <FluentIcon Slot="end" Name="@FluentIcons.ChevronDown" Color="@Color.Fill" />
                    </FluentButton>
                    <FluentMenu @ref="menu" id="@idMenu" Class="@menuClass" aria-labelledby="button" @onmenuchange=OnMenuChange>
                        <FluentMenuItem id="0078D4">Windows</FluentMenuItem>
                        <FluentMenuItem id="D83B01">Office</FluentMenuItem>
                        <FluentMenuItem id="464EB8">Teams</FluentMenuItem>
                        <FluentMenuItem id="107C10">Xbox</FluentMenuItem>
                        <FluentMenuItem id="8661C5">Visual Studio</FluentMenuItem>
                        <FluentMenuItem id="F2C811">Power BI</FluentMenuItem>
                        <FluentMenuItem id="0066FF">Power Automate</FluentMenuItem>
                        <FluentMenuItem id="742774">Power Apps</FluentMenuItem>
                        <FluentMenuItem id="0B556A">Power Virtual Agents</FluentMenuItem>
                    </FluentMenu>
                </div>
               """;
    }

    protected override void OnInitialized()
    {
        Display_CSharp();
    }
}