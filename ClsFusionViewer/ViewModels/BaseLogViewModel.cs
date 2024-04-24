using ClsFusionViewer.Stores;
using System;
using System.ComponentModel;

namespace ClsFusionViewer.ViewModels
{
    public abstract class BaseLogViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ClsStore _clsStore;

        public IServiceProvider ServiceProvider_ => _serviceProvider;
        public ClsStore ClsStore_ => _clsStore;

        protected BaseLogViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _clsStore = IoC.Helper.GetScopedService<ClsStore>(serviceProvider);

            PropertyChanged += PropertyChanged_;
        }

        public abstract void PropertyChanged_(object sender, PropertyChangedEventArgs e);
        public abstract void SetGlobals();
    }
}
