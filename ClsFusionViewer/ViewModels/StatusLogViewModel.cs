using ClsFusionViewer.Services;
using InoTec;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClsFusionViewer.ViewModels
{
    public class StatusLogViewModel : BaseLogViewModel
    {
        private readonly ObservableCollection<ClsFaultInfoType> _statusLogFiles;
        private ObservableCollection<string> _statusLogLines;

        public string Header => Resources.Strings.WindowStrings.HeaderActiveFaults;
        public ObservableCollection<string> StatusLogLines
        {
            get => _statusLogLines;
            set
            {
                _statusLogLines = value;
                OnPropertyChanged(nameof(StatusLogLines));
            }
        }

        public StatusLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _statusLogFiles = new ObservableCollection<ClsFaultInfoType>(base.ClsStore_.ClsStatusLogFiles);
            _statusLogLines = new ObservableCollection<string>();

            var foo = _statusLogFiles.Last().Stromkreise;

            foreach (ClsLightFaultInfoType t in foo)
            {
                var line = String.Format("{0} Leuchte {1}.{2}.{3}", t.Text, t.Cls, t.Slot, t.Adr);
                _statusLogLines.Add(line);
            }

            OnPropertyChanged(nameof(StatusLogLines));

            SetGlobals();
        }

        public override void PropertyChanged_(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StatusLogLines))
            {
                var faultCount = _statusLogFiles.Last().Stromkreise.ToList().Count +
                _statusLogFiles.Last().Batterie.ToList().Count +
                _statusLogFiles.Last().Externe.ToList().Count;

                IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                            .SetStatusBarInfoText(
                                String.Format(
                                    Resources.Strings.FormatedStrings.StatusLogEntriesFound,
                                    faultCount));
            }
        }
        public override void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.StatusLogViewTitle)
                    );
        }
    }
}
