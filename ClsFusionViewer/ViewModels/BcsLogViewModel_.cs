using ClsFusionViewer.Services;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel_ : BaseLogViewModel
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

            }
        }

        public BcsLogViewModel_(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _filterSelectedItem = _filterList[0];

            _bcsLogs = new ObservableCollection<string>( base.ClsStore_.BcsLogFiles.Select(x => x.BcsBatInfo.N.ToString()));

            SetGlobals();
        }

        private void FilterLogFiles()
        {
            //_bcsLogLines?.Clear();
            //_bcsLogLines = _bcsLogsSelectedItem.BcsBatStatus;

            //BcsBatStatusInfo item = null;

            //if (this.FilterSelectedItem == null && this.FilterSelectedItem.Equals(_filterList[0]))
            //{
            //    _bcsLogLines = _bcsLogsSelectedItem.BcsBatStatus;
            //}
            //else if(this.FilterSelectedItem.Equals(_filterList[1]))
            //{
            //    item = FilterBatStatus(_bcsLogsSelectedItem, 900);
            //}
            //else if (this.FilterSelectedItem.Equals(_filterList[2]))
            //{
            //    item = FilterBatStatus(_bcsLogsSelectedItem, 1200);
            //}
            //else if (this.FilterSelectedItem.Equals(_filterList[3]))
            //{
            //    item = FilterBatStatus(_bcsLogsSelectedItem, 1800);
            //}
            //else if (this.FilterSelectedItem.Equals(_filterList[4]))
            //{
            //    item = FilterBatStatus(_bcsLogsSelectedItem, 2400);
            //}
            //else if (this.FilterSelectedItem.Equals(_filterList[5]))
            //{
            //    item = FilterBatStatus(_bcsLogsSelectedItem, 3600);
            //}

            //if (item != null)
            //{
            //    _bcsLogLines = item.BcsBatStatus;
            //}

            //OnPropertyChanged(nameof(BcsLogLines));
            //OnPropertyChanged(nameof(BcsLogs));
            //OnPropertyChanged(nameof(BcsLogsSelectedItem));

            return;
        }
        private BcsBatStatusInfo FilterBatStatus(BcsBatStatusInfo item, int timeStop)
        {
            var result = new List<BatStatusType>();
            long temp = item.BcsBatStatus.First().N;
            long last = item.BcsBatStatus.Last().N;

            result.Add(item.BcsBatStatus.First());

            foreach (BatStatusType b in item.BcsBatStatus)
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

            item.BcsBatStatus = new ObservableCollection<BatStatusType>(result);

            return item;
        }

        public override void PropertyChanged_(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(BcsLogsSelectedItem) ||
            //    e.PropertyName == nameof(FilterSelectedItem))
            //{
            //    if (this.BcsLogsSelectedItem != null)
            //    {
            //        IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
            //            .SetStatusBarInfoText(
            //                String.Format(
            //                    Resources.Strings.FormatedStrings.BcsLogEntriesFound,
            //                    _bcsLogs.Count));
            //    }
            //}
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

            //IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
            //            .SetStatusBarInfoText(
            //                String.Format(
            //                    Resources.Strings.FormatedStrings.BcsLogEntriesFound,
            //                    _bcsLogs.Count));
        }
    }
}
