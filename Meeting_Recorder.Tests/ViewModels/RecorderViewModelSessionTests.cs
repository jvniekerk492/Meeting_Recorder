using AudioManager;
using DataRepository;
using Meeting_Recorder.ViewModels;
using Moq;
using Xunit;

namespace Meeting_Recorder.Tests.ViewModels
{
    public sealed class RecorderViewModelSessionTests
    {
        private readonly Mock<IAudioRecorder> mockAudioRecorder;
        private readonly Mock<IRecordingSessionRepository> mockSessionRepository;

        public RecorderViewModelSessionTests()
        {
            this.mockAudioRecorder = new Mock<IAudioRecorder>();
            this.mockSessionRepository = new Mock<IRecordingSessionRepository>();
        }

        [StaFact]
        public void StartCommand_Captures_Start_Time()
        {
            using var viewModel = new RecorderViewModel(this.mockAudioRecorder.Object, null, this.mockSessionRepository.Object);
            var beforeStart = DateTime.Now;

            viewModel.StartCommand.Execute(null);

            Assert.True(viewModel.RecordingStartedAt >= beforeStart);
        }

        [StaFact]
        public void StopCommand_Saves_Session_With_FilePath_And_StartTime()
        {
            using var viewModel = new RecorderViewModel(this.mockAudioRecorder.Object, null, this.mockSessionRepository.Object);
            viewModel.OutputFilePath = @"C:\Recordings\Meeting_20240101_120000.wav";
            viewModel.StartCommand.Execute(null);

            viewModel.StopCommand.Execute(null);

            this.mockSessionRepository.Verify(
                r => r.Save(It.Is<IRecordingSession>(s =>
                    s.FilePath == @"C:\Recordings\Meeting_20240101_120000.wav" &&
                    s.FileName == "Meeting_20240101_120000.wav")),
                Times.Once);
        }

        [StaFact]
        public void StopCommand_Saves_Session_With_Correct_StartedAt()
        {
            using var viewModel = new RecorderViewModel(this.mockAudioRecorder.Object, null, this.mockSessionRepository.Object);
            viewModel.OutputFilePath = @"C:\Recordings\Meeting_20240101_120000.wav";
            var beforeStart = DateTime.Now;
            viewModel.StartCommand.Execute(null);
            var afterStart = DateTime.Now;

            viewModel.StopCommand.Execute(null);

            this.mockSessionRepository.Verify(
                r => r.Save(It.Is<IRecordingSession>(s =>
                    s.StartedAt >= beforeStart && s.StartedAt <= afterStart)),
                Times.Once);
        }

        [StaFact]
        public void StopCommand_Does_Not_Save_Session_When_Repository_Is_Null()
        {
            using var viewModel = new RecorderViewModel(this.mockAudioRecorder.Object, null, null);
            viewModel.OutputFilePath = @"C:\Recordings\Meeting_20240101_120000.wav";
            viewModel.StartCommand.Execute(null);

            var exception = Record.Exception(() => viewModel.StopCommand.Execute(null));

            Assert.Null(exception);
        }
    }
}
