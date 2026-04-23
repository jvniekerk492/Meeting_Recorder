namespace DataRepository
{
    public interface IRecordingSession
    {
        string FilePath { get; set; }

        string FileName { get; set; }

        DateTime StartedAt { get; set; }
    }
}
