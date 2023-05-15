using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Microsoft.Fast.Components.FluentUI;

public partial class ConfirmationToast : FluentToastBase, IToastComponent
{
    protected override void OnInitialized()
    {
        
        //Settings = Toast.Settings;
    }

    public ConfirmationToast() : base()
    {
        //Id = Identifier.NewId();
        EndContentType = ToastEndContentType.Dismiss;
    }

    public ConfirmationToast(ToastIntent intent, string title, ToastSettings settings) : this()
    {
        Intent = intent;
        Title = title;
        Settings = settings;

    }
    
    protected internal override void ToastContent(RenderTreeBuilder builder)
        => builder.AddContent(0, Content());
}