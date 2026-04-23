using DataRepository;
using Microsoft.Data.Sqlite;

namespace Data
{
    public sealed class SqliteRecordingSessionRepository : IRecordingSessionRepository
    {
        private readonly string databasePath;

        public SqliteRecordingSessionRepository(string databasePath)
        {
            this.databasePath = databasePath ?? throw new ArgumentNullException(nameof(databasePath));
            this.InitializeDatabase();
        }

        public void Save(IRecordingSession session)
        {
            ArgumentNullException.ThrowIfNull(session);

            using var connection = this.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
INSERT INTO RecordingSession (FilePath, FileName, StartedAt)
VALUES ($filePath, $fileName, $startedAt);";
            command.Parameters.AddWithValue("$filePath", session.FilePath);
            command.Parameters.AddWithValue("$fileName", session.FileName);
            command.Parameters.AddWithValue("$startedAt", session.StartedAt.ToString("o"));
            command.ExecuteNonQuery();
        }

        public IReadOnlyList<IRecordingSession> GetAll()
        {
            using var connection = this.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT FilePath, FileName, StartedAt FROM RecordingSession ORDER BY StartedAt;";
            using var reader = command.ExecuteReader();

            var sessions = new List<IRecordingSession>();

            while (reader.Read())
            {
                sessions.Add(new SqliteRecordingSession
                {
                    FilePath = reader.GetString(0),
                    FileName = reader.GetString(1),
                    StartedAt = DateTime.Parse(reader.GetString(2))
                });
            }

            return sessions;
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
CREATE TABLE IF NOT EXISTS RecordingSession
(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FilePath TEXT NOT NULL,
    FileName TEXT NOT NULL,
    StartedAt TEXT NOT NULL
);";
            command.ExecuteNonQuery();
        }
    }
}
