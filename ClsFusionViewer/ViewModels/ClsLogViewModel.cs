using ClsFusionViewer.Services;
using ClsFusionViewer.Stores;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ClsStore _clsStore;

        public ClsLogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _clsStore = IoC.Helper.GetScopedService<ClsStore>(serviceProvider);

            SetGlobals();
        }

        private void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.ClsLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .SetStatusBarInfoText("");
        }
    }
}
