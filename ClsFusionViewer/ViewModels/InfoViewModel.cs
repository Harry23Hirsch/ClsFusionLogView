using ClsFusionViewer.Commands;
using ClsFusionViewer.Views;
using System.Windows.Input;

namespace ClsFusionViewer.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        private string _infoTitle;
        private ICommand _closeCommand;
        private string _header;
        private string _url;
        private string _email;

        public string InfoTitle => _infoTitle;
        public string Header => _header;
        public string Email => _email;
        public string Url => _url;
        public ICommand CloseCommand => _closeCommand;

        public InfoViewModel()
        {
            _infoTitle = Resources.Strings.WindowStrings.InfoTitle;
            _header = Resources.Strings.WindowStrings.DefaultTitle;
            _email = Resources.Strings.WindowStrings.Email;
            _url = Resources.Strings.WindowStrings.Url;

            _closeCommand = new RelayCommand<object>(CloseCommand_Execute);
        }

        private void CloseCommand_Execute(object obj)
        {
            var info = (InfoView)obj;
            info.Close();
        }
    }
}
