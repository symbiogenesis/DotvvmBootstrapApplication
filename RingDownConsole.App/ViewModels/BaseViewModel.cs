using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Serilog;

namespace RingDownConsole.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected void ShowError(string message)
        {
            LogError(message);
            MessageBox.Show(message);
        }

        protected void LogError(string message)
        {
            Log.Information(message);
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
