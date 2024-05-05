using ClsFusionViewer.ViewModels;
using System;

namespace ClsFusionViewer.Stores
{
    public class NavigationStore
    {
        private BaseViewModel _currentViewModel;

        public event Action CurrentViewModel_Changed;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModel_Changed?.Invoke();
        }
    }
}
