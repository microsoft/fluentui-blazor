namespace FluentUI.Demo.Shared.Pages.Emoji;

public partial class EmojiPage
{
    /*
        [Inject]
        private ILogger<EmojiPage> Logger { get; set; } = default!;

        private IJSObjectReference? _jsModule;
        private EditContext? editContext;
        private List<EmojiModel>? emojis = new();
        private int totalCount = 0;
        private int maxResults = 50;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                     "./_content/FluentUI.Demo.Shared/Pages/Emoji/EmojiPage.razor.js");
            }
        }

        public void HandleSize(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value?.ToString()))
                Form.Size = Enum.Parse<EmojiSize>((string)args.Value);
            else
                Form.Size = EmojiSize.Size32;

            HandleSearch();
        }

        public void HandleSkintone(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
                Form.Skintone = Enum.Parse<EmojiSkintone>((string)args.Value!);
            else
                Form.Skintone = null;

            HandleSearch();
        }

        public void HandleStyle(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
                Form.Style = Enum.Parse<EmojiStyle>((string)args.Value!);
            else
                Form.Style = null;

            HandleSearch();
        }

        public void HandleGroup(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value?.ToString()) && args.Value?.ToString() != "-1")
                Form.Group = Enum.Parse<EmojiGroup>((string)args.Value!);
            else
                Form.Group = null;

            HandleSearch();
        }

        public void HandleSearch()
        {
            emojis = null;


            FluentEmojiSearcher searcher = new(EmojiService);

            emojis = searcher.WithName(Form.Searchterm)
                .InGroup(Form.Group)
                .WithStyle(Form.Style)
                .WithSkintone(Form.Skintone)
                .ToList(maxResults);

            totalCount = searcher.ResultCount();

            StateHasChanged();
        }

        public void HandleReset()
        {
            Form.Searchterm = "";
            Form.Size = EmojiSize.Size32;
            Form.Group = null;
            Form.Style = null;
            Form.Skintone = null;

            if (emojis is not null)
                emojis.Clear();
        }

        public async void HandleClick(EmojiModel emoji)
        {
            Logger.LogInformation($"You clicked on {emoji.Name}");

            string skintone = emoji.Skintone is not null ? $"Skintone=@EmojiSkintone.{emoji.Skintone}" : string.Empty;

            string Text = $$"""<FluentEmoji Name="@FluentEmojis.{{emoji.Name}}" Size="@EmojiSize.{{Form.Size}}" Style="@EmojiStyle.{{emoji.Style}}" {{skintone}}/>""";

            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("copyText", Text);
            }
        }

        public class FormModel
        {
            public string? Searchterm { get; set; }

            public EmojiGroup? Group { get; set; }

            public EmojiSize Size { get; set; }

            public EmojiStyle? Style { get; set; }

            public EmojiSkintone? Skintone { get; set; }

        }

        private FormModel Form = new() { Searchterm = "", Size = EmojiSize.Size32 };

        protected override void OnInitialized()
        {
            editContext = new EditContext(Form);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_jsModule is not null)
                {
                    await _jsModule.DisposeAsync();
                }
            }
            catch (JSDisconnectedException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
    */
}
