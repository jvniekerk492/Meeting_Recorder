using AudioManager;
using Xunit;

namespace Meeting_Recorder.Tests
{
    public sealed class AudioRecorderTests
    {
        [Fact]
        public void Implements_IAudioRecorder_Interface()
        {
            var recorderType = typeof(AudioRecorder);
            Assert.True(typeof(IAudioRecorder).IsAssignableFrom(recorderType));
        }

        [Fact]
        public void IsRecording_Initially_False()
        {
            using var recorder = new AudioRecorder();
            Assert.False(recorder.IsRecording);
        }

        [Fact]
        public void Elapsed_Initially_Zero()
        {
            using var recorder = new AudioRecorder();
            Assert.Equal(TimeSpan.Zero, recorder.Elapsed);
        }

        [Fact]
        public void Dispose_Does_Not_Throw_When_Not_Recording()
        {
            var recorder = new AudioRecorder();
            var exception = Record.Exception(() => recorder.Dispose());
            Assert.Null(exception);
        }
    }
}
