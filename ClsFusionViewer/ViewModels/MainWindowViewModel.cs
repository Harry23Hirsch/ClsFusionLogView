using InoTec;
using System;
using ClsFusionViewer.Services;
using System.Windows.Input;
using ClsFusionViewer.Commands;
using ClsFusionViewer.Stores;
using System.Linq;
using System.ComponentModel;

namespace ClsFusionViewer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionStore _interactionStore;
        private readonly NavigationStore _navigationStore;
        private readonly ClsStore _clsStore;

        private bool _projectLoaded;
        private bool _clsLogEnabled;
        private bool _clsLogIsChecked;
        private bool _bcsLogEnabled;
        private bool _statusLogEnabled;

        private readonly ICommand _openCommand;
        private readonly ICommand _closeCommand;
        private readonly ICommand _closeProjectCommand;
        private readonly ICommand _clsLogViewCommand;
        private readonly ICommand _bcsLogViewCommand;
        private readonly ICommand _statusLogViewCommand;
        private readonly ICommand _openInfoCommand;

        public ICommand OpenCommand => _openCommand;
        public ICommand CloseCommand => _closeCommand;
        public ICommand CloseProjectCommand => _closeProjectCommand;
        public ICommand ClsLogViewCommand => _clsLogViewCommand;
        public ICommand BcsLogViewCommand => _bcsLogViewCommand;
        public ICommand StatusLogViewCommand => _statusLogViewCommand;
        public ICommand OpenInfoCommand => _openInfoCommand;

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
        public bool ClsLogIsChecked
        {
            get => _clsLogIsChecked;
            set
            {
                _clsLogIsChecked = value;
                OnPropertyChanged(nameof(ClsLogIsChecked));
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

            _clsStore = IoC.Helper.GetScopedService<ClsStore>(servicesProvider);

            PropertyChanged += PropertyChanged_;

            _openCommand = new RelayCommand<object>(OpenProjectCommand_Execute, OpenProjectCommand_CanExecute);
            _closeCommand = new RelayCommand<object>(CloseCommand_Execute);
            _closeProjectCommand = new RelayCommand<object>(CloseProjectCommand_Execute, CloseProjectCommand_CanExecute);
            _openInfoCommand = new RelayCommand<object>(OpenInfoCommand_Execute);

            _clsLogViewCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateClsLogViewModel));
            _bcsLogViewCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateBcsLogViewModel));
            _statusLogViewCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateStatusLogViewModel));

            _projectLoaded = false;
        }

        private void PropertyChanged_(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProjectLoaded))
            {
                if (!ProjectLoaded)
                    IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                        .SetStatusBarInfoText(Resources.Strings.WindowStrings.ProjectClosed);
            }
        }

        private BaseViewModel CreateStatusLogViewModel()
        {
            return new StatusLogViewModel(_serviceProvider);
        }
        private BaseViewModel CreateBcsLogViewModel()
        {
            return new BcsLogViewModel(_serviceProvider);
        }
        private BaseViewModel CreateClsLogViewModel()
        {
            _clsLogIsChecked = true;
            OnPropertyChanged(nameof(ClsLogIsChecked));

            return new ClsLogViewModel(_serviceProvider);
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
            if (LoadCLS())
            {
                _projectLoaded = true;
                OnPropertyChanged(nameof(ProjectLoaded));
            }
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
        private void OpenInfoCommand_Execute(object obj)
        {
            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .OpenInfoView();
        }

        private bool LoadCLS()
        {
            var projectPath = IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?.OpenFolderDialog() ?? throw new AccessViolationException();

            if (string.IsNullOrEmpty(projectPath))
            {
                return false;
            }

            var fh = new ClsFileHandler(projectPath);

            try
            {
                _clsStore.ClsLogFiles = fh.GetClsLogFiles();
                if (_clsStore.ClsLogFiles.Any())
                {
                    _clsLogEnabled = true;
                }
                else
                    _clsLogEnabled = false;
                OnPropertyChanged(nameof(ClsLogEnabled));
            }
            catch (Exception)
            {
                _clsLogEnabled = false;
                OnPropertyChanged(nameof(ClsLogEnabled));
            }

            try
            {
                _clsStore.BcsLogFiles = fh.GetBtLogFiles();
                if (_clsStore.BcsLogFiles.Any())
                    _bcsLogEnabled = true;
                else
                    _bcsLogEnabled = false;
                OnPropertyChanged(nameof(BcsLogEnabled));
            }
            catch (Exception)
            {
                _bcsLogEnabled = false;
                OnPropertyChanged(nameof(BcsLogEnabled));
            }

            try
            {
                _clsStore.ClsStatusLogFiles = fh.GetClsFaultInfos();
                if (_clsStore.ClsStatusLogFiles.Any())
                    _statusLogEnabled = true;
                else
                    _statusLogEnabled = false;
                OnPropertyChanged(nameof(StatusLogEnabled));
            }
            catch (Exception)
            {
                _statusLogEnabled = false;
                OnPropertyChanged(nameof(StatusLogEnabled));
            }

            _clsLogViewCommand.Execute(null);

            return true;
        }
    }
}
