using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIModelInteractor
{
    public class LlamaModel : LanguageModelBase
    {
        private readonly string apiKey;
        public LlamaModel(string UrlToHost, string modelName, TimeSpan timeout, string apiKey) : base(UrlToHost, modelName, timeout)
        {
            //Currently the builder is sending the request to the wrong url /chat/completions so added the work around to send to /v1/chat/completions
            HttpClient.BaseAddress = new Uri(HttpClient.BaseAddress + "v1");
            this.apiKey = apiKey;
        }

        protected override void InnitModel()
        {
            Builder.AddOpenAIChatCompletion(
                modelId: ModelName, // Use the name of your loaded model in LM Studio
                apiKey: apiKey,
                httpClient: HttpClient
            );
            ProviderKernel = Builder.Build();
            ChatService = ProviderKernel.GetRequiredService<IChatCompletionService>();
        }
    }
}
