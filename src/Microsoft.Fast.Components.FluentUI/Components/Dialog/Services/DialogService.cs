using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogService : IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, DialogParameters, Action<DialogSettings>?>? OnShow;

    //public event Action<DialogInstance>? OnDialogInstanceAdded;

    public event Action<string>? OnDialogCloseRequested;


    public void ShowDialog<T>(string title, DialogParameters parameters, Action<DialogSettings>? settings = null)
        where T : IDialogContentComponent
    {
        ShowDialog(typeof(T), title, parameters, settings);
    }



    //public virtual async Task<DialogInstance> ShowDialog(Type contentComponent, string title, DialogParameters? parameters, DialogSettings? settings)
    //{
    //    if (!typeof(IDialogContentComponent).IsAssignableFrom(contentComponent))
    //    {
    //        throw new ArgumentException($"{contentComponent.FullName} must be a Dialog Component");
    //    }

    //    IDialogReference? dialogReference = CreateReference();

    //    RenderFragment? dialogContent = new(builder =>
    //    {
    //        int i = 0;
    //        builder.OpenComponent(i++, contentComponent);

    //        // DialogReference
    //        string? dialogReferenceProperty = GetDialogReferenceProperty(contentComponent);
    //        if (!string.IsNullOrEmpty(dialogReferenceProperty))
    //        {
    //            builder.AddAttribute(i++, dialogReferenceProperty, dialogReference);
    //        }

    //        if (parameters != null)
    //        {
    //            if (!dialogReference.AreParametersRendered)
    //            {
    //                foreach (KeyValuePair<string, object> parameter in parameters)
    //                {
    //                    builder.AddAttribute(i++, parameter.Key, parameter.Value);
    //                }

    //                dialogReference.AreParametersRendered = true;
    //            }
    //            else
    //            {
    //                i += parameters.Count;
    //            }
    //        }

    //        builder.AddComponentReferenceCapture(i++, inst => { dialogReference.InjectDialog(inst); });
    //        builder.CloseComponent();
    //    });

    //    var titleContent = new RenderFragment(builder =>
    //    {
    //        builder.AddContent(0, (MarkupString)title);
    //    });

    //    var dialogInstance = new RenderFragment(builder =>
    //    {
    //        builder.OpenComponent<FluentPanel>(0);
    //        builder.SetKey(dialogReference.Id);
    //        if (!string.IsNullOrEmpty(title))
    //        {
    //            builder.AddAttribute(1, "Header", titleContent);    // Property PowerPanel.Header
    //        }

    //        builder.AddAttribute(3, "Body", dialogContent);         // Property PowerPanel.Body
    //        builder.AddAttribute(4, "Id", dialogReference.Id);
    //        builder.AddAttribute(5, "IsOpen", true);
    //        builder.AddAttribute(6, "Settings", settings ?? new());
    //        builder.CloseComponent();
    //    });
    //    dialogReference.InjectRenderFragment(dialogInstance);
    //    OnDialogInstanceAdded?.Invoke(dialogReference);

    //    // If contentComponent implements IDialogInstance,
    //    // try to run close method
    //    if (contentComponent.GetInterface(nameof(IDialogInstance)) != null)
    //    {
    //        DialogResult? result = await dialogReference.Result;
    //        if (dialogReference.Dialog is IDialogInstance dialog)
    //        {
    //            await dialog.OnCloseAsync(result);
    //        }
    //    }

    //    return dialogReference;
    //}


    /// <summary>
    /// Shows the dialog with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="title">TTitle of the dialog</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Settings to configure the dialog component.</param>
    public virtual void ShowDialog(Type dialogComponent, string title, DialogParameters parameters, Action<DialogSettings>? settings = null)
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        if (!string.IsNullOrEmpty(title))
        {
            parameters.Add("Title", title);
        }

        OnShow?.Invoke(dialogComponent, parameters, settings);

        //if (dialogComponent.GetInterface(nameof(IDialogHandler)) != null)
        //{
        //    DialogResult? result = await dialogReference.Result;
        //    if (dialogReference.Dialog is IDialogHandler dialog)
        //    {
        //        await dialog.OnCloseAsync(result);
        //    }
        //}

        //return dialogReference;
    }

    public void ShowSplashScreen(string title, string subTitle, string? loading = null)
    {
        DialogParameters parameters = new()
        {
            { nameof(FluentSplashScreen.ProductName), title },
            { nameof(FluentSplashScreen.SuiteName), subTitle },
            { nameof(FluentSplashScreen.LoadingLabel), loading ?? "Loading..." }, //PowerLaunchScreenResource.LoadingLabel },
        };

        Action<DialogSettings> settings = new(x =>
        {
            x.Alignment = HorizontalAlignment.Center;
            x.Modal = true;
            x.ShowDismiss = false;
            x.PrimaryButton = null;
            x.SecondaryButton = null;
            x.Width = "600px";
            x.Height = "370px";
            x.DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;";
            x.AriaLabel = "splashscreen";
        });

        ShowDialog(typeof(FluentSplashScreen), title, parameters, settings);
    }

    // ToDo: Add Success, Warning?
    public void ShowErrorAsync(string message, string? title = null)
    {
        ShowMessageBox(new MessageBoxParameters()
        {
            Intent = MessageBoxIntent.Error,
            PrimaryButtonText = "Ok", /*FluentPanelResources.ButtonOK,*/
            SecondaryButtonText = string.Empty,
            Icon = FluentIcons.DismissCircle,
            IconColor = Color.Error,
            Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*FluentPanelResources.TitleError*/ : title,
            Message = message,
        }); ;
    }

    public void ShowInfoAsync(string message, string? title = null)
    {
        ShowMessageBox(new MessageBoxParameters()
        {
            Intent = MessageBoxIntent.Info,
            PrimaryButtonText = "Ok", /*FluentPanelResources.ButtonOK,*/
            SecondaryButtonText = string.Empty,
            Icon = FluentIcons.Info,
            IconColor = Color.Warning,
            Title = string.IsNullOrWhiteSpace(title) ? "Information" /*FluentPanelResources.TitleInformation*/ : title,
            Message = message,
        });
    }

    public void ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback, string message, string? title = null)
    {
        ShowMessageBox(new MessageBoxParameters()
        {
            Intent = MessageBoxIntent.Confirmation,
            PrimaryButtonText = "Yes", /*FluentPanelResources.ButtonYes,*/
            SecondaryButtonText = "No", /*FluentPanelResources.ButtonNo,*/
            Icon = FluentIcons.QuestionCircle,
            IconColor = Color.Success,
            Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*FluentPanelResources.TitleConfirmation*/ : title,
            Message = message,
            OnDialogResult = EventCallback.Factory.Create(receiver, callback)
        });
    }

    public void ShowMessageBox(MessageBoxParameters parameters)
    {
        Action<DialogSettings> dialogSettings = new(x =>
        {
            x.Alignment = HorizontalAlignment.Center;
            x.Modal = string.IsNullOrEmpty(parameters.SecondaryButtonText);
            x.ShowDismiss = false;
            x.PrimaryButton = parameters.PrimaryButtonText;
            x.SecondaryButton = parameters.SecondaryButtonText;
            x.Width = parameters.Width;
            x.Height = parameters.Height;
        });


        if (string.IsNullOrWhiteSpace(parameters.Title))
        {
            parameters.Title = "...";
        }

        ShowDialog<MessageBox>(parameters.Title, parameters, dialogSettings);
    }

    //public Task CloseAsync(string dialogId)
    //{
    //    return CloseAsync(dialogId, DialogResult.Ok<object?>(null));
    //}

    //public async Task CloseAsync(string dialogId, DialogResult result)
    //{
    //    await Task.Run(() => { });  // To avoid warning
    //    OnDialogCloseRequested?.Invoke(dialogId, result);
    //}

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback)
    {
        return EventCallback.Factory.Create(receiver, callback);
    }
}
