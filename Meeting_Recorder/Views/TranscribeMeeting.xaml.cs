using System.Windows;
using System.Windows.Controls;
using Meeting_Recorder.Interface;

namespace Meeting_Recorder.Views
{
    public sealed partial class TranscribeMeeting : UserControl, IView
    {
        public TranscribeMeeting(IViewModel viewModel)
        {
            this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.DataContext = this.ViewModel;
            this.InitializeComponent();
        }

        public FrameworkElement View => this;

        public IViewModel ViewModel { get; }
    }
}
