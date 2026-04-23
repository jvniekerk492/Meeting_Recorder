using Meeting_Recorder.Factories;
using Meeting_Recorder.Tests.Infrastructure;
using Meeting_Recorder.Tests.TestDoubles;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;
using Xunit;

namespace Meeting_Recorder.Tests.Views
{
    [Collection("WPF Application Collection")]
    public sealed class TranscribeMeetingViewTests
    {
        [StaFact]
        public void CreateView_Returns_TranscribeMeeting_View_With_Provided_ViewModel()
        {
            var repository = new InMemoryApplicationSettingsRepository();
            var viewModel = new TranscribeMeetingViewModel(repository);

            var view = ViewFactory.Instance.CreateView(ViewType.TranscribeMeeting, viewModel);

            Assert.IsType<TranscribeMeeting>(view);
            Assert.Same(viewModel, view.ViewModel);
            Assert.Same(viewModel, view.View.DataContext);
        }
    }
}
