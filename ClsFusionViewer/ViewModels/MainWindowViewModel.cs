using InoTec;
using System.Collections.Generic;
using System;
using ClsFusionViewer.Services;
using System.Windows.Input;
using ClsFusionViewer.Commands;
using ClsFusionViewer.Stores;

namespace ClsFusionViewer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionStore _interactionStore;
        private readonly NavigationStore _navigationStore;
        private ICommand _openCommand;
        private ICommand _closeCommand;

        public ICommand OpenCommand => _openCommand;
        public ICommand CloseCommand => _closeCommand;

        public BaseViewModel CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
            set => _navigationStore.CurrentViewModel = value;
        }
        public string WindowTitle
        {
            get => _interactionStore.WindowTitle;
            set => _interactionStore.WindowTitle = value;
        }
        public string StatusBarInfoText
        {
            get => _interactionStore.StatusBarInfoText;
            set => _interactionStore.StatusBarInfoText = value;
        }

        public MainWindowViewModel(IServiceProvider servicesProvider)
        {
            _serviceProvider = servicesProvider;
            _interactionStore = IoC.Helper.GetScopedService<InteractionStore>(servicesProvider);
            _navigationStore = IoC.Helper.GetScopedService<NavigationStore>(servicesProvider);

            _interactionStore.WindowTitle_Changed += InteractionStore_WindowTitle_Changed;
            _interactionStore.StatusBarInfoText_Changed += InteractionStore_StatusBarInfoText_Changed;

            _navigationStore.CurrentViewModel_Changed += NavigationStore_CurrentViewModel_Changed;

            _openCommand = new RelayCommand<object>(OpenCommand_Execute);
            _closeCommand = new RelayCommand<object>(CloseCommand_Execute);
        }


        private void NavigationStore_CurrentViewModel_Changed()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private void InteractionStore_WindowTitle_Changed()
        {
            OnPropertyChanged(nameof(WindowTitle));
        }
        private void InteractionStore_StatusBarInfoText_Changed()
        {
            OnPropertyChanged(nameof(StatusBarInfoText));
        }

        private void OpenCommand_Execute(object obj)
        {
            Test();
        }
        private void CloseCommand_Execute(object obj)
        {
            // Nicht das Schönste
            var mw = (MainWindow)obj;
            if (mw !=  null)
                mw.Close();
        }

        private void Test()
        {
            var projectPath = IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?.OpenFolderDialog() ?? throw new AccessViolationException();

            if (string.IsNullOrEmpty(projectPath))
            {
                return;
            }

            var fh = new ClsFileHandler(projectPath);

            IEnumerable<IEnumerable<ClsLogFileLine>> logFile = new List<IEnumerable<ClsLogFileLine>>();
            try
            {
                logFile = fh.GetClsLogFiles();
            }
            catch (Exception ex)
            {
            }

            IEnumerable<BcsBatStatusInfo> btLogFile = new List<BcsBatStatusInfo>();
            try
            {
                btLogFile = fh.GetBtLogFiles();

            }
            catch (Exception ex)
            {
            }

            IEnumerable<ClsFaultInfo> clsFaultFile = new List<ClsFaultInfo>();
            try
            {
                clsFaultFile = fh.GetClsFaultInfos();
            }
            catch (Exception ex)
            {
            }

            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?.SetStatusBarInfoText("TestText");
        }
    }
}
