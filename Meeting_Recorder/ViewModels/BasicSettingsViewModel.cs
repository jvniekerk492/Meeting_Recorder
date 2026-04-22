using System.Collections.ObjectModel;
using System.Windows.Input;
using DataRepository;

namespace Meeting_Recorder.ViewModels
{
    public sealed class BasicSettingsViewModel : ViewModelBase
    {
        private readonly IApplicationSettingsRepository applicationSettingsRepository;
        private string outputFolder = string.Empty;
        private string selectedAudioQuality = "Standard";
        private string selectedRecordingFormat = "wav";
        private string statusText = "Ready";

        public BasicSettingsViewModel(IApplicationSettingsRepository applicationSettingsRepository)
        {
            this.applicationSettingsRepository = applicationSettingsRepository ?? throw new ArgumentNullException(nameof(applicationSettingsRepository));
            this.AudioQualityOptions = new ObservableCollection<string>
            {
                "Low",
                "Standard",
                "High"
            };
            this.RecordingFormatOptions = new ObservableCollection<string>
            {
                "wav"
            };
            this.SaveCommand = new RelayCommand(this.ExecuteSave);
            this.LoadSettings();
        }

        public ObservableCollection<string> AudioQualityOptions { get; }

        public ObservableCollection<string> RecordingFormatOptions { get; }

        public string OutputFolder
        {
            get => this.outputFolder;
            set => this.SetProperty(ref this.outputFolder, value);
        }

        public string SelectedAudioQuality
        {
            get => this.selectedAudioQuality;
            set => this.SetProperty(ref this.selectedAudioQuality, value);
        }

        public string SelectedRecordingFormat
        {
            get => this.selectedRecordingFormat;
            set => this.SetProperty(ref this.selectedRecordingFormat, value);
        }

        public string StatusText
        {
            get => this.statusText;
            private set => this.SetProperty(ref this.statusText, value);
        }

        public ICommand SaveCommand { get; }

        private void LoadSettings()
        {
            var applicationSettings = this.applicationSettingsRepository.GetOrCreate();
            this.OutputFolder = applicationSettings.OutputFolder;
            this.SelectedAudioQuality = applicationSettings.AudioQuality;
            this.SelectedRecordingFormat = applicationSettings.RecordingFormat;
        }

        private void ExecuteSave()
        {
            this.applicationSettingsRepository.Save(new ApplicationSettings
            {
                OutputFolder = this.outputFolder,
                AudioQuality = this.selectedAudioQuality,
                RecordingFormat = this.selectedRecordingFormat
            });
            this.StatusText = "Settings saved";
        }
    }
}
