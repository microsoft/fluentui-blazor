using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Fast.Components.FluentUI;
public class ActionButton<T>
{
    /// <summary>
    /// The text to show for the button
    /// </summary>
    public string? Text { get; set; }



    /// <summary>
    /// The function to call when the link is clicked
    /// </summary>
    public Func<T, Task>? OnClick { get; set; }
}
