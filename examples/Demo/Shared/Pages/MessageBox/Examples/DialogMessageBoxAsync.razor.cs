using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.MessageBox.Examples
{
    public partial class DialogMessageBoxAsync
    {
        private IDialogReference? _dialog;
        private MessageBoxContent? _data;
        private DialogResult? _result;

        private async Task ShowSuccessAsync()
        {
            DemoLogger.WriteLine($"Open Success MessageBox");
            _dialog = await DialogService.ShowSuccessAsync("The action was run successfuly", "Success title here");
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Confirmed: {!_result.Cancelled}");
        }

        private async Task ShowWarningAsync()
        {
            DemoLogger.WriteLine($"Open Warning MessageBox");
            _dialog = await DialogService.ShowWarningAsync("This is your final warning!", "Warning title here");
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Confirmed: {!_result.Cancelled}");
        }

        private async Task ShowErrorAsync()
        {
            DemoLogger.WriteLine($"Open Error MessageBox");
            _dialog = await DialogService.ShowErrorAsync("This is an error", "Error title here");
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Confirmed: {!_result.Cancelled}");
        }

        private async Task ShowInformationAsync()
        {
            DemoLogger.WriteLine($"Open Information MessageBox");
            _dialog = await DialogService.ShowInfoAsync("This is a message", "Info title here");
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Confirmed: {!_result.Cancelled}");
        }

        private async Task ShowConfirmationAsync()
        {
            DemoLogger.WriteLine($"Open Confirmation MessageBox");
            _dialog = await DialogService.ShowConfirmationAsync("Do you have two eyes?", "Yup", "Nope", "Eyes on you");
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Confirmed: {!_result.Cancelled}");
        }

        private async Task ShowMessageBoxLongAsync()
        {
            DemoLogger.WriteLine($"Open Long MessageBox");
            _dialog = await DialogService.ShowInfoAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum");
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Confirmed: {!_result.Cancelled}");
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
            _result = await _dialog.Result;
            _data = _result.Data as MessageBoxContent;
            DemoLogger.WriteLine($"{_data?.Title} - Action: {(_result.Cancelled ? "Minus" : "Plus")}");
        }
    }
}