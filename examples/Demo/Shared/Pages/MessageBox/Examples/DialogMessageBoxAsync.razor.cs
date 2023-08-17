using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.MessageBox.Examples
{
    public partial class DialogMessageBoxAsync
    {
        private IDialogReference? _dialog;
        private async Task ShowSuccessAsync()
        {
            DemoLogger.WriteLine($"Open Success MessageBox");
            await DialogService.ShowSuccessAsync("The action was run successfuly", "Success title here");
        }

        private async Task ShowWarningAsync()
        {
            DemoLogger.WriteLine($"Open Warning MessageBox");
            await DialogService.ShowWarningAsync("This is your final warning!", "Warning title here");
        }

        private async Task ShowErrorAsync()
        {
            DemoLogger.WriteLine($"Open Error MessageBox");
            await DialogService.ShowErrorAsync("This is an error", "Error title here");
        }

        private async Task ShowInformationAsync()
        {
            DemoLogger.WriteLine($"Open Information MessageBox");
            await DialogService.ShowInfoAsync("This is a message", "Info title here");
        }

        private async Task ShowConfirmationAsync()
        {
            DemoLogger.WriteLine($"Open Confirmation MessageBox");
            _dialog = await DialogService.ShowConfirmationAsync("Do you have two eyes?", "Yup", "Nope", "Eyes on you");
            DialogResult result = await _dialog.Result;
            MessageBoxContent? data = result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{data?.Title} - Confirmed: {!result.Cancelled}");
        }

        private async Task ShowMessageBoxLongAsync()
        {
            DemoLogger.WriteLine($"Open Long MessageBox");
            await DialogService.ShowInfoAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum");
        }

        private async Task ShowMessageBoxAsync()
        {
            DemoLogger.WriteLine($"Open Customized MessageBox");
            _dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
            {
                Content = new()
                {
                    Title = "My title",
                    MarkupMessage = new MarkupString("My <strong>customized</strong> message"),
                    Icon = new Icons.Regular.Size24.Games(),
                    IconColor = Color.Success,
                },
                PrimaryAction = "Plus",
                SecondaryAction = "Minus",
                Width = "300px",
            });
            DialogResult result = await _dialog.Result;
            MessageBoxContent? data = result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{data?.Title} - Action: {(!result.Cancelled ? "Plus" : "Minus")}");
        }
    }
}