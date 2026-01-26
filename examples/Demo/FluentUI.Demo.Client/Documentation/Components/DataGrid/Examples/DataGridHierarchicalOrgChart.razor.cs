// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;
using static FluentUI.Demo.SampleData.People;

namespace FluentUI.Demo.Client.Documentation.Components.DataGrid.Examples;

public partial class DataGridHierarchicalOrgChart
{
    private PersonGridItem? ceoItem;
    private readonly List<PersonGridItem> items = [];

    protected override void OnInitialized()
    {
        var allPeople = GeneratePersons(30).ToList();

        // Level 0: CEO/Manager
        var ceo = allPeople[0] with { FirstName = "Mads", LastName = "Torgersen", JobTitle = "CEO", Department = "Executive" };
        ceoItem = new PersonGridItem
        {
            Item = ceo,
            IsCollapsed = false
        };
        items.Add(ceoItem);

        // ... existing code ...
        for (var i = 1; i <= 3; i++)
        {
            var manager = allPeople[i] with { JobTitle = "Director", Department = "Engineering" };
            var managerItem = new PersonGridItem { Item = manager, Depth = 1, IsCollapsed = i != 2 };
            ceoItem.Children.Add(managerItem);
            items.Add(managerItem);

            // Level 2: Employees
            for (var j = 0; j < 4; j++)
            {
                var employee = allPeople[4 + (i - 1) * 4 + j];
                var employeeItem = new PersonGridItem { Item = employee, Depth = 2, IsHidden = i != 2 };
                managerItem.Children.Add(employeeItem);
                items.Add(employeeItem);
            }
        }
    }

    private void ToggleCEO()
    {
        if (ceoItem is not null)
        {
            ceoItem.IsCollapsed = !ceoItem.IsCollapsed;
        }
    }

    public class PersonGridItem : HierarchicalGridItem<Person, PersonGridItem>
    {
    }
}
