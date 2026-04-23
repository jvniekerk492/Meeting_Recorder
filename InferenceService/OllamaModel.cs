using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;
using System.Threading.Tasks;

namespace AIModelInteractor
{
    public class OllamaModel : LanguageModelBase
    {
        public OllamaModel(string UrlToHost, string modelName, TimeSpan timeout)
            :base(UrlToHost, modelName, timeout)
        {
        }
        protected override void InnitModel()
        {
            Builder.Services.AddKeyedSingleton<IChatCompletionService>("OllamaModel", (serviceProvider, _) =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

                var ollamaClient = (IChatClient)new OllamaApiClient(HttpClient, defaultModel: ModelName);

                var builder = ollamaClient.AsBuilder();
                if (loggerFactory is not null)
                {
                    builder.UseLogging(loggerFactory);
                }

                return builder
                    .UseKernelFunctionInvocation(loggerFactory)
                    .Build(serviceProvider)
                    .AsChatCompletionService();
            });
            ProviderKernel = Builder.Build();
        }
    }
}
