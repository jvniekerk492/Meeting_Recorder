namespace DataRepository
{
    public sealed class RecordingSession : IRecordingSession
    {
        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }
    }
}
