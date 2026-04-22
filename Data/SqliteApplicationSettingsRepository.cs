using DataRepository;
using Microsoft.Data.Sqlite;

namespace Data
{
    public sealed class SqliteApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private readonly string databasePath;

        public SqliteApplicationSettingsRepository(string databasePath)
        {
            this.databasePath = databasePath ?? throw new ArgumentNullException(nameof(databasePath));
            this.InitializeDatabase();
        }

        public IApplicationSettings GetOrCreate()
        {
            using var connection = this.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT OutputFolder, AudioQuality, RecordingFormat FROM ApplicationSettings WHERE Id = 1;";
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new SqliteApplicationSettings
                {
                    OutputFolder = reader.GetString(0),
                    AudioQuality = reader.GetString(1),
                    RecordingFormat = reader.GetString(2)
                };
            }

            var defaultSettings = this.CreateDefaultSettings();
            this.Save(defaultSettings);
            return defaultSettings;
        }

        public void Save(IApplicationSettings applicationSettings)
        {
            ArgumentNullException.ThrowIfNull(applicationSettings);

            using var connection = this.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
INSERT INTO ApplicationSettings (Id, OutputFolder, AudioQuality, RecordingFormat)
VALUES (1, $outputFolder, $audioQuality, $recordingFormat)
ON CONFLICT(Id) DO UPDATE SET
    OutputFolder = excluded.OutputFolder,
    AudioQuality = excluded.AudioQuality,
    RecordingFormat = excluded.RecordingFormat;";
            command.Parameters.AddWithValue("$outputFolder", applicationSettings.OutputFolder);
            command.Parameters.AddWithValue("$audioQuality", applicationSettings.AudioQuality);
            command.Parameters.AddWithValue("$recordingFormat", applicationSettings.RecordingFormat);
            command.ExecuteNonQuery();
        }

        private SqliteConnection CreateConnection()
        {
            return new SqliteConnection($"Data Source={this.databasePath}");
        }

        private void InitializeDatabase()
        {
            var directory = Path.GetDirectoryName(this.databasePath);

            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var connection = this.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS ApplicationSettings
(
    Id INTEGER PRIMARY KEY,
    OutputFolder TEXT NOT NULL,
    AudioQuality TEXT NOT NULL,
    RecordingFormat TEXT NOT NULL
);";
            command.ExecuteNonQuery();
        }

        private SqliteApplicationSettings CreateDefaultSettings()
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var defaultOutputFolder = Path.Combine(documentsFolder, "MeetingRecorder");

            return new SqliteApplicationSettings
            {
                OutputFolder = defaultOutputFolder,
                AudioQuality = "Standard",
                RecordingFormat = "wav"
            };
        }
    }
}
