using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Serilog;

namespace RingDownConsole.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string _errorMessage = "Device not found";
        private bool _showErrorPanel;

        public bool ShowErrorPanel
        {
            get { return _showErrorPanel; }
            set
            {
                if (_showErrorPanel != value)
                {
                    _showErrorPanel = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected void HideError()
        {
            ShowErrorPanel = false;
        }

        protected void ShowError(string message, bool mesageBox = false)
        {
            LogError(message);

            if (mesageBox)
            {
                MessageBox.Show(message);
            }
            else
            {
                ErrorMessage = message;
                ShowErrorPanel = true;
            }
        }

        protected void LogError(string message)
        {
            Log.Information(message);
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
