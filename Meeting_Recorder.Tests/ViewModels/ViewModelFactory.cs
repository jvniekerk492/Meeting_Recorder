using DataRepository;
using Meeting_Recorder.Interface;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Recorder.Tests.ViewModels
{
    internal class ViewModelFactory
    {
        IViewModel Create(ViewType viewType, IApplicationSettingsRepository applicationSettingsRepository)
        {
            switch (viewType)
            {
                case ViewType.Recorder:
                    return new MainWindowViewModel(applicationSettingsRepository);
                case ViewType.BasicSettings:
                    return new BasicSettingsViewModel(applicationSettingsRepository);
                default:
                    throw new ArgumentException("Invalid view type", nameof(viewType));
            }
        }
    }
}
