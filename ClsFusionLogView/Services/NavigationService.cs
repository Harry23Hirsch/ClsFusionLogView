using ClsFusionViewer.Stores;
using ClsFusionViewer.ViewModels;
using System;

namespace ClsFusionViewer.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<BaseViewModel> _createViewModel;

        public NavigationService(NavigationStore navigatonStore, Func<BaseViewModel> createViewModel)
        {
            _navigationStore = navigatonStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
