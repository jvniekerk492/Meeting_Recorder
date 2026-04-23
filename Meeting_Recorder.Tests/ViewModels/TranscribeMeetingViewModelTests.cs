using DataRepository;
using Meeting_Recorder.Tests.TestDoubles;
using Meeting_Recorder.ViewModels;
using Xunit;

namespace Meeting_Recorder.Tests.ViewModels
{
    public sealed class TranscribeMeetingViewModelTests
    {
        [Fact]
        public void Constructor_Throws_When_Repository_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new TranscribeMeetingViewModel(null!));
        }

        [Fact]
        public void Constructor_Succeeds_With_Valid_Repository()
        {
            var repository = new InMemoryApplicationSettingsRepository();

            var viewModel = new TranscribeMeetingViewModel(repository);

            Assert.NotNull(viewModel);
        }
    }
}
