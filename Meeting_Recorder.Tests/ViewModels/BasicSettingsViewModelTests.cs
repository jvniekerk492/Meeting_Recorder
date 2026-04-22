using DataRepository;
using Meeting_Recorder.Tests.TestDoubles;
using Meeting_Recorder.ViewModels;
using Xunit;

namespace Meeting_Recorder.Tests.ViewModels
{
    public sealed class BasicSettingsViewModelTests
    {
        [Fact]
        public void Constructor_Loads_Settings_From_Repository()
        {
            var repository = new InMemoryApplicationSettingsRepository();
            repository.Save(new ApplicationSettings
            {
                OutputFolder = @"C:\\Recordings",
                AudioQuality = "High",
                RecordingFormat = "wav"
            });

            var viewModel = new BasicSettingsViewModel(repository);

            Assert.Equal(@"C:\\Recordings", viewModel.OutputFolder);
            Assert.Equal("High", viewModel.SelectedAudioQuality);
            Assert.Equal("wav", viewModel.SelectedRecordingFormat);
        }

        [Fact]
        public void SaveCommand_Persists_Updated_Settings()
        {
            var repository = new InMemoryApplicationSettingsRepository();
            var viewModel = new BasicSettingsViewModel(repository)
            {
                OutputFolder = @"D:\\Audio",
                SelectedAudioQuality = "Low",
                SelectedRecordingFormat = "wav"
            };

            viewModel.SaveCommand.Execute(null);

            var savedSettings = repository.GetOrCreate();
            Assert.Equal(@"D:\\Audio", savedSettings.OutputFolder);
            Assert.Equal("Low", savedSettings.AudioQuality);
            Assert.Equal("wav", savedSettings.RecordingFormat);
            Assert.Equal("Settings saved", viewModel.StatusText);
        }
    }
}
