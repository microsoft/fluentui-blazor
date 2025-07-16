// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTimePicker : FluentInputBase<DateTime?>
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/DateTime/FluentTimePicker.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary />
    protected override string? StyleValue => new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the design of this input.
    /// </summary>
    [Parameter]
    public virtual FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the time format.
    /// </summary>
    [Parameter]
    public TimeDisplay TimeDisplay { get; set; } = TimeDisplay.HourMinute;

    /// <summary />
    protected override string? FormatValueAsString(DateTime? value)
    {
        var format = TimeDisplay switch
        {
            TimeDisplay.HourMinute => "HH:mm",
            TimeDisplay.HourMinuteSeconds => "HH:mm:ss",
            _ => "HH:mm",
        };

        var result = value?.ToString(format, CultureInfo.InvariantCulture);

        return result;
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        var acceptedFormats = new string[] { "HH:mm", "HH:mm:ss", "HH:mm:ss.fff" };

        DateTime currentValue = Value ?? DateTime.MinValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            result = null;
        }
        else if (DateTime.TryParseExact(value, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var valueConverted))
        {
            result = currentValue.Date + valueConverted.TimeOfDay;
        }
        else
        {
            result = Value?.Date;
        }

        validationErrorMessage = null;
        return true;
    }

    /// <summary />
    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(Id) && TimeDisplay == TimeDisplay.HourMinuteSeconds)
        {
            Id = Identifier.NewId();
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && TimeDisplay == TimeDisplay.HourMinuteSeconds)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            await Module.InvokeVoidAsync("setControlAttribute", Id, "step", 1);
        }
    }
}
