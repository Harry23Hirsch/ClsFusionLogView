using ClsFusionViewer.Services;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel : BaseLogViewModel
    {
        private readonly string[] _filterList = { "Alle", "15min", "20min", "30min", "40min", "60min", };
        private ObservableCollection<string> _bcsLogs;
        private string _filterSelectedItem;
        private ObservableCollection<BatStatusType> _bcsLogLines;
        private string _bcsLogsSelectedItem;
        private string _bcsLogsLastSelectedItem;

        public ObservableCollection<string> BcsLogs
        {
            get => _bcsLogs;
            set
            {
                _bcsLogs = value;
                OnPropertyChanged(nameof(BcsLogs));
            }
        }
        public string BcsLogsSelectedItem
        {
            get => _bcsLogsSelectedItem;
            set
            {
                _bcsLogsSelectedItem = value;
                OnPropertyChanged(nameof(_bcsLogsSelectedItem));

                if (BcsLogsSelectedItem != null)
                {
                    _bcsLogsLastSelectedItem = _bcsLogsSelectedItem;
                    OnPropertyChanged(nameof(BcsLogsLastSelectedItem));
                }

                GetBcsLogLines();
                OnPropertyChanged(nameof(BcsLogLines));

                FilterLogFiles();
            }
        }
        public string BcsLogsLastSelectedItem
        {
            get => _bcsLogsLastSelectedItem;
            set
            {
                _bcsLogsLastSelectedItem = value;
                OnPropertyChanged(nameof(_bcsLogsLastSelectedItem));
            }
        }
        public ObservableCollection<BatStatusType> BcsLogLines
        {
            get => _bcsLogLines;
            set
            {
                _bcsLogLines = value;
                OnPropertyChanged(nameof(BcsLogLines));
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

        public BcsLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _bcsLogs = new ObservableCollection<string>(base.ClsStore_.BcsLogFiles.Select(x => x.BcsBatInfo.N.ToString()));

            this.BcsLogsSelectedItem = _bcsLogs.Last();
            this.FilterSelectedItem = _filterList[0];

            SetGlobals();
        }

        private void GetBcsLogLines()
        {
            var foo = base.ClsStore_.BcsLogFiles.Where(x => x.BcsBatInfo.N.ToString().Equals(_bcsLogsSelectedItem)).FirstOrDefault();
            _bcsLogLines = new ObservableCollection<BatStatusType>(foo.BcsBatStatus.ToList());
        }
        private void FilterLogFiles()
        {
            GetBcsLogLines();

            if (this.FilterSelectedItem == null || this.FilterSelectedItem.Equals(_filterList[0]))
            {
                GetBcsLogLines();
            }
            else if (this.FilterSelectedItem.Equals(_filterList[1]))
            {
                _bcsLogLines = FilterBatStatus(_bcsLogLines, 900);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[2]))
            {
                _bcsLogLines = FilterBatStatus(_bcsLogLines, 1200);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[3]))
            {
                _bcsLogLines = FilterBatStatus(_bcsLogLines, 1800);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[4]))
            {
                _bcsLogLines = FilterBatStatus(_bcsLogLines, 2400);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[5]))
            {
                _bcsLogLines = FilterBatStatus(_bcsLogLines, 3600);
            }

            OnPropertyChanged(nameof(BcsLogLines));

            return;
        }
        private ObservableCollection<BatStatusType> FilterBatStatus(IEnumerable<BatStatusType> bcsLogLines, int timeStop)
        {
            var result = new List<BatStatusType>();
            long temp = bcsLogLines.First().N;
            long last = bcsLogLines.Last().N;

            result.Add(bcsLogLines.First());

            foreach (BatStatusType b in bcsLogLines)
            {
                if (b.N == last)
                {
                    result.Add(b);
                    break;
                }

                var foo = temp + timeStop;
                if (b.N >= foo)
                {
                    temp = b.N;
                    result.Add(b);
                }
            }

            return new ObservableCollection<BatStatusType>(result);
        }

        public override void PropertyChanged_(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BcsLogLines))
            {
                if (this.BcsLogsSelectedItem != null)
                {
                    IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                        .SetStatusBarInfoText(
                            String.Format(
                                Resources.Strings.FormatedStrings.BcsLogEntriesFound,
                                _bcsLogLines.Count));
                }
            }
        }
        public override void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.BcsLogViewTitle)
                    );
        }
    }
}
