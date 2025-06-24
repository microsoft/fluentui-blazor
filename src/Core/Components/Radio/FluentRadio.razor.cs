// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Fluent Radio button component.
/// </summary>
public partial class FluentRadio<TValue> : FluentComponentBase, IDisposable
{
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentRadio{TRadioValue}"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library. Cannot be null.</param>
    public FluentRadio(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// For unit testing purposes only.
    /// </summary>
    /// <param name="id"></param>
    internal FluentRadio(string? id) : this(LibraryConfiguration.Empty)
    {
        Id = id;
    }

    /// <summary />
    [CascadingParameter(Name = "RadioGroup")]
    internal FluentRadioGroup<TValue> Owner { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder.Build();

    /// <inheritdoc cref="IFluentField.Disabled" />
    [Parameter]
    public virtual bool? Disabled { get; set; }

    /// <inheritdoc cref="IFluentField.Label" />
    [Parameter]
    public virtual string? Label { get; set; }

    /// <inheritdoc cref="IFluentField.LabelTemplate" />
    [Parameter]
    public virtual RenderFragment? LabelTemplate { get; set; }

    /// <inheritdoc cref="IFluentField.LabelTemplate" />
    [Parameter]
    public virtual RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="IFluentField.LabelWidth" />
    [Parameter]
    public virtual string? LabelWidth { get; set; }

    /// <summary>
    /// Gets or sets the value of the radio element.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        if (Owner is null)
        {
            throw new InvalidOperationException($"The {nameof(FluentRadio<TValue>)} must be included in a {nameof(FluentRadioGroup<TValue>)} component and must be of the same type.");
        }

        Owner.AddRadio(this);
    }

    /// <summary />
    internal string? GetValue()
    {
        return Owner.RadioValue?.Invoke(Value)
            ?? Value?.ToString()
            ?? Label
            ?? Id;
    }

    /// <summary />
    internal bool GetDisabled()
    {
        return Disabled is not null
            ? Disabled == true
            : Owner.RadioDisabled?.Invoke(Value) ?? false;
    }

    /// <summary />
    internal string? GetLabel()
    {
        return Label ?? Owner.RadioLabel?.Invoke(Value);
    }

    /// <summary />
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
                Owner.RemoveRadio(this);
            }

            _disposedValue = true;
        }
    }

    /// <summary />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
