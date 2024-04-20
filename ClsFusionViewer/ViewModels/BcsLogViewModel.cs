using ClsFusionViewer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClsFusionViewer.ViewModels
{
    public class BcsLogViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public BcsLogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            SetGlobals();
        }

        private void SetGlobals()
        {
            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .SetWindowTitle(
                    String.Format(
                        "{0} - {1}",
                        Resources.Strings.WindowStrings.DefaultTitle,
                        Resources.Strings.WindowStrings.BcsLogViewTitle)
                    );

            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?
                .SetStatusBarInfoText("");
        }
    }
}
