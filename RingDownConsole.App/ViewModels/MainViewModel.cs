using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dataq.Devices;
using Dataq.Devices.DI1100;
using Dataq.Misc;
using Dataq.Protocols.Enums;
using Microsoft.Win32;
using RingDownConsole.Models;
using RingDownConsole.Utils.Extensions;
using Serilog;

namespace RingDownConsole.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private const double SAMPLE_RATE = 915.55;

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:3456") };
        private static readonly BackgroundWorker _worker = new BackgroundWorker { WorkerReportsProgress = true };
        private static DateTime _lastSentDate = DateTime.MinValue;

        private static IEnumerable<Status> _statuses;

        private static bool _showSettings;
        private static bool _showNameEntry;
        private static Location _location;
        private static Device _targetDevice;
        private static Task _taskRead;
        private static CancellationTokenSource _cancelRead;
        private static PhoneStatus? _currentPhoneStatus;
        private static string _currentPhoneUser;
        private static string _locationCode;
        private static string _locationName;

        public MainViewModel()
        {
            Settings = new SettingsViewModel();

            _worker.DoWork += async (object sender, DoWorkEventArgs e) => await Initialize();
            _worker.RunWorkerAsync();

            SystemEvents.PowerModeChanged += OnPowerChange;

            PopulateColors();
        }

        ~MainViewModel()
        {
            if (_targetDevice != null)
            {
                _targetDevice.Dispose();
                _targetDevice = null;
            }
        }

        #region Properties

        public SettingsViewModel Settings { get; }

        public PhoneStatus? CurrentPhoneStatus
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

        public string CurrentPhoneUser
        {
            get { return _currentPhoneUser; }
            set
            {
                if (_currentPhoneUser != value)
                {
                    _currentPhoneUser = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LocationCode
        {
            get { return _locationCode; }
            set
            {
                if (_locationCode != value)
                {
                    _locationCode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LocationName
        {
            get { return _locationName; }
            set
            {
                if (_locationName != value)
                {
                    _locationName = value;
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

        public bool ShowNameEntry
        {
            get { return Settings.PromptForName && _showNameEntry; }
            set
            {
                if (_showNameEntry != value)
                {
                    if (Settings.PromptForName && value)
                        ShowWindow();

                    _showNameEntry = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void ShowWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
            }));
        }

        public ICommand FindDeviceCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = async () => await FindDevice()
                };
            }
        }

        public ICommand CloseSettingsCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => ShowSettings = false
                };
            }
        }

        #endregion

        private async Task GetLocation(string serialNumber)
        {
            try
            {
                _location = await _httpClient.GetLocationBySerialNumberAsync<Location>(serialNumber);
                if (_location != null)
                {
                    LocationCode = _location.Code;
                    LocationName = _location.Name;
                }
            }
            catch
            {
                ShowError("Couldn't connect to Web Service");
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

        private async Task Initialize()
        {
            if (_statuses == null)
            {
                _statuses = await GetStatuses();
            }

            var deviceFound = await FindDevice();

            if (deviceFound)
                await ToggleDataAcquisition();
        }

        private async Task<IEnumerable<Status>> GetStatuses()
        {
            return await _httpClient.GetDataAsync<Status>();
        }

        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    //ResetUsb();
                    _worker.CancelAsync();
                    _worker.RunWorkerAsync();
                    break;
                case PowerModes.Suspend:
                    break;
            }
        }

        private void ResetUsb()
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4, RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 3, RegistryValueKind.DWord);
        }

        private async Task<bool> FindDevice()
        {
            if (_targetDevice == null)
            {
                //  Get a list of devices with model DI-1100
                IReadOnlyList<IDevice> AllDevices = await Discovery.ByModelAsync(typeof(Device));

                var anyDevice = AllDevices.Count > 0;

                if (anyDevice)
                {
                    HideError();

                    Log.Information("Found a DI-1100.");

                    //  Cast first device from generic device to specific DI-1100 type
                    _targetDevice = ((Device) (AllDevices[0]));

                    await GetLocation(_targetDevice.Serial);

                    //  Send serial number as unique identifier to web service
                }
                else
                {
                    ShowError("No DI-1100 found.");
                    return false;
                }
            }

            // Disconnect any open connections
            await _targetDevice.DisconnectAsync();

            // Try to connect
            await _targetDevice.ConnectAsync();

            //  Ensure it's stopped
            await _targetDevice.AcquisitionStopAsync();

            //  Query device for some info
            await _targetDevice.QueryDeviceAsync();

            return true;
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
                if (!TimeHasElapsed())
                    continue;

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

                    await SaveVoltageData(voltage);
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

        private PhoneStatus? GetStatus(double voltage)
        {
            if (voltage >= Settings.MinConnectedVoltage && voltage <= Settings.MaxConnectedVoltage)
            {
                return PhoneStatus.Connected;
            }

            if (voltage >= Settings.MinOffHookVoltage && voltage <= Settings.MaxOffHookVoltage)
            {
                return PhoneStatus.OffHook;
            }

            if (voltage >= Settings.MinOnHookVoltage && voltage <= Settings.MaxOnHookVoltage)
            {
                return PhoneStatus.OnHook;
            }

            if (voltage >= Settings.MinNoDialToneVoltage && voltage <= Settings.MaxNoDialToneVoltage)
            {
                return PhoneStatus.NoDialTone;
            }

            if (voltage < 0)
            {
                return PhoneStatus.OnHook;
            }

            return PhoneStatus.Unknown;
        }

        private async Task SaveVoltageData(double voltage)
        {
            if (TimeHasElapsed())
            {
                CurrentPhoneStatus = GetStatus(voltage);

                switch (_currentPhoneStatus)
                {
                    case PhoneStatus.OffHook:
                    case PhoneStatus.Connected:
                        ShowNameEntry = true;
                        //var dispatcherOperation = Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                        //{
                        //    if (Application.Current.MainWindow.Visibility == Visibility.Hidden)
                        //        Application.Current.MainWindow.Show();
                        //}));
                        break;
                    case null:
                        ShowNameEntry = false;
                        LogError($"Voltage {voltage} does not correspond to a status");
                        return;
                    default:
                        ShowNameEntry = false;
                        break;
                }

                await SendStatus();
            }
        }

        private bool TimeHasElapsed()
        {
            return _lastSentDate <= DateTime.UtcNow.AddSeconds(Settings.IntervalSeconds * -1);
        }

        private async Task SendStatus()
        {
            if (_location == null)
            {
                await GetLocation(_targetDevice.Serial);
                return;
            }

            var status = GetStatusEntity(_currentPhoneStatus);

            var locationStatus = new LocationStatus
            {
                LocationId = _location.Id,
                StatusId = status.Id,
                CurrentPhoneUser = _currentPhoneUser,
                RecordedDate = DateTime.UtcNow
            };

            var success = await _httpClient.PostDataAsync(locationStatus);

            if (success.IsSuccessStatusCode)
                _lastSentDate = locationStatus.RecordedDate;
        }

        private Status GetStatusEntity(PhoneStatus? currentPhoneStatus)
        {
            if (currentPhoneStatus == null)
            {
                return _statuses.First(s => s.Name == nameof(PhoneStatus.Unknown));
            }
            else
            {
                var status = _statuses.FirstOrDefault(s => s.Name == currentPhoneStatus?.ToString());
                return status ?? _statuses.First(s => s.Name == nameof(PhoneStatus.Unknown));
            }
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
                    ShowError("Please enable at least one analog channel or digital port.");

                    // Detect if no channels are enabled, and bail if so. 
                    //MessageBox.Show(ErrorMessage, "No Enabled Channels", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    ShowError("Please configure at least one analog channel or digital port as an input");
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
