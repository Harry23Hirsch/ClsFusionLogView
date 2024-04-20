using ClsFusionViewer.Services;
using System;


namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel : BaseLogViewModel
    {
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
