namespace DataRepository
{
    public interface IApplicationSettingsRepository
    {
        IApplicationSettings GetOrCreate();

        void Save(IApplicationSettings applicationSettings);
    }
}
