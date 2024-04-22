using ClsFusionViewer.Services;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class StatusLogViewModel : BaseLogViewModel
    {

        public StatusLogViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SetGlobals();
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

            IoC.Helper.GetScopedService<InterActionServices>(base.ServiceProvider_)?
                .SetStatusBarInfoText("");
        }
    }
}
