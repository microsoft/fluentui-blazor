using Bunit;
using Xunit;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.InputFile;
public class FluentInputFileTests : TestBase
{
    [Fact]
    public void FluentInputFile_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        bool multiple = default!;
        int maximumFileCount = default!;
        long maximumFileSize = default!;
        uint bufferSize = default!;
        string accept = default!;
        InputFileMode mode = default!;
        bool dragDropZoneVisible = default!;
        Action<InputFileChangeEventArgs> onInputFileChange = _ => { };
        Action<FluentInputFileEventArgs> onFileUploaded = _ => { };
        Action<FluentInputFileEventArgs> onProgressChange = _ => { };
        Action<FluentInputFileEventArgs> onFileError = _ => { };
        Action<IEnumerable<FluentInputFileEventArgs>> onCompleted = _ => { };
        string anchorId = default!;
        var cut = TestContext.RenderComponent<FluentInputFile>(parameters => parameters
            .Add(p => p.Multiple, multiple)
            .Add(p => p.MaximumFileCount, maximumFileCount)
            .Add(p => p.MaximumFileSize, maximumFileSize)
            .Add(p => p.BufferSize, bufferSize)
            .Add(p => p.Accept, accept)
            .Add(p => p.Mode, mode)
            .Add(p => p.DragDropZoneVisible, dragDropZoneVisible)
            .AddChildContent(childContent)
            .Add(p => p.OnInputFileChange, onInputFileChange)
            .Add(p => p.OnFileUploaded, onFileUploaded)
            .Add(p => p.OnProgressChange, onProgressChange)
            .Add(p => p.OnFileError, onFileError)
            .Add(p => p.OnCompleted, onCompleted)
            .Add(p => p.AnchorId, anchorId)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






