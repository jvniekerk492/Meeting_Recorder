using DataRepository;

namespace Data
{
    public sealed class SqliteRecordingSession : IRecordingSession
    {
        public string FilePath { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }
    }
}
