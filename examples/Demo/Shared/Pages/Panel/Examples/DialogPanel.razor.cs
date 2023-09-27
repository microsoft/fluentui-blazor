using System.Diagnostics.CodeAnalysis;
using FluentUI.Demo.Shared.SampleData;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Panel.Examples
{
    public partial class DialogPanel
    {
        private readonly SimplePerson simplePerson = new()
        {
            Firstname = "Steve",
            Lastname = "Roth",
            Age = 42,
        };

        private void OpenPanelRight()
        {
            DemoLogger.WriteLine($"Open right panel");

#pragma warning disable CS0618 // Type or member is obsolete
            DialogService.ShowPanel<SimplePanel, SimplePerson>(new DialogParameters<SimplePerson>()
            {
                Content = simplePerson,
                Alignment = HorizontalAlignment.Right,
                Title = $"Hello {simplePerson.Firstname}",
                OnDialogResult = DialogService.CreateDialogCallback(this, HandlePanel),
                PrimaryAction = "Yes",
                SecondaryAction = "No",
            });
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private void OpenPanelLeft()
        {
            DemoLogger.WriteLine($"Open left panel");
            DialogParameters<SimplePerson> parameters = new()
            {
                Content = simplePerson,
                Title = $"Hello {simplePerson.Firstname}",
                OnDialogResult = DialogService.CreateDialogCallback(this, HandlePanel),
                Alignment = HorizontalAlignment.Left,
                Modal = false,
                ShowDismiss = false,
                PrimaryAction = "Maybe",
                SecondaryAction = "Cancel",
                Width = "300px",
            };
#pragma warning disable CS0618 // Type or member is obsolete
            DialogService.ShowPanel<SimplePanel, SimplePerson>(parameters);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private async Task HandlePanel(DialogResult result)
        {
            if (result.Cancelled)
            {
                await Task.Run(() => DemoLogger.WriteLine($"Panel cancelled"));
                return;
            }

            if (result.Data is not null)
            {
                SimplePerson? simplePerson = result.Data as SimplePerson;
                await Task.Run(() => DemoLogger.WriteLine($"Panel closed by {simplePerson?.Firstname} {simplePerson?.Lastname} ({simplePerson?.Age})"));
                return;
            }

            await Task.Run(() => DemoLogger.WriteLine($"Panel closed"));
        }
    }
}