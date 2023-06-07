using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogService : IDialogService
{
    public event Action<IDialogReference>? OnDialogInstanceAdded;

    public event Action<IDialogReference, DialogResult>? OnDialogCloseRequested;

    public Task<IDialogReference> ShowAsync<T>()
        where T : ComponentBase
    {
        return ShowAsync<T>(string.Empty, new DialogParameters(), new DialogOptions());
    }

    public Task<IDialogReference> ShowAsync<T>(string title)
        where T : ComponentBase
    {
        return ShowAsync<T>(title, new DialogParameters(), new DialogOptions());
    }

    public Task<IDialogReference> ShowAsync<T>(string title, DialogOptions options)
        where T : ComponentBase
    {
        return ShowAsync<T>(title, new DialogParameters(), options);
    }

    public Task<IDialogReference> ShowAsync<T>(string title, DialogParameters parameters)
        where T : ComponentBase
    {
        return ShowAsync<T>(title, parameters, new DialogOptions());
    }

    public Task<IDialogReference> ShowAsync<T>(string title, DialogParameters parameters, DialogOptions options)
        where T : ComponentBase
    {
        return ShowAsync(typeof(T), title, parameters, options);
    }

    public Task<IDialogReference> ShowAsync(Type contentComponent)
    {
        return ShowAsync(contentComponent, string.Empty, new DialogParameters(), new DialogOptions());
    }

    public Task<IDialogReference> ShowAsync(Type contentComponent, string title)
    {
        return ShowAsync(contentComponent, title, new DialogParameters(), new DialogOptions());
    }

    public Task<IDialogReference> ShowAsync(Type contentComponent, string title, DialogOptions options)
    {
        return ShowAsync(contentComponent, title, new DialogParameters(), options);
    }

    public Task<IDialogReference> ShowAsync(Type contentComponent, string title, DialogParameters parameters)
    {
        return ShowAsync(contentComponent, title, parameters, new DialogOptions());
    }

    //public Task<IDialogReference> ShowLaunchScreenAsync(string title, string subTitle, string? loading = null)
    //{
    //    DialogParameters parameters = new()
    //    {
    //        { nameof(PowerLaunchScreen.ProductName), title },
    //        { nameof(PowerLaunchScreen.SuiteName), subTitle },
    //        { nameof(PowerLaunchScreen.LoadingLabel), loading ?? PowerLaunchScreenResource.LoadingLabel },
    //    };

    //    DialogOptions options = new()
    //    {
    //        Alignment = HorizontalAlignment.Center,
    //        IsAllowadToCancelOusideDialog = false,
    //        ShowDismiss = false,
    //        IsCancelDisplayed = false,
    //        ShowOK = false,
    //        Width = "600px",
    //        Height = "370px",
    //        StyleBody = "width: 100%; height: 100%; margin: 0px;",
    //    };

    //    return ShowAsync(typeof(PowerLaunchScreen), string.Empty, parameters, options);
    //}

    public virtual async Task<IDialogReference> ShowAsync(Type contentComponent, string title, DialogParameters? parameters, DialogOptions? options)
    {
        if (!typeof(ComponentBase).IsAssignableFrom(contentComponent))
        {
            throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
        }

        var dialogReference = CreateReference();

        var dialogContent = new RenderFragment(builder =>
        {
            var i = 0;
            builder.OpenComponent(i++, contentComponent);

            // DialogReference
            var dialogReferenceProperty = GetDialogReferenceProperty(contentComponent);
            if (!string.IsNullOrEmpty(dialogReferenceProperty))
            {
                builder.AddAttribute(i++, dialogReferenceProperty, dialogReference);
            }

            if (parameters != null)
            {
                if (!dialogReference.AreParametersRendered)
                {
                    foreach (var parameter in parameters)
                    {
                        builder.AddAttribute(i++, parameter.Key, parameter.Value);
                    }

                    dialogReference.AreParametersRendered = true;
                }
                else
                {
                    i += parameters.Count;
                }
            }

            builder.AddComponentReferenceCapture(i++, inst => { dialogReference.InjectDialog(inst); });
            builder.CloseComponent();
        });

        var titleContent = new RenderFragment(builder =>
        {
            builder.AddContent(0, (MarkupString)title);
        });

        var dialogInstance = new RenderFragment(builder =>
        {
            builder.OpenComponent<FluentPanel>(0);
            builder.SetKey(dialogReference.Id);
            if (!string.IsNullOrEmpty(title))
            {
                builder.AddAttribute(1, "Header", titleContent);    // Property PowerPanel.Header
            }

            builder.AddAttribute(3, "Body", dialogContent);         // Property PowerPanel.Body
            builder.AddAttribute(4, "Id", dialogReference.Id);
            builder.AddAttribute(5, "IsOpen", true);
            builder.AddAttribute(6, "Options", options ?? new DialogOptions());
            builder.CloseComponent();
        });
        dialogReference.InjectRenderFragment(dialogInstance);
        OnDialogInstanceAdded?.Invoke(dialogReference);

        // If contentComponent implements IDialogInstance,
        // try to run close method
        if (contentComponent.GetInterface(nameof(IDialogInstance)) != null)
        {
            DialogResult? result = await dialogReference.Result;
            if (dialogReference.Dialog is IDialogInstance dialog)
            {
                await dialog.OnCloseAsync(result);
            }
        }

        return dialogReference;
    }

    //public Task<bool> ShowMessageBoxErrorAsync(string message, string? title = null)
    //{
    //    return ShowMessageBoxAsync(new MessageBoxOptions()
    //    {
    //        OkText = "Ok", /*FluentPanelResources.ButtonOK,*/
    //        CancelText = string.Empty,
    //        Icon = Icons.Regular.DismissCircle,
    //        IconColor = Colors.Status.ErrorType,
    //        Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*FluentPanelResources.TitleError*/ : title,
    //        Message = message,
    //    }) ; ;
    //}

    //public Task<bool> ShowMessageBoxInformationAsync(string message, string? title = null)
    //{
    //    return ShowMessageBoxAsync(new MessageBoxOptions()
    //    {
    //        OkText = "Ok", /*FluentPanelResources.ButtonOK,*/
    //        CancelText = string.Empty,
    //        Icon = Icons.Regular.Info,
    //        IconColor = Colors.Status.WarningType,
    //        Title = string.IsNullOrWhiteSpace(title) ? "Information" /*FluentPanelResources.TitleInformation*/ : title,
    //        Message = message,
    //    });
    //}

    //public Task<bool> ShowMessageBoxConfirmationAsync(string message, string? title = null)
    //{
    //    return ShowMessageBoxAsync(new MessageBoxOptions()
    //    {
    //        OkText = "Yes", /*FluentPanelResources.ButtonYes,*/
    //        CancelText = "No", /*FluentPanelResources.ButtonNo,*/
    //        Icon = Icons.Regular.QuestionCircle,
    //        IconColor = Colors.Status.SuccessType,
    //        Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*FluentPanelResources.TitleConfirmation*/ : title,
    //        Message = message,
    //    });
    //}

    //public async Task<bool> ShowMessageBoxAsync(MessageBoxOptions options)
    //{
    //    DialogParameters parameters = new()
    //    {
    //        { nameof(PowerMessageBox.MessageBoxOptions), options },
    //    };

    //    DialogOptions dialogOptions = new()
    //    {
    //        Alignment = HorizontalAlignment.Center,
    //        IsAllowadToCancelOusideDialog = false,
    //        ShowDismiss = false,
    //        CancelText = options.CancelText,
    //        IsCancelDisplayed = !string.IsNullOrEmpty(options.CancelText),
    //        OKText = options.OkText,
    //        ShowOK = !string.IsNullOrEmpty(options.OkText),
    //        Width = options.Width,
    //        Height = options.Height,
    //    };

    //    if (string.IsNullOrWhiteSpace(options.Title))
    //    {
    //        options.Title = "...";
    //    }

    //    var dialog = await ShowAsync(typeof(PowerMessageBox), options.Title, parameters, dialogOptions);
    //    var result = await dialog.Result;

    //    if (result.Cancelled)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    public Task CloseAsync(DialogReference dialog)
    {
        return CloseAsync(dialog, DialogResult.Ok<object?>(null));
    }

    public async Task CloseAsync(DialogReference dialog, DialogResult result)
    {
        await Task.Run(() => { });  // To avoid warning
        OnDialogCloseRequested?.Invoke(dialog, result);
    }

    internal virtual IDialogReference CreateReference()
    {
        return new DialogReference(Identifier.NewId(), this);
    }

    private static string? GetDialogReferenceProperty(Type contentComponent)
    {
        var property = contentComponent.GetProperties()
                                       .FirstOrDefault(i => i.PropertyType == typeof(IDialogReference) &&
                                                            i.CanRead &&
                                                            i.CanWrite);

        var parameterAttribute = property?.GetCustomAttribute<ParameterAttribute>();

        if (property != null && parameterAttribute != null)
        {
            return property.Name;
        }
        else
        {
            return null;
        }
    }
}
