namespace RingDownConsole.App.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public double MinOnlineVoltage
        {
            get { return Settings.Default.MinOnlineVoltage; }
            set
            {
                if (Settings.Default.MinOnlineVoltage != value)
                {
                    Settings.Default.MinOnlineVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxOnlineVoltage
        {
            get { return Settings.Default.MaxOnlineVoltage; }
            set
            {
                if (Settings.Default.MaxOnlineVoltage != value)
                {
                    Settings.Default.MaxOnlineVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MinConnectedVoltage
        {
            get { return Settings.Default.MinConnectedVoltage; }
            set
            {
                if (Settings.Default.MinConnectedVoltage != value)
                {
                    Settings.Default.MinConnectedVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxConnectedVoltage
        {
            get { return Settings.Default.MaxConnectedVoltage; }
            set
            {
                if (Settings.Default.MaxConnectedVoltage != value)
                {
                    Settings.Default.MaxConnectedVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MinOnHookVoltage
        {
            get { return Settings.Default.MinOnHookVoltage; }
            set
            {
                if (Settings.Default.MinOnHookVoltage != value)
                {
                    Settings.Default.MinOnHookVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxOnHookVoltage
        {
            get { return Settings.Default.MaxOnHookVoltage; }
            set
            {
                if (Settings.Default.MaxOnHookVoltage != value)
                {
                    Settings.Default.MaxOnHookVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MinOffHookVoltage
        {
            get { return Settings.Default.MinOffHookVoltage; }
            set
            {
                if (Settings.Default.MinOffHookVoltage != value)
                {
                    Settings.Default.MinOffHookVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxOffHookVoltage
        {
            get { return Settings.Default.MaxOffHookVoltage; }
            set
            {
                if (Settings.Default.MaxOffHookVoltage != value)
                {
                    Settings.Default.MaxOffHookVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MinNoDialToneVoltage
        {
            get { return Settings.Default.MinNoDialToneVoltage; }
            set
            {
                if (Settings.Default.MinNoDialToneVoltage != value)
                {
                    Settings.Default.MinNoDialToneVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxNoDialToneVoltage
        {
            get { return Settings.Default.MaxNoDialToneVoltage; }
            set
            {
                if (Settings.Default.MaxNoDialToneVoltage != value)
                {
                    Settings.Default.MaxNoDialToneVoltage = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public int IntervalSeconds
        {
            get { return Settings.Default.IntervalSeconds; }
            set
            {
                if (Settings.Default.IntervalSeconds != value)
                {
                    Settings.Default.IntervalSeconds = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public bool PromptForName
        {
            get { return Settings.Default.PromptForName; }
            set
            {
                if (Settings.Default.PromptForName != value)
                {
                    Settings.Default.PromptForName = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }

        public string WebApiConnectionString
        {
            get { return Settings.Default.WebApiConnectionString; }
            set
            {
                if (Settings.Default.WebApiConnectionString != value)
                {
                    Settings.Default.WebApiConnectionString = value;
                    Settings.Default.Save();
                    RaisePropertyChanged();
                }
            }
        }
    }
}
