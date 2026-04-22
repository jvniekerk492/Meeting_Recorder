using Meeting_Recorder.Tests.TestDoubles;
using Meeting_Recorder.ViewModels;
using Meeting_Recorder.Views;
using Xunit;

namespace Meeting_Recorder.Tests.ViewModels
{
    public sealed class MainWindowViewModelTests
    {
        [StaFact]
        public void Constructor_Sets_Default_View_To_Recorder()
        {
            var viewModel = CreateViewModel();

            Assert.IsType<Recorder>(viewModel.CurrentView);
        }

        [StaFact]
        public void Constructor_Sets_Menu_Closed()
        {
            var viewModel = CreateViewModel();

            Assert.False(viewModel.IsMenuOpen);
        }

        [StaFact]
        public void ToggleMenuCommand_Opens_Menu_When_Closed()
        {
            var viewModel = CreateViewModel();

            viewModel.ToggleMenuCommand.Execute(null);

            Assert.True(viewModel.IsMenuOpen);
        }

        [StaFact]
        public void ToggleMenuCommand_Closes_Menu_When_Open()
        {
            var viewModel = CreateViewModel();
            viewModel.ToggleMenuCommand.Execute(null);

            viewModel.ToggleMenuCommand.Execute(null);

            Assert.False(viewModel.IsMenuOpen);
        }

        [StaFact]
        public void NavigateCommand_Changes_Current_View_To_BasicSettings()
        {
            var viewModel = CreateViewModel();

            viewModel.NavigateCommand.Execute(ViewType.BasicSettings);

            Assert.IsType<BasicSettings>(viewModel.CurrentView);
        }

        [StaFact]
        public void NavigateCommand_Changes_Current_View_To_Recorder()
        {
            var viewModel = CreateViewModel();
            viewModel.NavigateCommand.Execute(ViewType.BasicSettings);

            viewModel.NavigateCommand.Execute(ViewType.Recorder);

            Assert.IsType<Recorder>(viewModel.CurrentView);
        }

        [StaFact]
        public void NavigateCommand_Closes_Menu()
        {
            var viewModel = CreateViewModel();
            viewModel.ToggleMenuCommand.Execute(null);

            viewModel.NavigateCommand.Execute(ViewType.BasicSettings);

            Assert.False(viewModel.IsMenuOpen);
        }

        [StaFact]
        public void IsNavigationExpanded_Defaults_To_False()
        {
            var viewModel = CreateViewModel();

            Assert.False(viewModel.IsNavigationExpanded);
        }

        [StaFact]
        public void ToggleNavigationCommand_Expands_Navigation_Submenu()
        {
            var viewModel = CreateViewModel();

            viewModel.ToggleNavigationCommand.Execute(null);

            Assert.True(viewModel.IsNavigationExpanded);
        }

        [StaFact]
        public void ToggleNavigationCommand_Collapses_When_Already_Expanded()
        {
            var viewModel = CreateViewModel();
            viewModel.ToggleNavigationCommand.Execute(null);

            viewModel.ToggleNavigationCommand.Execute(null);

            Assert.False(viewModel.IsNavigationExpanded);
        }

        [StaFact]
        public void NavigateCommand_Raises_PropertyChanged_For_CurrentView()
        {
            var viewModel = CreateViewModel();
            var propertyChanged = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MainWindowViewModel.CurrentView))
                {
                    propertyChanged = true;
                }
            };

            viewModel.NavigateCommand.Execute(ViewType.BasicSettings);

            Assert.True(propertyChanged);
        }

        private static MainWindowViewModel CreateViewModel()
        {
            return new MainWindowViewModel(new InMemoryApplicationSettingsRepository());
        }
    }
}
