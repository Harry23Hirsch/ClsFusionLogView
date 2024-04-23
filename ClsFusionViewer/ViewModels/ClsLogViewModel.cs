using ClsFusionViewer.Services;
using ClsFusionViewer.Stores;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogViewModel : BaseLogViewModel
    {
        private string _clsLogFileSelectedItem;
        private ObservableCollection<string> _clsLogMonth;
        private ObservableCollection<ClsLogFileLine> _clsLogLines;
        private bool _isClsMonthEnabled;
        private string _clsLogMonthSelectedItem;

        public ObservableCollection<string> ClsLogFiles => MapLogs(base.ClsStore_.ClsLogFiles);
        public string ClsLogFileSelectedItem
        {
            get => _clsLogFileSelectedItem;
            set
            {
                _clsLogFileSelectedItem = value;
                OnPropertyChanged(nameof(ClsLogFileSelectedItem));

                MapLogYear();
            }
        }

        public ObservableCollection<string> ClsLogMonth
        {
            get => _clsLogMonth;
            set
            {
                _clsLogMonth = value;
                OnPropertyChanged(nameof(ClsLogMonth));
            }
        }
        public string ClsLogMonthSelectedItem
        {
            get => _clsLogMonthSelectedItem;
            set
            {
                _clsLogMonthSelectedItem = value;
                
                MapLogMonth();
            }
        }
        public bool IsClsMonthEnabled
        {
            get => _isClsMonthEnabled;
            set
            {
                _isClsMonthEnabled = value;
                OnPropertyChanged(nameof(IsClsMonthEnabled));
            }
        }
        public ObservableCollection<ClsLogFileLine> ClsLogLines
        {
            get => _clsLogLines;
            set
            {
                _clsLogLines = value;
                OnPropertyChanged(nameof(ClsLogLines));
            }
        }

        public ClsLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _isClsMonthEnabled = true;
            _clsLogMonth = new ObservableCollection<string>();
            this.ClsLogFileSelectedItem = "Alle";

            SetGlobals();
        }

        private ObservableCollection<string> MapLogs(IEnumerable<IEnumerable<ClsLogFileLineType>> logs)
        {
            var result = new List<string>();

            foreach (IEnumerable<ClsLogFileLineType> log in logs)
            {

                foreach (ClsLogFileLineType line in log)
                {
                    if (result.Contains(line.Year))
                        continue;

                    result.Add(line.Year);
                }
            }

            result.Add("Alle");

            return new ObservableCollection<string>(new ObservableCollection<string>(result).Reverse().ToList());
        }
        private void MapLogYear()
        {
            _isClsMonthEnabled = true;
            var storeLogFiles = base.ClsStore_.ClsLogFiles;

            if (_clsLogFileSelectedItem.Equals("Alle"))
            {
                _isClsMonthEnabled = false;
                _clsLogMonthSelectedItem = null;

                var result = new ObservableCollection<ClsLogFileLine>();

                foreach (IEnumerable<ClsLogFileLineType> log in storeLogFiles)
                {
                    foreach (ClsLogFileLineType line in log)
                    {
                        result.Add(new ClsLogFileLine(line));
                    }
                }

                _clsLogLines = new ObservableCollection<ClsLogFileLine>(new ObservableCollection<ClsLogFileLine>(result).Reverse().ToList());
                OnPropertyChanged(nameof(ClsLogLines));
            }
            else
            {
                _clsLogMonth.Clear();

                var resu = new List<string>();
                

                foreach (IEnumerable<ClsLogFileLineType> log in storeLogFiles)
                {

                    var fu = log.Select(x => new ClsLogFileLine(x)).ToList();
                    foreach (ClsLogFileLine line in fu)
                    {
                        if (line.Year == _clsLogFileSelectedItem)
                        {
                            resu.Add(line.Month);
                            break;
                        }
                    }
                    
                }

                resu.Sort();
                resu.Reverse();
                resu.Add("Alle");
                resu.Reverse();

                _clsLogMonth = new ObservableCollection<string>(resu);
                this.ClsLogMonthSelectedItem = _clsLogMonth.First();
            }

            OnPropertyChanged(nameof(ClsLogMonth));
            OnPropertyChanged(nameof(ClsLogMonthSelectedItem));
            OnPropertyChanged(nameof(IsClsMonthEnabled));
        }
        private void MapLogMonth()
        {
            if (!string.IsNullOrEmpty(_clsLogMonthSelectedItem) && _clsLogMonthSelectedItem.Equals("Alle"))
            {
                _clsLogLines.Clear();

                var result = new ObservableCollection<ClsLogFileLine>();

                foreach (IEnumerable<ClsLogFileLineType> log in base.ClsStore_.ClsLogFiles)
                {

                    var fu = log.Select(x => new ClsLogFileLine(x)).ToList();
                    foreach (ClsLogFileLine line in fu)
                    {
                        if (line.Month == _clsLogMonthSelectedItem)
                        {
                            result.Add(line);
                        }
                    }
                }

                _clsLogLines = new ObservableCollection<ClsLogFileLine>(new ObservableCollection<ClsLogFileLine>(result).Reverse().ToList());
                OnPropertyChanged(nameof(ClsLogLines));
            }
        }
        public override void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.ClsLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                .SetStatusBarInfoText($"CLS Log-Einträge gefunden: {this.ClsLogFiles.Count}");
        }


    }
}
