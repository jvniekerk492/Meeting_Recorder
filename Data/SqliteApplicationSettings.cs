using DataRepository;

namespace Data
{
    public sealed class SqliteApplicationSettings : IApplicationSettings
    {
        public string OutputFolder { get; set; } = string.Empty;

        public string AudioQuality { get; set; } = "Standard";

        public string RecordingFormat { get; set; } = "wav";
    }
}
