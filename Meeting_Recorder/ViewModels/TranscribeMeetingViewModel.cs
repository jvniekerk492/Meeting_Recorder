using DataRepository;

namespace Meeting_Recorder.ViewModels
{
    public sealed class TranscribeMeetingViewModel : ViewModelBase
    {
        private readonly IApplicationSettingsRepository applicationSettingsRepository;

        public TranscribeMeetingViewModel(IApplicationSettingsRepository applicationSettingsRepository)
        {
            this.applicationSettingsRepository = applicationSettingsRepository ?? throw new ArgumentNullException(nameof(applicationSettingsRepository));
        }
    }
}
