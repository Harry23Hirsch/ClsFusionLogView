using ClsFusionViewer.Services;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public ClsLogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            IoC.Helper.GetScopedService<InterActionServices>(_serviceProvider)?.SetStatusBarInfoText("ClsLogView loaded");
        }
    }
}
