using System.IO;
using Data;
using DataRepository;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Meeting_Recorder.Tests.Data
{
    public sealed class SqliteRecordingSessionRepositoryTests : IDisposable
    {
        private readonly string databasePath;
        private readonly SqliteRecordingSessionRepository repository;

        public SqliteRecordingSessionRepositoryTests()
        {
            this.databasePath = Path.Combine(Path.GetTempPath(), $"test_sessions_{Guid.NewGuid()}.db");
            this.repository = new SqliteRecordingSessionRepository(this.databasePath);
        }

        public void Dispose()
        {
            SqliteConnection.ClearAllPools();

            if (File.Exists(this.databasePath))
            {
                File.Delete(this.databasePath);
            }
        }

        [Fact]
        public void Constructor_Throws_When_DatabasePath_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SqliteRecordingSessionRepository(null!));
        }

        [Fact]
        public void Save_Persists_Session_To_Database()
        {
            var session = new RecordingSession
            {
                FilePath = @"C:\Recordings\Meeting_20240101_120000.wav",
                FileName = "Meeting_20240101_120000.wav",
                StartedAt = new DateTime(2024, 1, 1, 12, 0, 0)
            };

            this.repository.Save(session);

            var sessions = this.repository.GetAll();
            Assert.Single(sessions);
            Assert.Equal(session.FilePath, sessions[0].FilePath);
            Assert.Equal(session.FileName, sessions[0].FileName);
            Assert.Equal(session.StartedAt, sessions[0].StartedAt);
        }

        [Fact]
        public void Save_Throws_When_Session_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => this.repository.Save(null!));
        }

        [Fact]
        public void GetAll_Returns_Empty_List_When_No_Sessions()
        {
            var sessions = this.repository.GetAll();

            Assert.Empty(sessions);
        }

        [Fact]
        public void GetAll_Returns_Multiple_Sessions()
        {
            this.repository.Save(new RecordingSession
            {
                FilePath = @"C:\Recordings\Session1.wav",
                FileName = "Session1.wav",
                StartedAt = new DateTime(2024, 1, 1, 10, 0, 0)
            });

            this.repository.Save(new RecordingSession
            {
                FilePath = @"C:\Recordings\Session2.wav",
                FileName = "Session2.wav",
                StartedAt = new DateTime(2024, 1, 2, 11, 0, 0)
            });

            var sessions = this.repository.GetAll();
            Assert.Equal(2, sessions.Count);
        }
    }
}
