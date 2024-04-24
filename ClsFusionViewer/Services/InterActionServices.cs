using ClsFusionViewer.Stores;
using ClsFusionViewer.ViewModels;
using ClsFusionViewer.Views;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;

namespace ClsFusionViewer.Services
{
    public class InterActionServices
    {
        private readonly InteractionStore _interactionStore;

        public InterActionServices(InteractionStore interactionStore)
        {
            _interactionStore = interactionStore;
        }

        public MessageBoxResult ShowMessageBox(string msg, string title, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage img = MessageBoxImage.None)
        {
            return MessageBox.Show(msg, title, button, img);
        }

        public string OpenFolderDialog()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }
        public void OpenInfoView()
        {
            var info = new InfoView();
            info.DataContext = new InfoViewModel();
            info.ShowDialog();
        }

        public void SetWindowTitle(string text)
        {
            _interactionStore.WindowTitle = text;
        }
        public void SetStatusBarInfoText(string text)
        {
            _interactionStore.StatusBarInfoText = text;
        }
    }
}
