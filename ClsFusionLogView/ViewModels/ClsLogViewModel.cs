﻿using ClsFusionViewer.Services;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
                OnPropertyChanged(nameof(ClsLogMonthSelectedItem));

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

                _clsLogLines = new ObservableCollection<ClsLogFileLine>(result.Reverse());
            }
            else
            {
                _clsLogMonth.Clear();

                var resultMonthInt = new List<int>();
                var resultLogLines =  new List<ClsLogFileLine>();

                foreach (IEnumerable<ClsLogFileLineType> log in storeLogFiles)
                {
                    foreach (ClsLogFileLineType logLine in log)
                    {
                        if (logLine.Year == _clsLogFileSelectedItem)
                        {
                            resultLogLines.Add(new ClsLogFileLine(logLine));

                            if (!resultMonthInt.Contains(int.Parse(logLine.Month)))
                                resultMonthInt.Add(int.Parse(logLine.Month));
                        }
                    }   
                }

                resultMonthInt.Sort();

                var resultMonthString = new List<string>();
                resultMonthString.AddRange(resultMonthInt.Select(x => x.ToString()));
                resultMonthString.Add("Alle");
                resultMonthString.Reverse();

                _clsLogMonth = new ObservableCollection<string>(resultMonthString);
                _clsLogMonthSelectedItem = _clsLogMonth.First();

                resultLogLines.Sort();
                resultLogLines.Reverse();
                _clsLogLines = new ObservableCollection<ClsLogFileLine>(resultLogLines);
            }

            OnPropertyChanged(nameof(ClsLogLines));
            OnPropertyChanged(nameof(ClsLogMonth));
            OnPropertyChanged(nameof(ClsLogMonthSelectedItem));
            OnPropertyChanged(nameof(IsClsMonthEnabled));
        }
        private void MapLogMonth()
        {
            _clsLogLines.Clear();

            var result = new List<ClsLogFileLine>();

            foreach (IEnumerable<ClsLogFileLineType> log in base.ClsStore_.ClsLogFiles)
            {
                var fu = log.Select(x => new ClsLogFileLine(x)).ToList();
                foreach (ClsLogFileLine line in fu)
                {
                    if (line is null)
                        continue;

                    if (_clsLogMonthSelectedItem is null)
                        continue;

                    if (_clsLogMonthSelectedItem.Equals("Alle"))
                    {
                        if (line.Year == int.Parse(_clsLogFileSelectedItem))
                            result.Add(line);
                    }
                    else
                    {
                        if (line.Year == int.Parse(_clsLogFileSelectedItem) && 
                            line.Month == int.Parse(_clsLogMonthSelectedItem))
                            result.Add(line);
                    }
                }
            }

            result.Sort();
            result.Reverse();

            _clsLogLines = new ObservableCollection<ClsLogFileLine>(result);
            OnPropertyChanged(nameof(ClsLogLines));
        }

        public override void PropertyChanged_(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ClsLogLines) ||
                e.PropertyName == nameof(ClsLogFileSelectedItem) ||
                e.PropertyName == nameof(ClsLogMonthSelectedItem))
            {
                if (_clsLogFileSelectedItem != null)
                {
                    IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                        .SetStatusBarInfoText(
                            String.Format(
                                Resources.Strings.FormatedStrings.BcsLogEntriesFound,
                                _clsLogLines?.Count));
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
                        Resources.Strings.WindowStrings.ClsLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                    .SetStatusBarInfoText(
                        String.Format(
                            Resources.Strings.FormatedStrings.LogEntriesFound,
                            _clsLogLines.Count));
        }
    }
}
