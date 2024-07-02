namespace FluentUI.Demo.Shared.SampleData;

public class DataTypeDemo
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
}
