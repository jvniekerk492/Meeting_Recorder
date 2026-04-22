using System.Windows.Input;
using DataRepository;
using Meeting_Recorder.Factories;
using Meeting_Recorder.Interface;
using Meeting_Recorder.Views;

namespace Meeting_Recorder.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly IApplicationSettingsRepository applicationSettingsRepository;
        private readonly ViewModelFactory viewModelFactory;
        private bool isMenuOpen;
        private bool isNavigationExpanded;


        public MainWindowViewModel(IApplicationSettingsRepository applicationSettingsRepository)
        {
            this.applicationSettingsRepository = applicationSettingsRepository ?? throw new ArgumentNullException(nameof(applicationSettingsRepository));
            this.ToggleMenuCommand = new RelayCommand(this.ExecuteToggleMenu);
            this.NavigateCommand = new RelayCommand<ViewType?>(this.ExecuteNavigate);
            this.ToggleNavigationCommand = new RelayCommand(this.ExecuteToggleNavigation);
            this.currentView = ViewFactory.Instance.CreateView(ViewType.Recorder, new RecorderViewModel(new AudioManager.AudioRecorder(), applicationSettingsRepository));
            this.viewModelFactory = new ViewModelFactory();
        }

        public bool IsMenuOpen
        {
            get { return this.isMenuOpen; }
            private set => this.SetProperty(ref this.isMenuOpen, value);
        }

        public bool IsNavigationExpanded
        {
            get => this.isNavigationExpanded;
            private set => this.SetProperty(ref this.isNavigationExpanded, value);
        }

        private IView currentView;
        public IView CurrentView
        {
            get => this.currentView;
            private set => this.SetProperty(ref this.currentView, value);
        }

        public ICommand ToggleMenuCommand { get; }

        public ICommand NavigateCommand { get; }

        public ICommand ToggleNavigationCommand { get; }

        public IApplicationSettingsRepository ApplicationSettingsRepository => this.applicationSettingsRepository;

        private void ExecuteToggleMenu()
        {
            this.IsMenuOpen = !this.IsMenuOpen;
        }

        private void ExecuteNavigate(ViewType? viewType)
        {
            if (viewType.HasValue)
            {
                var viewmodel = this.viewModelFactory.Create(viewType.Value, this.applicationSettingsRepository);
                this.CurrentView = ViewFactory.Instance.CreateView(viewType.Value, viewmodel);
            }
            this.IsMenuOpen = false;
        }

        private void ExecuteToggleNavigation()
        {
            this.IsNavigationExpanded = !this.IsNavigationExpanded;
        }
    }
}
