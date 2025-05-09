// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Localization;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a component for handling file uploads with support for drag-and-drop, progress tracking, and file
/// validation.
/// </summary>
/// <remarks>The <see cref="FluentInputFile"/> component provides a flexible and customizable way to upload files
/// in Blazor applications.  It supports multiple file uploads, file size and type validation, and progress tracking.
/// The component also includes events  for handling file upload progress, completion, and errors.  Use the <see
/// cref="Mode"/> property to specify the file reading mode, such as buffering, saving to a temporary folder, or
/// streaming. The <see cref="OnFileUploaded"/> and <see cref="OnCompleted"/> events can be used to handle file upload
/// completion.</remarks>
public partial class FluentInputFile : FluentComponentBase, IAsyncDisposable, IInputFileOptions
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "InputFile/FluentInputFile.razor.js";
    private ElementReference? _containerElement;
    private InputFile? _inputFile;
    private IJSObjectReference? _containerInstance;

    /// <summary />
    public FluentInputFile()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-inputfile-container")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("display", "none", when: () => !DragDropZoneVisible)
        .AddStyle("width", Width, when: () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: () => !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets or sets the component width.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the component width.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// To enable multiple file selection and upload, set the Multiple property to true.
    /// Set <see cref="MaximumFileCount"/> to change the number of allowed files.
    /// </summary>
    [Parameter]
    public bool Multiple { get; set; } = false;

    /// <summary>
    /// To select multiple files, set the maximum number of files allowed to be uploaded.
    /// Default value is 10.
    /// </summary>
    [Parameter]
    public int MaximumFileCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the maximum size of a file to be uploaded (in bytes).
    /// Default value is 10 MiB.
    /// </summary>
    [Parameter]
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the sze of buffer to read bytes from uploaded file (in bytes).
    /// Default value is 10 KiB.
    /// </summary>
    [Parameter]
    public uint BufferSize { get; set; } = 10 * 1024;

    /// <summary>
    /// Gets or sets the filter for what file types the user can pick from the file input dialog box.
    /// Example: ".gif, .jpg, .png, .doc", "audio/*", "video/*", "image/*"
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept</see>
    /// for more information.
    /// </summary>
    [Parameter]
    public string Accept { get; set; } = string.Empty;

    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the type of file reading.
    /// For SaveToTemporaryFolder, use <see cref="FluentInputFileEventArgs.LocalFile" /> to retrieve the file.
    /// For Buffer, use <see cref="FluentInputFileEventArgs.Buffer" /> to retrieve bytes.
    /// For Stream, use <see cref="FluentInputFileEventArgs.Stream"/> to have full control over retrieving the file.
    /// </summary>
    [Parameter]
    public InputFileMode Mode { get; set; } = InputFileMode.SaveToTemporaryFolder;

    /// <summary>
    /// Gets or sets a value indicating whether the Drag/Drop zone is visible.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool DragDropZoneVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the progress content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment<ProgressFileDetails>? ProgressTemplate { get; set; }

    /// <summary>
    /// Use the native event raised by the <seealso href="https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads">InputFile</seealso> component.
    /// If you use this event, the <see cref="OnFileUploaded"/> will never be raised.
    /// </summary>
    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnInputFileChange { get; set; }

    /// <summary>
    /// Raise when a file is completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileEventArgs> OnFileUploaded { get; set; }

    /// <summary>
    /// Raise when a progression step is updated.
    /// You can use <see cref="ProgressPercent"/> and <see cref="ProgressTitle"/> to have more detail on the progression.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileEventArgs> OnProgressChange { get; set; }

    /// <summary>
    /// Raise when a file raised an error.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileErrorEventArgs> OnFileError { get; set; }

    /// <summary>
    /// Raise when all files are completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FluentInputFileEventArgs>> OnCompleted { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the source component clickable by the end user.
    /// </summary>
    [Parameter]
    public string? AnchorId { get; set; }

    /// <summary>
    /// Gets the current label display when an upload is in progress.
    /// </summary>
    public string? ProgressTitle { get; private set; }

    /// <summary>
    /// Gets or sets the current global value of the percentage of a current upload.
    /// </summary>
    [Parameter]
    public int ProgressPercent { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the <see cref="ProgressPercent"/>.
    /// </summary>
    [Parameter]
    public EventCallback<int> ProgressPercentChanged { get; set; }

    /// <summary />
    private ProgressFileDetails ProgressFileDetails { get; set; }

    /// <summary />
    private string ProgressStyle => ProgressTemplate == null ? $"visibility: {(ProgressPercent > 0 ? "visible" : "hidden")};" : string.Empty;

    /// <summary>
    /// Open the dialog-box to select files.
    /// Use <see cref="AnchorId"/> instead to specify the ID of the button (for example) on which the user should click.
    /// ⚠️ This method doesn't work on Safari and iOS.
    /// </summary>
    /// <returns></returns>
    public async Task ShowFilesDialogAsync()
    {
        await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.InputFile.RaiseFluentInputFile", Id);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
            _containerInstance = await JSModule.ObjectReference.InvokeAsync<IJSObjectReference>("Microsoft.FluentUI.Blazor.InputFile.InitializeFileDropZone", _containerElement, _inputFile?.Element);
        }

        if (!string.IsNullOrEmpty(AnchorId) && _containerInstance is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.InputFile.AttachClickHandler", AnchorId, Id);
        }
    }

    /// <summary />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
    protected async Task OnUploadFilesHandlerAsync(InputFileChangeEventArgs e)
    {
        if (e.FileCount > MaximumFileCount && OnFileError.HasDelegate)
        {
            var err = FluentInputFileErrorEventArgs.FileCountExceeded;
            await OnFileError.InvokeAsync(new(err.Code, err.Message, fileCount: e.FileCount));
            return;
        }

        // Use the native Blazor event
        if (OnInputFileChange.HasDelegate)
        {
            await OnInputFileChange.InvokeAsync(e);
            return;
        }

        // Start
        await UpdateProgressAsync(0, Localizer[LanguageResource.FluentInputFile_LoadingBefore]);

        var uploadedFiles = new List<FluentInputFileEventArgs>();
        var allFiles = e.GetMultipleFiles(MaximumFileCount);
        var allFilesSummary = allFiles.Select(i => new UploadedFileDetails(i.Name, i.Size, i.ContentType)).ToList();
        var totalFileSizes = allFiles.Sum(i => i.Size);
        var totalRead = 0L;
        var fileNumber = 0;

        foreach (var file in allFiles)
        {
            ProgressFileDetails = new ProgressFileDetails(fileNumber, file.Name, 0);
            // Keep a trace of this file
            FluentInputFileEventArgs? fileDetails = new()
            {
                AllFiles = allFilesSummary,
                Index = fileNumber,
                Name = file.Name,
                ContentType = file.ContentType,
                Size = file.Size,
                LastModified = file.LastModified,
                IsCancelled = false,
            };
            uploadedFiles.Add(fileDetails);

            // Max size => ERROR
            if (file.Size > MaximumFileSize)
            {
                fileDetails.ErrorMessage = "The maximum size allowed is reached";

                if (OnFileError.HasDelegate)
                {
                    var err = FluentInputFileErrorEventArgs.MaximumSizeReached;
                    await OnFileError.InvokeAsync(new(err.Code, err.Message, fileName: file.Name));
                }

                continue;
            }

            // Progress
            var title = string.Format(CultureInfo.InvariantCulture, Localizer[LanguageResource.FluentInputFile_LoadingInProgress], fileNumber + 1, allFiles.Count, file.Name)
                     ?? string.Empty;
            fileDetails.ProgressTitle = title;

            switch (Mode)
            {
                case InputFileMode.Buffer:

                    // Read all buffers and raise the event OnProgressChange
                    await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, (buffer, bytesRead) =>
                    {
                        totalRead += bytesRead;

                        fileDetails.Buffer = new FluentInputFileBuffer(buffer, bytesRead);

                        return UpdateProgressAsync(totalRead, totalFileSizes, title);
                    });

                    break;

                case InputFileMode.SaveToTemporaryFolder:

                    // Save to temporary file
                    var tempFileName = Path.GetTempFileName();
                    fileDetails.LocalFile = new FileInfo(tempFileName);

                    // Create a local file and write all read buffers
                    await using (FileStream writeStream = new(tempFileName, FileMode.Create))
                    {
                        await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, async (buffer, bytesRead) =>
                        {
                            totalRead += bytesRead;

                            await writeStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                            await UpdateProgressAsync(totalRead, totalFileSizes, title);
                        });
                    }

                    break;

                case InputFileMode.Stream:

                    var fileSizePart1 = file.Size / 2;
                    var fileSizePart2 = file.Size - fileSizePart1;

                    // Get a reference to the current file Stream
                    fileDetails.Stream = file.OpenReadStream(MaximumFileSize);

                    // Progression percent (first 50%)
                    totalRead += fileSizePart1;
                    await UpdateProgressAsync(totalRead, totalFileSizes, title);

                    // Uploaded event
                    if (OnFileUploaded.HasDelegate)
                    {
                        fileDetails.ProgressPercent = ProgressPercent;
                        await OnFileUploaded.InvokeAsync(fileDetails);
                    }

                    // Progression percent (last 50%)
                    totalRead += fileSizePart2;
                    await UpdateProgressAsync(totalRead, totalFileSizes, title);

                    break;

                default:
                    throw new ArgumentException("Invalid Mode value.");
            }

            if (fileDetails.IsCancelled)
            {
                break;
            }

            fileNumber++;
        }

        // Canceled or Completed
        if (uploadedFiles.Any(i => i.IsCancelled))
        {
            await UpdateProgressAsync(100, Localizer[LanguageResource.FluentInputFile_LoadingCanceled]);
        }
        else
        {
            await UpdateProgressAsync(100, Localizer[LanguageResource.FluentInputFile_LoadingCompleted]);
        }

        if (OnCompleted.HasDelegate)
        {
            await OnCompleted.InvokeAsync(uploadedFiles.ToArray());
        }
    }

    private async Task ReadFileToBufferAndRaiseProgressEventAsync(IBrowserFile file, FluentInputFileEventArgs fileDetails, Func<byte[], int, Task> action)
    {
        using var readStream = file.OpenReadStream(MaximumFileSize);
        var bytesRead = 0;
        var buffer = new byte[BufferSize];

        // Read file
        while ((bytesRead = await readStream.ReadAsync(buffer)) != 0)
        {
            await action(buffer, bytesRead);

            if (ProgressPercent <= 0)
            {
                ProgressPercent = 1;
            }

            if (OnProgressChange.HasDelegate)
            {
                fileDetails.ProgressPercent = ProgressPercent;
                await OnProgressChange.InvokeAsync(fileDetails);

                if (fileDetails.IsCancelled)
                {
                    break;
                }
            }

            StateHasChanged();
        }

        // Uploaded event
        if (OnFileUploaded.HasDelegate)
        {
            fileDetails.ProgressPercent = ProgressPercent;
            await OnFileUploaded.InvokeAsync(fileDetails);
        }
    }

    /// <summary />
    private Task UpdateProgressAsync(long current, long size, string title)
    {
        return UpdateProgressAsync(Convert.ToInt32(decimal.Divide(current, size <= 0 ? 1 : size) * 100), title);
    }

    /// <summary />
    private async Task UpdateProgressAsync(int percent, string title)
    {
        if (ProgressPercent != percent)
        {
            ProgressPercent = percent;

            if (ProgressPercentChanged.HasDelegate)
            {
                await ProgressPercentChanged.InvokeAsync(percent);
            }
        }

        if (!string.Equals(ProgressTitle, title, StringComparison.Ordinal))
        {
            ProgressTitle = title;
        }
    }

    /// <summary>
    /// Unregister the drop zone events
    /// </summary>
    /// <param name="jsModule"></param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        if (_containerInstance is not null)
        {
            await _containerInstance.InvokeVoidAsync("dispose");
            await _containerInstance.DisposeAsync().ConfigureAwait(false);
        }
    }
}
