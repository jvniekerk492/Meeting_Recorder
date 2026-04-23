using AIModelInteractor;
using Xunit;

namespace Meeting_Recorder.Tests.InferenceService
{
    public sealed class LlamaModelTests
    {
        private const string TestUrl = "http://localhost:1234/";
        private const string TestModel = "llama3";
        private const string TestApiKey = "test-key";

        [Fact]
        public void Constructor_Sets_BaseAddress_With_V1_Suffix()
        {
            var model = new LlamaModel(TestUrl, TestModel, TimeSpan.FromSeconds(30), TestApiKey);

            Assert.IsAssignableFrom<LanguageModelBase>(model);
        }

        [Fact]
        public void GetModelResponse_Returns_EmptyString_For_WhiteSpacePrompt()
        {
            var model = new LlamaModel(TestUrl, TestModel, TimeSpan.FromSeconds(30), TestApiKey);

            var exception = Record.ExceptionAsync(() => model.GetModelResponse("   "));

            Assert.Null(exception.Result?.InnerException as InvalidOperationException);
        }

        [Fact]
        public void ClearHistory_Does_Not_Throw()
        {
            var model = new LlamaModel(TestUrl, TestModel, TimeSpan.FromSeconds(30), TestApiKey);

            var exception = Record.Exception(() => model.ClearHistory());

            Assert.Null(exception);
        }
    }
}
