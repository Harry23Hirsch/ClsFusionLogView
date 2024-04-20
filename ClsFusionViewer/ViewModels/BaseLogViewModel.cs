using ClsFusionViewer.Stores;
using System;

namespace ClsFusionViewer.ViewModels
{
    public abstract class BaseLogViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ClsStore _clsStore;

        public IServiceProvider ServiceProvider => _serviceProvider;
        public ClsStore ClsStore => _clsStore;

        protected BaseLogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _clsStore = IoC.Helper.GetScopedService<ClsStore>(serviceProvider);

            SetGlobals();
        }

        public abstract void SetGlobals();
    }
}
