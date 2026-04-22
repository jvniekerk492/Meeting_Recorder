using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AudioManager;
using Data;
using DataRepository;
using Meeting_Recorder.Factories;
using Meeting_Recorder.Interface;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;

namespace Meeting_Recorder
{
    public sealed partial class MainWindow : Window
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            var databasePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MeetingRecorder", "settings.db");
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(databasePath)!);
            var repository = new SqliteApplicationSettingsRepository(databasePath);
            this.mainWindowViewModel = new MainWindowViewModel(repository);
            this.DataContext = this.mainWindowViewModel;
        }

        private void MenuOverlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.mainWindowViewModel.ToggleMenuCommand.Execute(null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
