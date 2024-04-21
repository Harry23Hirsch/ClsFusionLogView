using ClsFusionViewer.Services;
using InoTec;
using System;
using System.Collections.ObjectModel;
using System.Linq;


namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel : BaseLogViewModel
    {
        public ObservableCollection<BcsBatStatusInfo> BcsLogs => new ObservableCollection<BcsBatStatusInfo>(base.ClsStore.BcsLogFiles.ToList());

        public BcsLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
                
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
