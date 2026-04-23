using AIModelInteractor;
using Xunit;

namespace Meeting_Recorder.Tests.InferenceService
{
    public sealed class OllamaModelTests
    {
        private const string TestUrl = "http://localhost:11434/";
        private const string TestModel = "llama3";

        [Fact]
        public void Constructor_Creates_Instance_Inheriting_LanguageModelBase()
        {
            var model = new OllamaModel(TestUrl, TestModel, TimeSpan.FromSeconds(30));

            Assert.IsAssignableFrom<LanguageModelBase>(model);
        }

        [Fact]
        public void ClearHistory_Does_Not_Throw()
        {
            var model = new OllamaModel(TestUrl, TestModel, TimeSpan.FromSeconds(30));

            var exception = Record.Exception(() => model.ClearHistory());

            Assert.Null(exception);
        }

        [Fact]
        public async Task GetModelResponse_Returns_EmptyString_For_WhiteSpacePrompt()
        {
            var model = new OllamaModel(TestUrl, TestModel, TimeSpan.FromSeconds(1));

            var result = await model.GetModelResponse("   ");

            Assert.Equal(string.Empty, result);
        }
    }
}
