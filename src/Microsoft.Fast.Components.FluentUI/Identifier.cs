namespace Microsoft.Fast.Components.FluentUI;


/// <summary />
public static class Identifier
{
    private static readonly Random _rnd = new Random();

    /// <summary>
    /// Returns a new small Id.
    /// HTML id must start with a letter.
    /// Example: f127d9edf14385adb
    /// </summary>
    /// <returns></returns>
    public static string NewId()
    {
        return $"f{_rnd.Next():x}{_rnd.Next():x}";
    }

    /* There are several solutions to generate a small identifier.
       Here you will find examples and time estimates.
       ----------------------------------------------------------------------

       BenchmarkDotNet=v0.13.1, OS=Windows 11
       Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
       DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
    
       | Method           | Example                              |  Mean     |
       |------------------|--------------------------------------|-----------|
       | From Rnd Long:     10aad4a0                                51.14 ns |
       | From Rnd Int:      4a9d0f06                                51.59 ns |
   --> | From Rnd Long:     6aa977f03314e4b4                       115.12 ns |
       | From Ticks:        8da188a929219fab                       174.41 ns |
       | From Guid Base64:  aRk3NU0b0ka04Oqc                       206.67 ns |
       | From Guid:         25ee3842-0c33-4a63-9b83-a7d1c0266809   224.59 ns |
       | From Guid Shorted: 6ee0e232196b4a9                        237.51 ns | 
     
       PS: Ticks does not always generate unique values in WASM apps.

     */
}
