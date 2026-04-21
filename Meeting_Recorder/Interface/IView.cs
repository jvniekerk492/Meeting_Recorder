using System.Windows;

namespace Meeting_Recorder.Interface
{
    public interface IView
    {
        FrameworkElement View { get; }

        IViewModel ViewModel { get; }
    }
}
