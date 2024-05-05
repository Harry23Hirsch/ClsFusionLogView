using System;

namespace ClsFusionViewer.Stores
{
    public class InteractionStore
    {
        private string _statusbarInfoText;
        private string _windowTitle;

        public event Action WindowTitle_Changed;
        public event Action StatusBarInfoText_Changed;

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnWindowTitleChanged();
            }
        }
        public string StatusBarInfoText
        {
            get => _statusbarInfoText;
            set
            {
                _statusbarInfoText = value;
                OnStatusBarInfoTextChanged();
            }
        }

        private void OnWindowTitleChanged()
        {
            WindowTitle_Changed?.Invoke();
        }
        private void OnStatusBarInfoTextChanged()
        {
            StatusBarInfoText_Changed?.Invoke();
        }
    }
}
