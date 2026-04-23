using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using AudioManager;
using DataRepository;

namespace Meeting_Recorder.ViewModels
{
    public sealed class RecorderViewModel : ViewModelBase, IDisposable
    {
        private readonly IAudioRecorder audioRecorder;
        private readonly IApplicationSettingsRepository? applicationSettingsRepository;
        private readonly IRecordingSessionRepository? recordingSessionRepository;
        private readonly DispatcherTimer elapsedTimer;
        private string statusText = "Ready";
        private string elapsedTime = "00:00:00";
        private string outputFilePath = string.Empty;
        private bool isRecording;
        private bool disposed;
        private DateTime recordingStartedAt;

        public RecorderViewModel(IAudioRecorder audioRecorder)
            : this(audioRecorder, null, null)
        {
        }

        public RecorderViewModel(IAudioRecorder audioRecorder, IApplicationSettingsRepository? applicationSettingsRepository)
            : this(audioRecorder, applicationSettingsRepository, null)
        {
        }

        public RecorderViewModel(IAudioRecorder audioRecorder, IApplicationSettingsRepository? applicationSettingsRepository, IRecordingSessionRepository? recordingSessionRepository)
        {
            this.audioRecorder = audioRecorder ?? throw new ArgumentNullException(nameof(audioRecorder));
            this.applicationSettingsRepository = applicationSettingsRepository;
            this.recordingSessionRepository = recordingSessionRepository;
            this.elapsedTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            this.elapsedTimer.Tick += this.OnElapsedTimerTick;
            this.StartCommand = new RelayCommand(this.ExecuteStart, this.CanExecuteStart);
            this.StopCommand = new RelayCommand(this.ExecuteStop, this.CanExecuteStop);
        }

        public string StatusText
        {
            get => this.statusText;
            private set => this.SetProperty(ref this.statusText, value);
        }

        public string ElapsedTime
        {
            get => this.elapsedTime;
            private set => this.SetProperty(ref this.elapsedTime, value);
        }

        public string OutputFilePath
        {
            get => this.outputFilePath;
            set => this.SetProperty(ref this.outputFilePath, value);
        }

        public DateTime RecordingStartedAt => this.recordingStartedAt;

        public bool IsRecording
        {
            get => this.isRecording;
            private set
            {
                if (this.SetProperty(ref this.isRecording, value))
                {
                    ((RelayCommand)this.StartCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)this.StopCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        private bool CanExecuteStart()
        {
            return !this.isRecording;
        }

        private void ExecuteStart()
        {
            if (string.IsNullOrWhiteSpace(this.OutputFilePath))
            {
                var settings = this.applicationSettingsRepository?.GetOrCreate();
                var outputFolder = settings?.OutputFolder;

                if (string.IsNullOrWhiteSpace(outputFolder))
                {
                    outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                var recordingFormat = settings?.RecordingFormat;

                if (string.IsNullOrWhiteSpace(recordingFormat))
                {
                    recordingFormat = "wav";
                }

                if (!recordingFormat.StartsWith('.'))
                {
                    recordingFormat = $".{recordingFormat}";
                }

                this.OutputFilePath = Path.Combine(outputFolder, $"Recording_{DateTime.Now:yyyyMMdd_HHmmss}{recordingFormat}");
            }

            this.recordingStartedAt = DateTime.Now;
            this.audioRecorder.StartRecording(this.outputFilePath);
            this.IsRecording = true;
            this.StatusText = "Recording...";
            this.elapsedTimer.Start();
        }

        private bool CanExecuteStop()
        {
            return this.isRecording;
        }

        private void ExecuteStop()
        {
            this.elapsedTimer.Stop();
            this.audioRecorder.StopRecording();
            this.IsRecording = false;
            this.StatusText = $"Saved to {this.outputFilePath}";
            this.SaveRecordingSession();
        }

        private void SaveRecordingSession()
        {
            if (this.recordingSessionRepository == null)
            {
                return;
            }

            this.recordingSessionRepository.Save(new RecordingSession
            {
                FilePath = this.outputFilePath,
                FileName = Path.GetFileName(this.outputFilePath),
                StartedAt = this.recordingStartedAt
            });
        }

        private void OnElapsedTimerTick(object? sender, EventArgs e)
        {
            this.ElapsedTime = this.audioRecorder.Elapsed.ToString(@"hh\:mm\:ss");
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.elapsedTimer.Stop();
                this.audioRecorder.Dispose();
                this.disposed = true;
            }
        }
    }
}
