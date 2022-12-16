using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Demo.Shared;

public class StringLengthComparer : IComparer<string>
{
    public static readonly StringLengthComparer Instance = new StringLengthComparer();

    public int Compare(string? x, string? y)
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }

        if (y is null)
        {
            return 1;
        }

        return x.Length.CompareTo(y.Length);
    }
}
