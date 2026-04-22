namespace DataRepository
{
    public sealed class ApplicationSettings : IApplicationSettings
    {
        public string OutputFolder { get; set; } = string.Empty;

        public string AudioQuality { get; set; } = "Standard";

        public string RecordingFormat { get; set; } = "wav";
    }
}
