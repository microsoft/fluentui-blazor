namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class FluentInputFileBuffer
{
    /// <summary />
    public FluentInputFileBuffer(byte[] data, int bytesRead)
    {
        Data = data;
        BytesRead = bytesRead;
    }

    /// <summary>
    /// Buffer data read.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Number of bytes read.
    /// </summary>
    public int BytesRead { get; }

    /// <summary>
    /// Append the current buffer (Data) to this file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task AppendToFileAsync(string file)
    {
        using var stream = new FileStream(file, FileMode.Append);
        await stream.WriteAsync(Data.AsMemory(0, BytesRead));
    }

    /// <summary>
    /// Append the current buffer (Data) to this file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public Task AppendToFileAsync(FileInfo file)
    {
        return AppendToFileAsync(file.FullName);
    }
}
