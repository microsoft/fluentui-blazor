using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentInputFile : FluentComponentBase
{
    public static string ResourceLoadingBefore = "Loading...";
    public static string ResourceLoadingCompleted = "Completed";
    public static string ResourceLoadingCanceled = "Canceled";
    public static string ResourceLoadingInProgress = "Loading {0}/{1} - {2}";

    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/InputFile/FluentInputFile.razor.js";

    /// <summary />
    public FluentInputFile()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary />
    private bool DropOver { get; set; } = false;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-inputfile-container")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("display", "none", () => !DragDropZoneVisible)
        .Build();

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
    /// Default value is 10 MB.
    /// </summary>
    [Parameter]
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the sze of buffer to read bytes from uploaded file (in bytes).
    /// Default value is 10 KB.
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
    /// Raise when a file raised an error. Not yet used.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileEventArgs> OnFileError { get; set; }

    /// <summary>
    /// Raise when all files are completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FluentInputFileEventArgs>> OnCompleted { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the source component clickable by the end user.
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the current label display when an upload is in progress.
    /// </summary>
    public string ProgressTitle { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current global value of the percentage of a current upload.
    /// </summary>
    [Parameter]
    public int ProgressPercent { get; set; } = 0;

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
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));

        await Module.InvokeVoidAsync("raiseFluentInputFile", Id);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !string.IsNullOrEmpty(AnchorId))
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));

            await Module.InvokeVoidAsync("attachClickHandler", AnchorId, Id);
        }
    }

    /// <summary />
    protected async Task OnUploadFilesHandlerAsync(InputFileChangeEventArgs e)
    {
        if (e.FileCount > MaximumFileCount)
        {
            throw new ApplicationException($"The maximum number of files accepted is {MaximumFileCount}, but {e.FileCount} were supplied.");
        }

        DropOver = false;

        // Use the native Blazor event
        if (OnInputFileChange.HasDelegate)
        {
            await OnInputFileChange.InvokeAsync(e);
            return;
        }

        // Start
        await UpdateProgressAsync(0, ResourceLoadingBefore);

        List<FluentInputFileEventArgs>? uploadedFiles = [];
        IReadOnlyList<IBrowserFile>? allFiles = e.GetMultipleFiles(MaximumFileCount);
        var allFilesSummary = allFiles.Select(i => new UploadedFileDetails(i.Name, i.Size, i.ContentType)).ToList();
        var totalFileSizes = allFiles.Sum(i => i.Size);
        var totalRead = 0L;
        var fileNumber = 0;

        foreach (IBrowserFile file in allFiles)
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
                IsCancelled = false,
            };
            uploadedFiles.Add(fileDetails);

            // Max size => ERROR
            if (file.Size > MaximumFileSize)
            {
                fileDetails.ErrorMessage = "The maximum size allowed is reached";
                continue;
            }

            // Progress
            var title = string.Format(ResourceLoadingInProgress, fileNumber + 1, allFiles.Count, file.Name) ?? string.Empty;
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
            await UpdateProgressAsync(100, ResourceLoadingCanceled);
        }
        else
        {
            await UpdateProgressAsync(100, ResourceLoadingCompleted);
        }

        if (OnCompleted.HasDelegate)
        {
            await OnCompleted.InvokeAsync(uploadedFiles.ToArray());
        }
    }

    private async Task ReadFileToBufferAndRaiseProgressEventAsync(IBrowserFile file, FluentInputFileEventArgs fileDetails, Func<byte[], int, Task> action)
    {
        using Stream readStream = file.OpenReadStream(MaximumFileSize);
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

    private Task UpdateProgressAsync(long current, long size, string title)
    {
        return UpdateProgressAsync(Convert.ToInt32(decimal.Divide(current, size) * 100), title);
    }

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

        if (ProgressTitle != title)
        {
            ProgressTitle = title;
        }
    }
}
