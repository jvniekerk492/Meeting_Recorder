namespace DataRepository
{
    public interface IRecordingSessionRepository
    {
        void Save(IRecordingSession session);

        IReadOnlyList<IRecordingSession> GetAll();
    }
}
