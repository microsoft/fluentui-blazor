using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Resources;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;


namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentInputFile : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/InputFile/FluentInputFile.razor.js";

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
    /// Default value is 10
    /// </summary>
    [Parameter]
    public int MaximumFileCount { get; set; } = 10;

    /// <summary>
    /// Maximum size of a file to be uploaded (in bytes).
    /// Default value is 10 MB.
    /// </summary>
    [Parameter]
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Size of buffer to read bytes from uploaded file (in bytes).
    /// Default value is 10 KB.
    /// </summary>
    [Parameter]
    public uint BufferSize { get; set; } = 10 * 1024;

    /// <summary>
    /// Filter for what file types the user can pick from the file input dialog box.
    /// Example: ".gif, .jpg, .png, .doc", "audio/*", "video/*", "image/*"
    /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept</see>
    /// for more information.
    /// </summary>
    [Parameter]
    public string Accept { get; set; } = string.Empty;

    /// <summary>
    /// Type of file reading.
    /// For SaveToTemporaryFolder, use <see cref="FluentInputFileEventArgs.LocalFile" /> to retrieve the file.
    /// For Buffer, use <see cref="FluentInputFileEventArgs.Buffer" /> to retrieve bytes.
    /// </summary>
    [Parameter]
    public InputFileMode Mode { get; set; } = InputFileMode.SaveToTemporaryFolder;

    /// <summary>
    /// Drag/Drop zone visible or not. Default is true.
    /// You can.
    /// </summary>
    [Parameter]
    public bool DragDropZoneVisible { get; set; } = true;

    /// <summary>
    /// Current label display when an upload is in progress.
    /// </summary>
    public string ProgressTitle { get; private set; } = string.Empty;

    /// <summary>
    /// Current global value of the percentage of a current upload.
    /// </summary>
    public int ProgressPercent { get; private set; } = 0;

    /// <summary>
    /// Child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
    public EventCallback<FluentInputFileEventArgs> OnFileError { get; set; }

    /// <summary>
    /// Raise when all files are completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FluentInputFileEventArgs>> OnCompleted { get; set; }

    /// <summary>
    /// Identifier of the source component clickable by the end user.
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = string.Empty;

    public FluentInputFile()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Open the dialogbox to select files.
    /// Use <see cref="AnchorId"/> instead to specify the ID of the button (for example) on which the user should click.
    /// ⚠️ This method doesn't work on Safari and iOS.
    /// </summary>
    /// <returns></returns>
    public async Task ShowFilesDialogAsync()
    {
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);

        await Module.InvokeVoidAsync("raiseFluentInputFile", Id);
    }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(AnchorId))
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);

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
        ProgressPercent = 0;
        ProgressTitle = FluentInputFileResource.LoadingBefore;

        List<FluentInputFileEventArgs>? uploadedFiles = new();
        IReadOnlyList<IBrowserFile>? allFiles = e.GetMultipleFiles(MaximumFileCount);
        List<UploadedFileDetails>? allFilesSummary = allFiles.Select(i => (new UploadedFileDetails(i.Name, i.Size, i.ContentType))).ToList();
        long totalFileSizes = allFiles.Sum(i => i.Size);
        long totalRead = 0L;
        int fileNumber = 0;



        foreach (IBrowserFile file in allFiles)
        {
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
            ProgressTitle = string.Format(FluentInputFileResource.LoadingInProgress, fileNumber + 1, allFiles.Count, file.Name);
            fileDetails.ProgressTitle = ProgressTitle;


            switch (Mode)
            {
                case InputFileMode.Buffer:

                    // Read all buffers and raise the event OnProgressChange
                    await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, (buffer, bytesRead) =>
                    {
                        totalRead += bytesRead;

                        fileDetails.Buffer = new FluentInputFileBuffer(buffer, bytesRead);

                        ProgressPercent = Convert.ToInt32(decimal.Divide(totalRead, totalFileSizes) * 100);
                        return Task.CompletedTask;
                    });

                    break;

                case InputFileMode.SaveToTemporaryFolder:

                    // Save to temporary file
                    string? tempFileName = Path.GetTempFileName();
                    fileDetails.LocalFile = new FileInfo(tempFileName);

                    // Create a local file and write all read buffers
                    await using (FileStream writeStream = new(tempFileName, FileMode.Create))
                    {
                        await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, async (buffer, bytesRead) =>
                        {
                            totalRead += bytesRead;

                            await writeStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                            ProgressPercent = Convert.ToInt32(decimal.Divide(totalRead, totalFileSizes) * 100);
                        });
                    }

                    break;

                case InputFileMode.Stream:

                    long fileSizePart1 = file.Size / 2;
                    long fileSizePart2 = file.Size - fileSizePart1;

                    // Get a reference to the current file Stream
                    fileDetails.Stream = file.OpenReadStream(MaximumFileSize);

                    // Progression percent (first 50%)
                    totalRead += fileSizePart1;
                    ProgressPercent = Convert.ToInt32(decimal.Divide(totalRead, totalFileSizes) * 100);

                    // Uploaded event
                    if (OnFileUploaded.HasDelegate)
                    {
                        fileDetails.ProgressPercent = ProgressPercent;
                        await OnFileUploaded.InvokeAsync(fileDetails);
                    }

                    // Progression percent (last 50%)
                    totalRead += fileSizePart2;
                    ProgressPercent = Convert.ToInt32(decimal.Divide(totalRead, totalFileSizes) * 100);

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
            ProgressTitle = FluentInputFileResource.LoadingCanceled;
        }
        else
        {
            ProgressTitle = FluentInputFileResource.LoadingAfter;
        }

        ProgressPercent = 100;

        if (OnCompleted.HasDelegate)
        {
            await OnCompleted.InvokeAsync(uploadedFiles.ToArray());
        }
    }

    private async Task ReadFileToBufferAndRaiseProgressEventAsync(IBrowserFile file, FluentInputFileEventArgs fileDetails, Func<byte[], int, Task> action)
    {
        using Stream readStream = file.OpenReadStream(MaximumFileSize);
        int bytesRead = 0;
        byte[]? buffer = new byte[BufferSize];

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
}
