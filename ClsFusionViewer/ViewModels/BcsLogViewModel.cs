﻿using ClsFusionViewer.Services;
using ClsFusionViewer.Stores;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel : BaseLogViewModel
    {
        private readonly string[] _filterList = { "Alle", "15min", "20min", "30min", "40min", "60min", };
        private ObservableCollection<BcsBatStatusInfoViewModel> _bcsLogs;
        private string _filterSelectedItem;
        private int _filterSelectedIndex;
        private BcsBatStatusInfoViewModel _bcsLogsSelectedItem;
        private int _bcsLogsSelectedIndex;
        private BcsBatStatusInfoViewModel _bcsLogsLastSelectedItem;

        public ObservableCollection<BcsBatStatusInfoViewModel> BcsLogs 
        {
            get => _bcsLogs;
            set
            {
                _bcsLogs = value;
                OnPropertyChanged(nameof(BcsLogs));
            }
        }
        public BcsBatStatusInfoViewModel BcsLogsSelectedItem
        {
            get => _bcsLogsSelectedItem;
            set
            {
                _bcsLogsSelectedItem = value;
                OnPropertyChanged(nameof(BcsLogsSelectedItem));

                if (_bcsLogsSelectedItem != null)
                {
                    _bcsLogsLastSelectedItem = _bcsLogsSelectedItem;
                    OnPropertyChanged(nameof(BcsLogsLastSelectedItem));
                }
            }
        }
        public BcsBatStatusInfoViewModel BcsLogsLastSelectedItem
        {
            get => _bcsLogsLastSelectedItem;
            set
            {
                _bcsLogsLastSelectedItem = value;
                OnPropertyChanged(nameof(BcsLogsLastSelectedItem));
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
            _bcsLogs = new ObservableCollection<BcsBatStatusInfoViewModel>(base.ClsStore_.BcsLogFiles
                .Select(x => new BcsBatStatusInfoViewModel(x)).ToList());
            _bcsLogsSelectedItem = _bcsLogs.Last();
            _filterSelectedItem = _filterList.First();
            
            SetGlobals();
        }

        private void FilterLogFiles()
        {
            _bcsLogs.Clear();
            _bcsLogsSelectedItem = null;

            _bcsLogs = new ObservableCollection<BcsBatStatusInfoViewModel>(base.ClsStore_.BcsLogFiles
                .Select(x => new BcsBatStatusInfoViewModel(x)).ToList());

            if (_bcsLogsLastSelectedItem != null && _bcsLogs.Count > 0)
                _bcsLogsSelectedItem = _bcsLogsLastSelectedItem;
            else
                _bcsLogsSelectedItem= _bcsLogs.Last(); 

            BcsBatStatusInfoViewModel item = null;

            if (this.FilterSelectedItem.Equals(_filterList[1]))
            {
                item = FilterBatStatus(_bcsLogs[_bcsLogs.IndexOf(_bcsLogsSelectedItem)], 900);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[2]))
            {
                item = FilterBatStatus(_bcsLogs[_bcsLogs.IndexOf(_bcsLogsSelectedItem)], 1200);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[3]))
            {
                item = FilterBatStatus(_bcsLogs[_bcsLogs.IndexOf(_bcsLogsSelectedItem)], 1800);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[4]))
            {
                item = FilterBatStatus(_bcsLogs[_bcsLogs.IndexOf(_bcsLogsSelectedItem)], 2400);
            }
            else if (this.FilterSelectedItem.Equals(_filterList[5]))
            {
                item = FilterBatStatus(_bcsLogs[_bcsLogs.IndexOf(_bcsLogsSelectedItem)], 3600);
            }

            if (item != null)
            {
                _bcsLogs[_bcsLogs.IndexOf(_bcsLogsSelectedItem)] = item;
                _bcsLogsSelectedItem = item;
            }

            OnPropertyChanged(nameof(BcsLogs));
            OnPropertyChanged(nameof(BcsLogsSelectedItem));

            return;
        }
        private BcsBatStatusInfoViewModel FilterBatStatus(BcsBatStatusInfoViewModel item, int timeStop)
        {
            var result = new List<BatStatus>();
            long temp = item.BcsBatStatus.First().N;
            long last = item.BcsBatStatus.Last().N;

            result.Add(item.BcsBatStatus.First());

            foreach (BatStatus b in item.BcsBatStatus)
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

            item.BcsBatStatus = new ObservableCollection<BatStatus>(result);

            return item;
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

            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                .SetStatusBarInfoText("");
        }
    }
}
