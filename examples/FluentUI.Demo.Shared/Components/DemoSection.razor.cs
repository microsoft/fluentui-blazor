// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

namespace FluentUI.Demo.Shared.Components
{
    using System.Reflection.Metadata;
    //using Microsoft.Reality.Components.Blazor.Services;
    using Microsoft.AspNetCore.Components;

    public partial class DemoSection : ComponentBase
    {
        //[Inject]
        //public ILocalStorageService LocalStorageService { get; set; } = default!;

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public RenderFragment? Description { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        //No .razor needed
        public string? CodeFilename { get; set; }

        [Parameter]
        public bool IsNew { get; set; }

        private bool HasCode { get; set; }

        private string? CodeContents { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //var isDisplayDemoCode = await LocalStorageService.GetPropertyAsync<bool>("IsDisplayDemoCode", defaultValue: true);
                var hasFilename = !string.IsNullOrEmpty(CodeFilename);

                this.HasCode = hasFilename; //&& isDisplayDemoCode;

                if (HasCode)
                {
                    SetCodeContents();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected void SetCodeContents()
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                var allFiles = Directory.GetFiles(currentDir, "*.razor", SearchOption.AllDirectories);
                var filePath = allFiles.FirstOrDefault(x => x.Contains($"{CodeFilename}.razor"));

                if (!File.Exists(filePath))
                {
                    return;
                }

                CodeContents = File.ReadAllText(filePath);
                StateHasChanged();
            }
            catch
            {
                //Do Nothing
            }
        }
    }
}
