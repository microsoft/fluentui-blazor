// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

/// <summary />
public static class Identifier
{
    private const int RANGE_FROM = 0x10000000;          // The smallest value to generate a number that will be converted to an 8-character string.
    private const int RANGE_TO = int.MaxValue;          // The largest  value to generate a number that will be converted to an 8-character string.
    private static readonly Random _rnd = new();

    /// <summary>
    /// Returns a new <see cref="IdentifierContext"/> where ID are sequential: "f0000", "f0001", "f0002", ...
    /// </summary>
    /// <returns></returns>
    public static IdentifierContext SequentialContext() => new((n) => $"f{n:0000}");

    /// <summary>
    /// Returns a new small Id (8 chars).
    /// HTML id must start with a letter.
    /// Example: f127d9ed
    /// </summary>
    /// <remarks>
    /// You can use a <see cref="IdentifierContext"/> instance to customize the Generation process,
    /// for example in Unit Tests.
    /// </remarks>
    /// <returns></returns>
    public static string NewId()
    {
        const int LENGTH = 8;

        if (IdentifierContext.Current == null)
        {
            var rnd = _rnd.Next(RANGE_FROM, RANGE_TO);
            return $"f{rnd:x}"[..LENGTH];
        }

        return IdentifierContext.Current.GenerateId();
    }

    /// <summary>
    /// Returns a new small Id.
    /// HTML id must start with a letter.
    /// Example: f127d9edf14385adb
    /// </summary>
    /// <remarks>
    /// You can use a <see cref="IdentifierContext"/> instance to customize the Generation process,
    /// for example in Unit Tests.
    /// </remarks>
    /// <param name="length">The length of the identifier.</param>
    /// <returns></returns>
    public static string NewId(int length)
    {
        if (IdentifierContext.Current == null)
        {
            if (length > 16)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "length must be less than 16");
            }

            var rnd = _rnd.Next(RANGE_FROM, RANGE_TO);

            if (length <= 8)
            {
                return $"f{rnd:x}"[..length];
            }

            return $"f{rnd:x}{rnd:x}"[..length];
        }

        return IdentifierContext.Current.GenerateId();
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
