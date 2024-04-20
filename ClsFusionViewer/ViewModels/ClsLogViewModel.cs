using ClsFusionViewer.Services;
using ClsFusionViewer.Stores;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogViewModel : BaseLogViewModel
    {
        public ClsLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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
                .SetStatusBarInfoText("");
        }
    }
}
