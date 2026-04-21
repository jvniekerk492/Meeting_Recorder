using System.Windows;
using AudioManager;
using Meeting_Recorder.ViewModels;

namespace Meeting_Recorder
{
    public partial class MainWindow : Window
    {
        private readonly RecorderViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new RecorderViewModel(new AudioRecorder());
            DataContext = this.viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.viewModel.Dispose();
            base.OnClosed(e);
        }
    }
}