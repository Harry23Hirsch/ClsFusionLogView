using ClsFusionViewer.Commands;
using ClsFusionViewer.Views;
using System.Windows.Input;

namespace ClsFusionViewer.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        private string _infoTitle;
        private ICommand _closeCommand;

        public string InfoTitle => _infoTitle;
        public ICommand CloseCommand => _closeCommand;

        public InfoViewModel()
        {
            _infoTitle = Resources.Strings.WindowStrings.InfoTitle;

            _closeCommand = new RelayCommand<object>(CloseCommand_Execute);
        }

        private void CloseCommand_Execute(object obj)
        {
            var info = (InfoView)obj;
            info.Close();
        }
    }
}
