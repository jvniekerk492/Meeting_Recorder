using DataRepository;

namespace Meeting_Recorder.Tests.TestDoubles
{
    public sealed class InMemoryApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private IApplicationSettings applicationSettings = new ApplicationSettings();

        public IApplicationSettings GetOrCreate()
        {
            return this.applicationSettings;
        }

        public void Save(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }
    }
}
