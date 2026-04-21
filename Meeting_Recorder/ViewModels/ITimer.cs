namespace Meeting_Recorder.ViewModels
{
    public interface ITimer
    {
        event EventHandler Tick;

        void Start();

        void Stop();
    }
}
