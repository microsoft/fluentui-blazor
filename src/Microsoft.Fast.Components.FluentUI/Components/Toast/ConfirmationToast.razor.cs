using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ConfirmationToast : FluentToast, IToastComponent
{
    [CascadingParameter]
    private FluentToast Toast { get; set; } = default!;

    protected override void OnInitialized()
    {
        Id = Toast.Id;
        Settings = Toast.Settings;
    }

    public ConfirmationToast() : base()
    {
        //EndContentType = ToastEndContentType.Dismiss;
    }

    public ConfirmationToast(ToastIntent intent, string title, ToastSettings settings) : this()
    {
        Intent = intent;
        Title = title;
        Settings = settings;

    }

    // <inheritdoc />
    //protected internal override void ToastContent(RenderTreeBuilder builder)
    //    => builder.AddContent(0, Content());


}