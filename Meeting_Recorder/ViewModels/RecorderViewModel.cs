using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using AudioManager;

namespace Meeting_Recorder.ViewModels
{
    public sealed class RecorderViewModel : ViewModelBase, IDisposable
    {
        private readonly IAudioRecorder audioRecorder;
        private readonly DispatcherTimer elapsedTimer;
        private string statusText = "Ready";
        private string elapsedTime = "00:00:00";
        private string outputFilePath = string.Empty;
        private bool isRecording;
        private bool disposed;

        public RecorderViewModel(IAudioRecorder audioRecorder)
        {
            this.audioRecorder = audioRecorder ?? throw new ArgumentNullException(nameof(audioRecorder));

            this.elapsedTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            this.elapsedTimer.Tick += OnElapsedTimerTick;

            StartCommand = new RelayCommand(ExecuteStart, CanExecuteStart);
            StopCommand = new RelayCommand(ExecuteStop, CanExecuteStop);
        }

        public string StatusText
        {
            get => this.statusText;
            private set => SetProperty(ref this.statusText, value);
        }

        public string ElapsedTime
        {
            get => this.elapsedTime;
            private set => SetProperty(ref this.elapsedTime, value);
        }

        public string OutputFilePath
        {
            get => this.outputFilePath;
            set => SetProperty(ref this.outputFilePath, value);
        }

        public bool IsRecording
        {
            get => this.isRecording;
            private set
            {
                if (SetProperty(ref this.isRecording, value))
                {
                    ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)StopCommand).RaiseCanExecuteChanged();
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
            if (string.IsNullOrWhiteSpace(this.outputFilePath))
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                OutputFilePath = Path.Combine(documentsPath, $"Recording_{DateTime.Now:yyyyMMdd_HHmmss}.wav");
            }

            this.audioRecorder.StartRecording(this.outputFilePath);
            IsRecording = true;
            StatusText = "Recording...";
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
            IsRecording = false;
            StatusText = $"Saved to {this.outputFilePath}";
        }

        private void OnElapsedTimerTick(object? sender, EventArgs e)
        {
            ElapsedTime = this.audioRecorder.Elapsed.ToString(@"hh\:mm\:ss");
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
