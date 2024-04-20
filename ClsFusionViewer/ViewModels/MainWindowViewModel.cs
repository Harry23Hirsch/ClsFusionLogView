using InoTec;
using System.Collections.Generic;
using System;
using ClsFusionViewer.Services;
using System.Windows.Input;
using ClsFusionViewer.Commands;
using ClsFusionViewer.Stores;
using System.Linq;
using System.IO;

namespace ClsFusionViewer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionStore _interactionStore;
        private readonly NavigationStore _navigationStore;
        private bool _projectLoaded;
        private bool _clsLogEnabled;
        private bool _bcsLogEnabled;
        private bool _statusLogEnabled;

        private ICommand _openCommand;
        private ICommand _closeCommand;
        private ICommand _closeProjectCommand;
        private ICommand _clsLogViewCommand;
        private ICommand _bcsLogViewCommand;
        private ICommand _statusLogViewCommand;

        public ICommand OpenCommand => _openCommand;
        public ICommand CloseCommand => _closeCommand;
        public ICommand CloseProjectCommand => _closeProjectCommand;
        public ICommand ClsLogViewCommand => _clsLogViewCommand;
        public ICommand BcsLogViewCommand => _bcsLogViewCommand;
        public ICommand StatusLogViewCommand => _statusLogViewCommand;

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
        public bool ProjectLoaded
        {
            get => _projectLoaded;
            set
            {
                _projectLoaded = value;
                OnPropertyChanged(nameof(ProjectLoaded));
            }
        }
        public bool ClsLogEnabled
        {
            get => _clsLogEnabled;
            set
            {
                _clsLogEnabled = value;
                OnPropertyChanged(nameof(ClsLogEnabled));
            }
        }
        public bool BcsLogEnabled 
        {
            get => _bcsLogEnabled;
            set
            {
                _bcsLogEnabled = value;
                OnPropertyChanged(nameof(BcsLogEnabled));
            }
        }
        public bool StatusLogEnabled
        {
            get => _statusLogEnabled;
            set
            {
                _statusLogEnabled = value;
                OnPropertyChanged(nameof(StatusLogEnabled));
            }
        }

        public MainWindowViewModel(IServiceProvider servicesProvider)
        {
            _serviceProvider = servicesProvider;
            
            _interactionStore = IoC.Helper.GetScopedService<InteractionStore>(servicesProvider);
            _interactionStore.WindowTitle_Changed += InteractionStore_WindowTitle_Changed;
            _interactionStore.StatusBarInfoText_Changed += InteractionStore_StatusBarInfoText_Changed;

            _navigationStore = IoC.Helper.GetScopedService<NavigationStore>(servicesProvider);
            _navigationStore.CurrentViewModel_Changed += NavigationStore_CurrentViewModel_Changed;

            _openCommand = new RelayCommand<object>(OpenProjectCommand_Execute, OpenProjectCommand_CanExecute);
            _closeCommand = new RelayCommand<object>(CloseCommand_Execute);
            _closeProjectCommand = new RelayCommand<object>(CloseProjectCommand_Execute, CloseProjectCommand_CanExecute);

            _clsLogViewCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateClsLogViewModel));
            _bcsLogViewCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateBcsLogViewModel));
            _statusLogViewCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateStatusLogViewModel));

            _projectLoaded = false;
        }

        private BaseViewModel CreateStatusLogViewModel()
        {
            return IoC.Helper.GetScopedService<StatusLogViewModel>(_serviceProvider);
        }
        private BaseViewModel CreateBcsLogViewModel()
        {
            return IoC.Helper.GetScopedService<BcsLogViewModel>(_serviceProvider);
        }
        private BaseViewModel CreateClsLogViewModel()
        {
            return IoC.Helper.GetScopedService<ClsLogViewModel>(_serviceProvider);
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

        private void OpenProjectCommand_Execute(object obj)
        {
            Test();

            _projectLoaded = true;
            OnPropertyChanged(nameof(ProjectLoaded));
        }
        private bool OpenProjectCommand_CanExecute(object obj)
        {
            return !_projectLoaded;
        }
        private void CloseProjectCommand_Execute(object obj)
        {
            _projectLoaded = false;
            OnPropertyChanged(nameof(ProjectLoaded));
        }
        private bool CloseProjectCommand_CanExecute(object obj)
        {
            return _projectLoaded;
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
                if (logFile.Any())
                    _clsLogEnabled = true;
                else
                    _clsLogEnabled = false;
                OnPropertyChanged(nameof(ClsLogEnabled));
            }
            catch (Exception ex)
            {
                _clsLogEnabled = false;
                OnPropertyChanged(nameof(ClsLogEnabled));
            }

            IEnumerable<BcsBatStatusInfo> btLogFile = new List<BcsBatStatusInfo>();
            try
            {
                btLogFile = fh.GetBtLogFiles();
                if (btLogFile.Any())
                    _bcsLogEnabled = true;
                else
                    _bcsLogEnabled = false;
                OnPropertyChanged(nameof(BcsLogEnabled));
            }
            catch (Exception ex)
            {
                _bcsLogEnabled = false;
                OnPropertyChanged(nameof(BcsLogEnabled));
            }

            IEnumerable<ClsFaultInfo> clsFaultFile = new List<ClsFaultInfo>();
            try
            {
                clsFaultFile = fh.GetClsFaultInfos();
                if (clsFaultFile.Any())
                    _statusLogEnabled = true;
                else
                    _statusLogEnabled = false;
                OnPropertyChanged(nameof(StatusLogEnabled));
            }
            catch (Exception ex)
            {
                _statusLogEnabled = false;
                OnPropertyChanged(nameof(StatusLogEnabled));
            }

            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?.SetStatusBarInfoText("TestText");
        }
    }
}
