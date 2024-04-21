using ClsFusionViewer.Services;
using ClsFusionViewer.Stores;
using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogViewModel : BaseLogViewModel
    {
        private ObservableCollection<ObservableCollection<ClsLogFileLine>> _clsLogFiles;

        public ObservableCollection<ClsLogFileLine> ClsLogFiles => MapLogs(base.ClsStore.ClsLogFiles);

        public ClsLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            base.ClsStore.ClsLogFiles_Changed += ClsStore_ClsLogFiles_Changed;
        }

        private void ClsStore_ClsLogFiles_Changed()
        {
            OnPropertyChanged(nameof(this.ClsLogFiles));
        }
        private ObservableCollection<ClsLogFileLine> MapLogs(IEnumerable<IEnumerable<ClsLogFileLine>> logs)
        {
            var result = new ObservableCollection<ClsLogFileLine>();

            foreach (IEnumerable<ClsLogFileLine> log in logs)
            {
                foreach (ClsLogFileLine line in log)
                {
                    result.Add(line);
                } 
            }

            return new ObservableCollection<ClsLogFileLine>(result.Reverse().ToList());
        }

        public override void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.ClsLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider)?
                .SetStatusBarInfoText($"CLS Log-Einträge gefunden: {this.ClsLogFiles.Count}");
        }
    }
}
