using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dataq.Devices;
using Dataq.Devices.DI1100;
using Dataq.Misc;
using Dataq.Protocols.Enums;
using Serilog;

namespace RingDownConsole.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private const int SAMPLE_RATE = 39000;

        private bool _showSettings;
        private bool _showNameEntry;
        private bool _showDeviceNotFound;
        private Device _targetDevice;
        private Task _taskRead;
        private CancellationTokenSource _cancelRead;
        private PhoneStatus _currentPhoneStatus;

        public MainViewModel()
        {
            PopulateColors();

            new Action(async () =>
            {
                var deviceFound = await FindDevice();

                if (deviceFound)
                    await ToggleDataAcquisition();
            }).Invoke();
        }

        ~MainViewModel()
        {
            if (_targetDevice != null)
            {
                _targetDevice.Dispose();
                _targetDevice = null;
            }
        }

        public PhoneStatus CurrentPhoneStatus
        {
            get { return _currentPhoneStatus; }
            set
            {
                if (_currentPhoneStatus != value)
                {
                    _currentPhoneStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShowSettings
        {
            get { return _showSettings; }
            set
            {
                if (_showSettings != value)
                {
                    _showSettings = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShowDeviceNotFound
        {
            get { return _showDeviceNotFound; }
            set
            {
                if (_showDeviceNotFound != value)
                {
                    _showDeviceNotFound = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShowNameEntry
        {
            get { return _showNameEntry; }
            set
            {
                if (_showNameEntry != value)
                {
                    _showNameEntry = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand FindDeviceCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => ShowDeviceNotFound,
                    CommandAction = async () => await FindDevice()
                };
            }
        }

        private void PopulateColors()
        {
            var colors = new List<string>();

            foreach (var color in Enum.GetValues(typeof(LedColor1)))
            {
                colors.Add(color.ToString());
            }
        }

        private async Task<bool> FindDevice()
        {
            //  Get a list of devices with model DI-1100
            IReadOnlyList<IDevice> AllDevices = await Discovery.ByModelAsync(typeof(Device));

            var anyDevice = AllDevices.Count > 0;

            if (anyDevice)
            {
                ShowDeviceNotFound = false;

                Log.Information("Found a DI-1100.");
                //  Cast first device from generic device to specific DI-1100 type
                _targetDevice = ((Device) (AllDevices[0]));
                await _targetDevice.ConnectAsync();
                //  Ensure it's stopped
                await _targetDevice.AcquisitionStopAsync();
                //  Query device for some info
                await _targetDevice.QueryDeviceAsync();
                //  Print queried info to GUI
            }
            else
            {
                ShowDeviceNotFound = true;
                Log.Information("No DI-1100 found.");
            }

            return anyDevice;
        }

        private async Task GetChannelData()
        {
            // capture the first channel programmed as an input (MasterChannel)
            // and use it to track data availability for all input channels
            IChannelIn MasterChannel = null;
            for (int index = 0; (index <= _targetDevice.Channels.Count); index++)
            {
                if (_targetDevice.Channels[index].GetType().GetInterfaces().Contains(typeof(IChannelIn)))
                {
                    MasterChannel = (IChannelIn) _targetDevice.Channels[index];
                    //  we have our channel 
                    break;
                }
            }

            //  Keep reading while acquiring data
            while (_targetDevice.IsAcquiring)
            {
                //  Read data and catch if cancelled (to exit loop and continue)
                try
                {
                    // throws an error if acquisition has been cancelled
                    // otherwise refreshes the buffer DataIn with new data
                    await _targetDevice.ReadDataAsync(_cancelRead.Token);
                }
                catch (OperationCanceledException ex)
                {
                    Log.Information("Acquisition cancelled");
                    break;
                }

                // get here if acquisition is still active
                if (MasterChannel.DataIn.Count == 0)
                {
                    // get here if no data in the channel buffer
                    // TODO: Continue While... Warning!!! not translated
                }

                //  We have data. Convert it to strings
                double voltage = 0;
                for (var index = 0; (index <= (MasterChannel.DataIn.Count - 1)); index++)
                {
                    if (_targetDevice.Channels.Count > 1)
                    {
                        ShowError("Too many channels found");
                    }

                    //  this is the row (scan) counter
                    foreach (var ch in _targetDevice.Channels)
                    {
                        // this is the column (channel) counter
                        // get a channel value and convert it to a string
                        if (ch.GetType().GetInterfaces().Contains(typeof(IChannelIn)))
                        {
                            voltage = ((IChannelIn) (ch)).DataIn[index];
                        }
                    }

                    SetStatus(voltage);
                    SendVoltageData(voltage);
                }

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
        }
        
        private void SetStatus(double voltage)
        {
            //throw new NotImplementedException();
        }

        private void SendVoltageData(double voltage)
        {
            //throw new NotImplementedException();
        }

        private void ConfigureAnalogChannels()
        {
            var AnalogChan = ((AnalogVoltageIn) (_targetDevice.ChannelFactory(typeof(AnalogVoltageIn), 1)));
        }

        private bool SampleRateBad()
        {
            // ensure that the sample rate per channel is not out of range
            Dataq.Collections.IReadOnlyRange<double> SampleRateRange = ((Dataq.Devices.Dynamic.IReadOnlyChannelSampleRate) (_targetDevice.Channels[0])).GetSupportedSampleRateRange(_targetDevice);
            if (!SampleRateRange.ContainsValue(SAMPLE_RATE))
            {
                MessageBox.Show($"Selected sample rate is outside the range of  {SampleRateRange.Minimum.ToString()} to {SampleRateRange.Maximum.ToString()} Hz inclusive for your current channel settings.", "Sample Rate Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        private async void SetLightColor(LedColor1 color)
        {
            await _targetDevice.Protocol.SetLedColorAsync(color);
        }

        private async Task ToggleDataAcquisition()
        {
            if (_cancelRead != null)
            {
                // get here if an acquisition process is in progress and we've been commanded to stop
                _cancelRead.Cancel();
                // cancel the read process
                _cancelRead = null;
                await _taskRead;
                // wait for the read process to complete
                _taskRead = null;
                await _targetDevice.AcquisitionStopAsync();
                // stop the device from acquiring
            }
            else
            {
                // get here if we're starting a new acquisition process
                _targetDevice.Channels.Clear();
                // initialize the device
                this.ConfigureAnalogChannels();
                if (this.SampleRateBad())
                {
                    // get here if requested sample rate is out of range
                    // It's a bust, so...
                    return;
                }

                // otherwise, the selected sample rate is good, so use it
                _targetDevice.SetSampleRateOnChannels(SAMPLE_RATE);
                try
                {
                    await _targetDevice.InitializeAsync();
                    // configure the device as defined. Errors if no channels are enabled
                }
                catch (Exception ex)
                {
                    // Detect if no channels are enabled, and bail if so. 
                    MessageBox.Show("Please enable at least one analog channel or digital port.", "No Enabled Channels", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // now determine what sample rate per channel the device is using from the 
                // first enabled input channel, and display it
                ChannelIn FirstInChannel;
                bool NoInputChannels = true;
                for (int index = 0; (index <= (_targetDevice.Channels.Count - 1)); index++)
                {
                    if (_targetDevice.Channels[index].GetType().GetInterfaces().Contains(typeof(IChannelIn)))
                    {
                        FirstInChannel = (ChannelIn) _targetDevice.Channels[index];
                        NoInputChannels = false;
                        break;
                    }
                }

                if (NoInputChannels)
                {
                    MessageBox.Show("Please configure at least one analog channel or digital port as an input", "No Inputs Enabled", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Everything is good, so...
                _cancelRead = new CancellationTokenSource();
                //  Create the cancellation token
                await _targetDevice.AcquisitionStartAsync();
                // start acquiring
                //  NOTE: assumes at least one input channel enabled
                //  Start a task in the background to read data
                _taskRead = Task.Run(async () => await GetChannelData(), _cancelRead.Token);
            }
        }
    }
}
