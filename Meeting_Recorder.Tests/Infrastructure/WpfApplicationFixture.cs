using System.Windows;
using Xunit;

namespace Meeting_Recorder.Tests.Infrastructure
{
    public sealed class WpfApplicationFixture : IDisposable
    {
        private readonly Application application;
        private bool disposed;

        public WpfApplicationFixture()
        {
            if (Application.Current == null)
            {
                this.application = new App();
                this.application.Resources.MergedDictionaries.Add(
                    new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/Meeting_Recorder;component/Resources/Styles.xaml")
                    });
            }
            else
            {
                this.application = Application.Current;
            }
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.disposed = true;
            }
        }
    }

    [CollectionDefinition("WPF Application Collection")]
    public sealed class WpfApplicationCollection : ICollectionFixture<WpfApplicationFixture>
    {
    }
}
