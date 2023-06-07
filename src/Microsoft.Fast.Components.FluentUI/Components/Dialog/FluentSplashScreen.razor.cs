using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSplashScreen : FluentComponentBase
{
    [Parameter]
    public string ProductName { get; set; } = "Product name"; //PowerLaunchScreenResource.ProductName;

    [Parameter]
    public string SuiteName { get; set; } = "Suite name"; //PowerLaunchScreenResource.SuiteName;

    [Parameter]
    public string LoadingLabel { get; set; } = "Loading..."; //PowerLaunchScreenResource.LoadingLabel;

    public void UpdateLabels(string? productName = null, string? suiteName = null, string? loadingLabel = null)
    {
        if (productName != null)
        {
            ProductName = productName;
        }

        if (suiteName != null)
        {
            SuiteName = suiteName;
        }

        if (loadingLabel != null)
        {
            LoadingLabel = loadingLabel;
        }

        StateHasChanged();
    }
}
