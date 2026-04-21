namespace AudioManager
{
    public interface IAudioRecorder : IDisposable
    {
        bool IsRecording { get; }

        TimeSpan Elapsed { get; }

        void StartRecording(string outputFilePath);

        void StopRecording();
    }
}
