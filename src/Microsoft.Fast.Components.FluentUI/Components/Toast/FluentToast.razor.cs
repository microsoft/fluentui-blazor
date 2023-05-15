using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using System.Diagnostics;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentToast : FluentComponentBase
{
    [CascadingParameter]
    internal InternalToastContext ToastContext { get; set; } = default!;

    /// <summary>
    /// Use a custom component in the notfication
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a callback when a toast is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnClick { get; set; }

    //private bool _showBody;



    private static readonly RenderFragment EmptyChildContent = builder => { };

   

    //protected override void OnParametersSet()
    //{
    //    if (Settings.PercentageComplete is not null && (Settings.PercentageComplete < 0 || Settings.PercentageComplete > 100))
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(Settings.PercentageComplete), "PercentageComplete must be between 0 and 100");
    //    }
    //    else
    //    {
    //        _showBody = true;
    //    }
    //    if (Settings.PrimaryAction is not null || Settings.SecondaryAction is not null)
    //    {
    //        _showBody = true;
    //    }
    //    if (!string.IsNullOrWhiteSpace(Settings.Subtitle))
    //    {
    //        _showBody = true;
    //    }
    //    if (!string.IsNullOrWhiteSpace(Settings.Details))
    //    {
    //        _showBody = true;
    //    }
    //}

    public FluentToast() 
    {
        Id = Identifier.NewId();
    }

    //public FluentToast(ToastIntent intent, string title, ToastSettings settings) : this()
    //{
    //    Title = title;
    //    Intent = intent;
    //    Settings = settings;
    //}

    //public FluentToast(RenderFragment renderFragment, ToastSettings settings) : this()
    //{
    //    ChildContent = renderFragment;
    //    Settings = settings;
    //}

   // protected internal override void ToastContent(RenderTreeBuilder builder)
   //    => builder.AddContent(0, Content());

   

    

   

    private async Task HandleOnClickAsync()
    {
        await OnClick.InvokeAsync(Id);
    }
}