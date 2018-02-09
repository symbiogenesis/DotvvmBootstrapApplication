﻿using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Windows;
using Dataq.Devices;
using Dataq.Devices.DI1100;
using Serilog;

namespace RingDownConsole.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string _errorMessage = "Device not found";
        private bool _showErrorPanel;

        protected static Device _targetDevice;

        protected static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

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
            PurgeChannelData();

            Log.Information(message);
        }

        protected void PurgeChannelData()
        {
            if (_targetDevice == null)
                return;

            // get the next row
            //  purge displayed data
            foreach (var ch in _targetDevice.Channels)
            {
                if (ch.GetType().GetInterfaces().Contains(typeof(IChannelIn)))
                {
                    ((IChannelIn) (ch)).DataIn.Clear();
                }
            }
        }

        protected void DisposeDevice()
        {
            if (_targetDevice == null)
                return;

            PurgeChannelData();

            _targetDevice.AcquisitionStopAsync();

            _targetDevice.Dispose();
            _targetDevice = null;
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
