// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.Shared.SampleData;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.Panel.Examples;
public partial class DialogPanelWithValidation
{
    private IDialogReference? _dialog;

    private readonly SimplePerson simplePerson = new()
    {
        Firstname = "Steve",
        Lastname = "Roth",
        Age = 42,
    };

    private async Task OpenPanelRightAsync()
    {
        DemoLogger.WriteLine($"Open right panel");

        MessageService.Clear();

        _dialog = await DialogService.ShowPanelAsync<SimplePanel>(simplePerson, new DialogParameters<SimplePerson>()
        {
            Content = simplePerson,
            Alignment = HorizontalAlignment.Right,
            Title = $"Hello {simplePerson.Firstname}",
            PrimaryAction = "Yes",
            SecondaryAction = "No",
            PreventDismissOnOverlayClick = true,
            ValidateDialogAsync = async () =>
            {
                var result = simplePerson.Firstname.Length > 0 && simplePerson.Lastname.Length > 0;

                if (!result)
                {
                    DemoLogger.WriteLine("Panel cannot be closed because of validation errors.");

                    MessageService.ShowMessageBar(options =>
                    {
                        options.Intent = MessageIntent.Error;
                        options.Title = "Validation error";
                        options.Body = "First name and last name cannot be empty";
                        options.Timestamp = DateTime.Now;
                        options.Section = App.MESSAGES_DIALOG;
                    });
                }

                await Task.Delay(100);

                return result;
            }
        });

        DialogResult result = await _dialog.Result;
    }
}
