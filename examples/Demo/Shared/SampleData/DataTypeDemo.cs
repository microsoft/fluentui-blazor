using System.ComponentModel.DataAnnotations;

namespace FluentUI.Demo.Shared.SampleData;

public class DataTypeDemoBase
{
    public sbyte TinyInteger { get; set; }
    public short SmallInteger { get; set; }
    public int Integer { get; set; }
    public long LongInteger { get; set; }
    public float SinglePrecision { get; set; }
    public double DoublePrecision { get; set; }
    public decimal Decimal { get; set; }
    public bool Boolean { get; set; }
    public char Char { get; set; }
    public string String { get; set; } = default!;
    public DateTime DateTime { get; set; }
    public DateOnly DateOnly { get; set; }
    public TimeOnly TimeOnly { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; }
    public DataTypeDemoEnum Enum { get; set; }
}

public class DataTypeDemo : DataTypeDemoBase
{
    public DataTypeDemoBase Children { get; set; } = new();
}

public enum DataTypeDemoEnum : int
{
    Value0 = 0,

    [Display(Name = "Value 1")]
    Value1 = 1,

    Value2 = 2,
    Value3 = 3,
    Value4 = 4,
    Value5 = 5,
    Value6 = 6,
}
