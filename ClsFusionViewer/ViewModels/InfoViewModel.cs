using ClsFusionViewer.Commands;
using ClsFusionViewer.Stores;
using ClsFusionViewer.Views;
using System.Windows.Input;

namespace ClsFusionViewer.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        private readonly InteractionStore _interactionStore;
        private string _infoTitle;
        private string _header;
        private string _url;
        private string _email;

        private ICommand _closeCommand;
        private ICommand _copyToClipboard;

        public string InfoTitle => _infoTitle;
        public string Header => _header;
        public string Email => _email;
        public string Url => _url;
        public string ToolTip => Resources.Strings.WindowStrings.ToolTipInfoLink;

        public ICommand CloseCommand => _closeCommand;
        public ICommand CopyToClipboard => _copyToClipboard;

        public InfoViewModel(InteractionStore interactionStore)
        {
            _interactionStore = interactionStore;

            _infoTitle = Resources.Strings.WindowStrings.InfoTitle;
            _header = Resources.Strings.WindowStrings.DefaultTitle;
            _email = Resources.Strings.WindowStrings.Email;
            _url = Resources.Strings.WindowStrings.Url;

            _closeCommand = new RelayCommand<object>(CloseCommand_Execute);
            _copyToClipboard = new RelayCommand<object>(CopyToClipboard_Execute);
        }

        private void CopyToClipboard_Execute(object obj)
        {
            IoC.Helper.CopyToClipboard((string)obj);

            if (((string)obj).Contains("@"))
                _interactionStore.StatusBarInfoText = Resources.Strings.WindowStrings.EmailCopied;
            else
                _interactionStore.StatusBarInfoText = Resources.Strings.WindowStrings.LinkCopied;
        }

        private void CloseCommand_Execute(object obj)
        {
            var info = (InfoView)obj;
            info.Close();

            _interactionStore.StatusBarInfoText = string.Empty;
        }
    }
}
