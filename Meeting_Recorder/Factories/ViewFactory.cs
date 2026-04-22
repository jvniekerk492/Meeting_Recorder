using Meeting_Recorder.Interface;
using Meeting_Recorder.Views;

namespace Meeting_Recorder.Factories
{
    public sealed class ViewFactory
    {
        private static readonly Lazy<ViewFactory> instance = new(() => new ViewFactory());
        private readonly IReadOnlyDictionary<ViewType, Func<IViewModel, IView>> viewMappings;

        private ViewFactory()
        {
            this.viewMappings = new Dictionary<ViewType, Func<IViewModel, IView>>
            {
                [ViewType.Recorder] = static viewModel => new Recorder(viewModel),
                [ViewType.BasicSettings] = static viewModel => new BasicSettings(viewModel)
            };
        }

        public static ViewFactory Instance => instance.Value;

        public IView CreateView(ViewType viewType, IViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            if (!this.viewMappings.TryGetValue(viewType, out var createView))
            {
                throw new NotSupportedException($"View type '{viewType}' is not registered.");
            }

            return createView(viewModel);
        }
    }
}
