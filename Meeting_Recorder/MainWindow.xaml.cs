using System.Windows;
using AudioManager;
using Meeting_Recorder.Interface;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;

namespace Meeting_Recorder
{
    public sealed partial class MainWindow : Window
    {
        private readonly RecorderViewModel recorderViewModel;
        private readonly IView recorderView;

        public MainWindow()
        {
            InitializeComponent();
            this.recorderViewModel = new RecorderViewModel(new AudioRecorder());
            this.recorderView = ViewFactory.Instance.CreateView(ViewType.Recorder, this.recorderViewModel);
            this.Content = this.recorderView.View;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.recorderViewModel.Dispose();
            base.OnClosed(e);
        }
    }
}
