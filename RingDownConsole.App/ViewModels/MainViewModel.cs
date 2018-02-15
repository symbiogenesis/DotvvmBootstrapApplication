using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        private static readonly HttpClient _httpClient = new HttpClient();

        private static DateTime _lastSentDate = DateTime.MinValue;
        private static IEnumerable<Status> _statuses;
        private static bool _showSettings;
        private static bool _showNameEntry;
        private static Location _location;
        private static PhoneStatus? _currentPhoneStatus;
        private static string _currentPhoneUser;
        private static string _locationCode;
        private static string _locationName;
        private static IChannelIn _masterChannel;
        private static Status _unknown;

        public MainViewModel()
        {
            Settings = new SettingsViewModel();
            _httpClient.BaseAddress = new Uri(Settings.WebApiConnectionString);

            Task.Run(async () => await Initialize());
            SystemEvents.PowerModeChanged += async (object s, PowerModeChangedEventArgs e) => await OnPowerChange(e);

            PopulateColors();
        }

        ~MainViewModel()
        {
            DisposeDevice();
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
                    CommandAction = async () => await Initialize()
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
                HideError();

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
            try
            {
                ResetUsb();

                await PopulateStatuses();

                var found = await FindDevice();

                if (found)
                    await StartDataAcquisition();
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }

        private async Task PopulateStatuses()
        {
            try
            {
                if (_statuses == null)
                {
                    _statuses = await _httpClient.GetDataAsync<Status>();
                    _unknown = _statuses.First(s => s.Name == nameof(PhoneStatus.Unknown));
                }
            }
            catch {}
        }

        private async Task OnPowerChange(PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    await Initialize();
                    break;
                case PowerModes.Suspend:
                    break;
            }
        }

        private void ResetUsb()
        {
            if (IsAdministrator())
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 3, RegistryValueKind.DWord);
            }
        }

        private async Task<bool> FindDevice()
        {
            try
            {
                DisposeDevice();

                //  Get a list of devices with model DI-1100
                var allDevices = await Discovery.ByModelAsync(typeof(Device));

                var anyDevice = allDevices.Count > 0;

                if (anyDevice)
                {
                    HideError();

                    Log.Information("Found a DI-1100.");

                    //  Cast first device from generic device to specific DI-1100 type
                    _targetDevice = ((Device) (allDevices[0]));

                    await GetLocation(_targetDevice.Serial);

                    //  Send serial number as unique identifier to web service
                }
                else
                {
                    ShowError("No DI-1100 found.");
                    return false;
                }

                // Disconnect any open connections
                await _targetDevice.DisconnectAsync();

                // Try to connect
                await _targetDevice.ConnectAsync();

                //  Ensure it's stopped
                await _targetDevice.AcquisitionStopAsync();

                //  Query device for some info
                await _targetDevice.QueryDeviceAsync();

                ShowErrorPanel = false;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
                return false;
            }

            return true;
        }

        private async Task StartDataAcquisition()
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
            //  Create the cancellation token
            await _targetDevice.AcquisitionStartAsync();

            // start acquiring
            //  NOTE: assumes at least one input channel enabled
            //  Start a task in the background to read data
            await GetChannelData();
        }

        private async Task GetChannelData()
        {
            // capture the first channel programmed as an input (MasterChannel)
            // and use it to track data availability for all input channels
            if (_masterChannel == null)
            {
                if (_targetDevice == null)
                {
                    await FindDevice();
                }

                if (_targetDevice.Channels == null)
                {
                    ShowError("Channels are null");
                    return;
                }

                if (_targetDevice.Channels.Count == 0)
                {
                    ShowError("No Channels found");
                    return;
                }

                if (_targetDevice.Channels.Count > 1)
                {
                    ShowError("Too many channels found");
                    return;
                }

                for (var index = 0; (index <= _targetDevice.Channels.Count); index++)
                {
                    if (_targetDevice.Channels[index].GetType().GetInterfaces().Contains(typeof(IChannelIn)))
                    {
                        _masterChannel = (IChannelIn) _targetDevice.Channels[index];
                        //  we have our channel 
                        break;
                    }
                }
            }

            var voltages = new List<double>();

            //  Keep reading while acquiring data
            while (_targetDevice.IsAcquiring)
            {
                //  Read data and catch if cancelled (to exit loop and continue)
                try
                {
                    // throws an error if acquisition has been cancelled
                    // otherwise refreshes the buffer DataIn with new data
                    await _targetDevice.ReadDataAsync();
                }
                catch (OperationCanceledException ex)
                {
                    ShowError("Acquisition cancelled");
                    break;
                }
                catch (DataMisalignedException e)
                {
                    var data = e.Data;
                }

                // get here if acquisition is still active
                if (_masterChannel.DataIn.Count == 0)
                {
                    // get here if no data in the channel buffer
                    // TODO: Continue While... Warning!!! not translated
                }

                //  We have data. Convert it to strings
                double voltage = 0;
                for (var index = 0; (index <= (_masterChannel.DataIn.Count - 1)); index++)
                {
                    if (_targetDevice.Channels.Count > 1)
                    {
                        ShowError("Too many channels found");
                        return;
                    }

                    //  this is the row (scan) counter
                    foreach (var ch in _targetDevice.Channels)
                    {
                        // this is the column (channel) counter
                        // get a channel value and convert it to a string
                        if (ch.GetType().GetInterfaces().Contains(typeof(IChannelIn)))
                        {
                            voltage = ((IChannelIn) (ch)).DataIn[index];
                            voltages.Add(voltage);
                        }
                    }

                    if (voltages.Count >= 10)
                    {
                        var average = voltages.Average();
                        await SaveVoltageData(average);
                        voltages.Clear();
                    }
                }

                PurgeChannelData();
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
                        break;
                    case null:
                        ShowNameEntry = false;
                        ShowError($"Voltage {voltage} does not correspond to a status");
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

            var status = await GetStatusEntity(_currentPhoneStatus);

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

        private async Task<Status> GetStatusEntity(PhoneStatus? currentPhoneStatus)
        {
            await PopulateStatuses();

            if (currentPhoneStatus == null)
            {
                return _unknown;
            }
            else
            {
                var status = _statuses.FirstOrDefault(s => s.Name == FromCamelCase(currentPhoneStatus));
                return status ?? _unknown;
            }
        }

        public static string FromCamelCase(PhoneStatus? currentPhoneStatus)
        {
            var statusName = currentPhoneStatus?.ToString();

            if (statusName == null)
                return null;

            return Regex.Replace(statusName, "(\\B[A-Z])", " $1");
        }

        private void ConfigureAnalogChannels()
        {
            var AnalogChan = ((AnalogVoltageIn) (_targetDevice.ChannelFactory(typeof(AnalogVoltageIn), 1)));
        }

        private bool SampleRateBad()
        {
            // ensure that the sample rate per channel is not out of range
            if (_targetDevice.Channels.Count <= 0)
            {
                ShowError($"No Channels Found");
                return true;
            }

            var SampleRateRange = ((Dataq.Devices.Dynamic.IReadOnlyChannelSampleRate) (_targetDevice.Channels[0])).GetSupportedSampleRateRange(_targetDevice);
            if (!SampleRateRange.ContainsValue(SAMPLE_RATE))
            {
                ShowError($"Selected sample rate is outside the range of  {SampleRateRange.Minimum.ToString()} to {SampleRateRange.Maximum.ToString()} Hz inclusive for your current channel settings.");
                return true;
            }

            return false;
        }

        private async void SetLightColor(LedColor1 color)
        {
            await _targetDevice.Protocol.SetLedColorAsync(color);
        }
    }
}
