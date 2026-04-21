using AudioManager;
using Meeting_Recorder.ViewModels;
using Moq;
using Xunit;

namespace Meeting_Recorder.Tests.ViewModels
{
    public sealed class RecorderViewModelTests
    {
        private readonly Mock<IAudioRecorder> mockRecorder;

        public RecorderViewModelTests()
        {
            this.mockRecorder = new Mock<IAudioRecorder>();
        }

        [StaFact]
        public void Initial_State_Is_Not_Recording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            Assert.False(viewModel.IsRecording);
            Assert.Equal("Ready", viewModel.StatusText);
            Assert.Equal("00:00:00", viewModel.ElapsedTime);
        }

        [StaFact]
        public void StartCommand_Can_Execute_When_Not_Recording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            Assert.True(viewModel.StartCommand.CanExecute(null));
        }

        [StaFact]
        public void StopCommand_Cannot_Execute_When_Not_Recording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            Assert.False(viewModel.StopCommand.CanExecute(null));
        }

        [StaFact]
        public void StartCommand_Sets_IsRecording_True()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            viewModel.StartCommand.Execute(null);

            Assert.True(viewModel.IsRecording);
        }

        [StaFact]
        public void StartCommand_Updates_StatusText()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            viewModel.StartCommand.Execute(null);

            Assert.Equal("Recording...", viewModel.StatusText);
        }

        [StaFact]
        public void StartCommand_Calls_AudioRecorder_StartRecording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            viewModel.StartCommand.Execute(null);

            this.mockRecorder.Verify(r => r.StartRecording(It.IsAny<string>()), Times.Once);
        }

        [StaFact]
        public void StopCommand_Sets_IsRecording_False()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            viewModel.StartCommand.Execute(null);

            viewModel.StopCommand.Execute(null);

            Assert.False(viewModel.IsRecording);
        }

        [StaFact]
        public void StopCommand_Calls_AudioRecorder_StopRecording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            viewModel.StartCommand.Execute(null);

            viewModel.StopCommand.Execute(null);

            this.mockRecorder.Verify(r => r.StopRecording(), Times.Once);
        }

        [StaFact]
        public void StopCommand_StatusText_Contains_FilePath()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            viewModel.OutputFilePath = @"C:\test\recording.wav";
            viewModel.StartCommand.Execute(null);

            viewModel.StopCommand.Execute(null);

            Assert.Contains(@"C:\test\recording.wav", viewModel.StatusText);
        }

        [StaFact]
        public void StartCommand_Cannot_Execute_When_Already_Recording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            viewModel.StartCommand.Execute(null);

            Assert.False(viewModel.StartCommand.CanExecute(null));
        }

        [StaFact]
        public void StopCommand_Can_Execute_When_Recording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            viewModel.StartCommand.Execute(null);

            Assert.True(viewModel.StopCommand.CanExecute(null));
        }

        [StaFact]
        public void StartCommand_Generates_Default_FilePath_When_Empty()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            viewModel.StartCommand.Execute(null);

            Assert.False(string.IsNullOrWhiteSpace(viewModel.OutputFilePath));
            Assert.EndsWith(".wav", viewModel.OutputFilePath);
        }

        [StaFact]
        public void PropertyChanged_Fires_For_IsRecording()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            var propertyChanged = false;
            viewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(RecorderViewModel.IsRecording))
                {
                    propertyChanged = true;
                }
            };

            viewModel.StartCommand.Execute(null);

            Assert.True(propertyChanged);
        }

        [StaFact]
        public void PropertyChanged_Fires_For_StatusText()
        {
            using var viewModel = new RecorderViewModel(this.mockRecorder.Object);
            var propertyChanged = false;
            viewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(RecorderViewModel.StatusText))
                {
                    propertyChanged = true;
                }
            };

            viewModel.StartCommand.Execute(null);

            Assert.True(propertyChanged);
        }

        [StaFact]
        public void Dispose_Disposes_AudioRecorder()
        {
            var viewModel = new RecorderViewModel(this.mockRecorder.Object);

            viewModel.Dispose();

            this.mockRecorder.Verify(r => r.Dispose(), Times.Once);
        }

        [StaFact]
        public void Constructor_Throws_When_AudioRecorder_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new RecorderViewModel(null!));
        }
    }
}
