namespace DataRepository
{
    public interface IApplicationSettings
    {
        string OutputFolder { get; set; }

        string AudioQuality { get; set; }

        string RecordingFormat { get; set; }
    }
}
