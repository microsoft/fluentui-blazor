using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.MessageBox.Examples
{
    public partial class DialogMessageBox
    {
        private void ShowSuccess()
        {
            DemoLogger.WriteLine($"Open Success MessageBox");
            DialogService.ShowSuccess("The action was run successfuly", "Success title here");
        }

        private void ShowWarning()
        {
            DemoLogger.WriteLine($"Open Warning MessageBox");
            DialogService.ShowWarning("This is your final warning!", "Warning title here");
        }

        private void ShowError()
        {
            DemoLogger.WriteLine($"Open Error MessageBox");
            DialogService.ShowError("This is an error", "Error title here");
        }

        private void ShowInformation()
        {
            DemoLogger.WriteLine($"Open Information MessageBox");
            DialogService.ShowInfo("This is a message", "Info title here");
        }

        private void ShowConfirmation()
        {
            DemoLogger.WriteLine($"Open Confirmation MessageBox");
            DialogService.ShowConfirmation(this, HandleResult, "Do you have two eyes?", "Yup", "Nope", "Eyes on you");
        }

        private void ShowMessageBoxLong()
        {
            DemoLogger.WriteLine($"Open Long MessageBox");
            DialogService.ShowInfo("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum");
        }

        private void ShowMessageBox()
        {
            DemoLogger.WriteLine($"Open Customized MessageBox");
            DialogService.ShowMessageBox(new DialogParameters<MessageBoxContent>()
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
                OnDialogResult = DialogService.CreateDialogCallback(this, HandleResult)
            });
        }

        private async Task HandleResult(DialogResult result)
        {
            MessageBoxContent? data = result.Data as MessageBoxContent;
            await Task.Run(() => DemoLogger.WriteLine($"{data?.Title} - : Cancelled: {result.Cancelled}"));
        }
    }
}