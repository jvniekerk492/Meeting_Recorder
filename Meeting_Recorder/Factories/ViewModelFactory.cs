using DataRepository;
using Meeting_Recorder.Interface;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;

namespace Meeting_Recorder.Factories
{
    internal class ViewModelFactory
    {
        internal IViewModel Create(ViewType viewType, IApplicationSettingsRepository applicationSettingsRepository)
        {
            switch (viewType)
            {
                case ViewType.Recorder:
                    return new RecorderViewModel(new AudioManager.AudioRecorder(),applicationSettingsRepository);
                case ViewType.TranscribeMeeting:
                    return new TranscribeMeetingViewModel(applicationSettingsRepository);
                case ViewType.BasicSettings:
                    return new BasicSettingsViewModel(applicationSettingsRepository);
                default:
                    throw new ArgumentException("Invalid view type", nameof(viewType));
            }
        }
    }
}
