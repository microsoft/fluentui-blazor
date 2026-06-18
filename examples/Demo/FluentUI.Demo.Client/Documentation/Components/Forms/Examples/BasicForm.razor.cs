// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.SampleData;
using Microsoft.FluentUI.AspNetCore.Components;
using static FluentUI.Demo.SampleData.Olympics2024;

namespace FluentUI.Demo.Client.Documentation.Components.Forms.Examples;

public partial class BasicForm
{
    private Starship starship { get; set; } = new();

    protected override void OnInitialized()
    {
        starship.ProductionDate = System.DateTime.Now;
    }

    private static void OnSearch(OptionsSearchEventArgs<Country> e)
    {
        var allCountries = Countries;
        e.Items = allCountries.Where(i => i.Name.StartsWith(e.Text, StringComparison.OrdinalIgnoreCase))
                              .OrderBy(i => i.Name);
    }

    private static void HandleValidSubmit()
    {
        Console.WriteLine("HandleValidSubmit called");
        // Processing the valid form is not implemented for demo purposes
    }
}
