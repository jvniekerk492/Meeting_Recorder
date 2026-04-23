namespace AIModelInteractor
{
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.ChatCompletion;
    using System.Text;

    public abstract class LanguageModelBase
    {
        protected string ModelName { get; }
        protected Uri Host { get; }

        protected IKernelBuilder Builder { get; }
        protected IChatCompletionService? ChatService { get; set; }
        protected Kernel? ProviderKernel { get; set; }

        protected ChatHistory History { get; } = new ChatHistory();

        protected HttpClient HttpClient { get; }
        public LanguageModelBase(string UrlToHost, string modelName, TimeSpan timeout) {
            ModelName = modelName;
            Host = new Uri(UrlToHost);
            HttpClient = new HttpClient()
            {
                Timeout = timeout,
                BaseAddress = Host
            };
            Builder = Kernel.CreateBuilder();
        }

        protected abstract void InnitModel();

        public async Task<string> GetModelResponse(string prompt)
        {
            initChatService();
            if (prompt.IsWhiteSpace())
            {
                return string.Empty;
            }
            History.AddUserMessage(prompt);
            var response = await ChatService!.GetChatMessageContentsAsync(prompt);
            return ProcessResponse(response);
        }

        protected void initChatService()
        {
            if (ProviderKernel is null)
            {
                InnitModel();
            }
            if (ChatService is null)
            {
                ChatService = ProviderKernel!.GetRequiredService<IChatCompletionService>();
            }
        }

        public async Task<string> GetModelResponse(string prompt, string[] imagefilepaths)
        {
            initChatService();
            var images = new List<ImageContent>();
            var messagecontent = new ChatMessageContentItemCollection();
            messagecontent.Add(new Microsoft.SemanticKernel.TextContent(prompt));
            foreach (var imagefilepath in imagefilepaths)
            {
                byte[] imageBytes = File.ReadAllBytes(imagefilepath);
                messagecontent.Add(new Microsoft.SemanticKernel.ImageContent(imageBytes, "image/jpeg")); // Specify the correct MIME type
            }
            History.AddUserMessage(messagecontent);
            if (prompt.IsWhiteSpace())
            {
                return "Please let me know what task you want me to do.";
            }
            var response = await ChatService!.GetChatMessageContentsAsync(History);
            return ProcessResponse(response);
        }

        protected string ProcessResponse(IReadOnlyList<ChatMessageContent> responseStream)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var chunk in responseStream)
            {
                if (chunk.Content != null)
                {
                    Console.Write(chunk.Content);
                    stringBuilder.Append(chunk.Content);
                }
            }
            var textResponse = stringBuilder.ToString();
            History.AddAssistantMessage(textResponse);
            return textResponse;
        }
        public void ClearHistory()
        {
            History.Clear();
        }
    }
}
