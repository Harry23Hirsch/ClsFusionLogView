using ClsFusionViewer.Services;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class StatusLogViewModel : BaseLogViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public StatusLogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            SetGlobals();
        }

        public override void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.StatusLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .SetStatusBarInfoText("");
        }
    }
}
