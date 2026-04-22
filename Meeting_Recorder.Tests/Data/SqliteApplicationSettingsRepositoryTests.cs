using Data;
using Xunit;

namespace Meeting_Recorder.Tests.Data
{
    public sealed class SqliteApplicationSettingsRepositoryTests
    {
        [Fact]
        public void Save_Then_Load_Returns_Persisted_Values()
        {
            var databasePath = Path.Combine(Path.GetTempPath(), $"meeting-recorder-{Guid.NewGuid():N}.db");

            try
            {
                var repository = new SqliteApplicationSettingsRepository(databasePath);
                var settings = repository.GetOrCreate();
                settings.OutputFolder = @"C:\\RepoTest";
                settings.AudioQuality = "High";
                settings.RecordingFormat = "wav";

                repository.Save(settings);

                var loadedSettings = repository.GetOrCreate();
                Assert.Equal(@"C:\\RepoTest", loadedSettings.OutputFolder);
                Assert.Equal("High", loadedSettings.AudioQuality);
                Assert.Equal("wav", loadedSettings.RecordingFormat);
            }
            finally
            {
                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }
            }
        }
    }
}
