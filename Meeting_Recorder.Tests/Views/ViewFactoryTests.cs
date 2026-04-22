using AudioManager;
using Meeting_Recorder.Factories;
using Meeting_Recorder.Tests.Infrastructure;
using Meeting_Recorder.Tests.TestDoubles;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;
using Xunit;

namespace Meeting_Recorder.Tests.Views
{
    [Collection("WPF Application Collection")]
    public sealed class ViewFactoryTests
    {
        [Fact]
        public void Instance_Returns_Same_Factory()
        {
            Assert.Same(ViewFactory.Instance, ViewFactory.Instance);
        }

        [StaFact]
        public void CreateView_Returns_Recorder_View_With_Provided_ViewModel()
        {
            using var viewModel = new RecorderViewModel(new AudioRecorder());

            var view = ViewFactory.Instance.CreateView(ViewType.Recorder, viewModel);

            Assert.IsType<Recorder>(view);
            Assert.Same(viewModel, view.ViewModel);
            Assert.Same(viewModel, view.View.DataContext);
        }

        [StaFact]
        public void CreateView_Returns_BasicSettings_View_With_Provided_ViewModel()
        {
            var repository = new InMemoryApplicationSettingsRepository();
            var viewModel = new BasicSettingsViewModel(repository);

            var view = ViewFactory.Instance.CreateView(ViewType.BasicSettings, viewModel);

            Assert.IsType<BasicSettings>(view);
            Assert.Same(viewModel, view.ViewModel);
            Assert.Same(viewModel, view.View.DataContext);
        }

        [Fact]
        public void CreateView_Throws_For_Unregistered_ViewType()
        {
            using var viewModel = new RecorderViewModel(new AudioRecorder());

            Assert.Throws<NotSupportedException>(() => ViewFactory.Instance.CreateView((ViewType)999, viewModel));
        }
    }
}
