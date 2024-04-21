using ClsFusionViewer.Services;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel : BaseLogViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<BcsBatStatusInfo> _bcsLogs;
        private string[] _filterList = { "Alle", "15min", "20min", "30min", "40min", "60min", };
        private string _filterSelectedItem;
        private int _filterSelectedIndex;
        private BcsBatStatusInfo _bcsLogsSelectedItem;
        private int _bcsLogsSelectedIndex;

        public ObservableCollection<BcsBatStatusInfo> BcsLogs 
        {
            get => _bcsLogs;
            set
            {
                _bcsLogs = value;
                OnPropertyChanged(nameof(BcsLogs));
            }
        }
        public BcsBatStatusInfo BcsLogsSelectedItem
        {
            get => _bcsLogsSelectedItem;
            set
            {
                _bcsLogsSelectedItem = value;
                OnPropertyChanged(nameof(BcsLogsSelectedItem));
            }
        }
        public int BcsLogsSelectedIndex
        {
            get => _bcsLogsSelectedIndex;
            set
            {
                _bcsLogsSelectedIndex = value;
                OnPropertyChanged(nameof(BcsLogsSelectedIndex));
            }
        }
        public string[] FilterList => _filterList;
        public string FilterSelectedItem
        {
            get => _filterSelectedItem;
            set
            {
                _filterSelectedItem = value;
                OnPropertyChanged(nameof(FilterSelectedItem));


                FilterLogFiles();

            }
        }
        public int FilterSelectedIndex
        {
            get => _filterSelectedIndex;
            set
            {
                _filterSelectedIndex = value;
                OnPropertyChanged(nameof(FilterSelectedIndex));

            }
        }

        public BcsLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            _serviceProvider = serviceProvider;

            base.ClsStore.BcsLogFiles_Changed += ClsStore_BcsLogFiles_Changed;

            _bcsLogs = new ObservableCollection<BcsBatStatusInfo>(base.ClsStore.BcsLogFiles);
            _bcsLogsSelectedItem = _bcsLogs.Last();
            _filterSelectedItem = _filterList.First();
        }

        private void ClsStore_BcsLogFiles_Changed()
        {
            OnPropertyChanged(nameof(BcsLogs));
        }

        private void FilterLogFiles()
        {
            var logs = new ObservableCollection<BcsBatStatusInfo>(base.ClsStore.BcsLogFiles);

            switch (this.FilterSelectedItem)
            {
                case "Alle":

                    _bcsLogs = new ObservableCollection<BcsBatStatusInfo>(base.ClsStore.BcsLogFiles);
                    OnPropertyChanged(nameof(BcsLogs));

                    _bcsLogsSelectedItem = logs.Last();
                    OnPropertyChanged(nameof(BcsLogsSelectedItem));

                    break;

                case "15min":

                    var result = new List<BatStatus>();
                    var al = logs;
                    long temp = _bcsLogsSelectedItem.BcsBatStatus.First().N;
                    long last = _bcsLogsSelectedItem.BcsBatStatus.Last().N;

                    foreach (BatStatus b in _bcsLogsSelectedItem.BcsBatStatus)
                    {
                        if (b.N == last)
                        {
                            result.Add(b);
                            break;
                        }

                        if (b.N >= temp + 180)
                        {
                            temp = b.N;
                            result.Add(b);
                        }
                    }

                    _bcsLogsSelectedItem.BcsBatStatus = result;

                    _bcsLogs = al;
                    OnPropertyChanged(nameof(BcsLogs));

                    OnPropertyChanged(nameof(BcsLogsSelectedItem));


                    break;

                case "20min":
                    _bcsLogs = null;
                    OnPropertyChanged(nameof(BcsLogs));

                    _bcsLogsSelectedItem = null;
                    OnPropertyChanged(nameof(BcsLogsSelectedItem));
                    break;

                case "30min":
                    break;

                case "40min":
                    break;

                case "60min":
                    break;

            }
        }

        public override void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.BcsLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider)?
                .SetStatusBarInfoText("");
        }
    }
}
