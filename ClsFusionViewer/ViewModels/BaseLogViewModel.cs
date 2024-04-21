using ClsFusionViewer.Stores;
using System;

namespace ClsFusionViewer.ViewModels
{
    public abstract class BaseLogViewModel : BaseViewModel
    {
        protected BaseLogViewModel()
        {
        }

        public abstract void SetGlobals();
    }
}
