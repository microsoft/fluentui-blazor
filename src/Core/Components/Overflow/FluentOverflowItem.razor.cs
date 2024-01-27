
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflowItem : IDisposable
{
    private bool _disposed;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("power-overflow-item")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>
    /// Gets or sets the reference to the associated container.
    /// </summary>
    /// <value>The splitter.</value>
    [CascadingParameter]
    public FluentOverflow Container { get; set; }

    /// <summary>
    /// Gets or sets the content to display. All first HTML elements are included in the items flow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets True if this component is out of panel.
    /// </summary>
    public bool? Overflow { get; private set; }

    /// <summary>
    /// Gets the InnerText of <see cref="ChildContent"/>.
    /// </summary>
    public string Text { get; private set; } = string.Empty;

    public FluentOverflowItem()
    {
        Id = Identifier.NewId();
        Container = new FluentOverflow();
    }

    /// <summary />
    protected override void OnInitialized()
    {
        Container.AddItem(this);
        base.OnInitialized();
    }

    /// <summary />
    internal void SetProperties(bool? isOverflow, string? text)
    {
        Overflow = isOverflow == true ? isOverflow : null;
        Text = text ?? string.Empty;
    }

    /// <summary />
    public void Dispose()
    {
        Dispose(false);
    }

    /// <summary />
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            // Release unmanaged resources (natives).
            // ...

            if (disposing)
            {
                return;
            }

            // Dispose managed resources.
            Container.RemoveItem(this);
        }
        finally
        {
            _disposed = true;
            if (!disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
