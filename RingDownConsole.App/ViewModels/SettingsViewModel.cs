namespace RingDownConsole.App.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private double _minOnlineVoltage;
        private double _maxOnlineVoltage;
        private double _minConnectedVoltage;
        private double _maxConnectedVoltage;
        private double _minOnHookVoltage;
        private double _maxOnHookVoltage;
        private double _minOffHookVoltage;
        private double _maxOffHookVoltage;
        private double _minNoDialToneVoltage;
        private double _maxNoDialToneVoltage;

        public double MinOnlineVoltage
        {
            get { return _minOnlineVoltage; }
            set
            {
                if (_minOnlineVoltage != value)
                {
                    _minOnlineVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxOnlineVoltage
        {
            get { return _maxOnlineVoltage; }
            set
            {
                if (_maxOnlineVoltage != value)
                {
                    _maxOnlineVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MinConnectedVoltage
        {
            get { return _minConnectedVoltage; }
            set
            {
                if (_minConnectedVoltage != value)
                {
                    _minConnectedVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxConnectedVoltage
        {
            get { return _maxConnectedVoltage; }
            set
            {
                if (_maxConnectedVoltage != value)
                {
                    _maxConnectedVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MinOnHookVoltage
        {
            get { return _minOnHookVoltage; }
            set
            {
                if (_minOnHookVoltage != value)
                {
                    _minOnHookVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxOnHookVoltage
        {
            get { return _maxOnHookVoltage; }
            set
            {
                if (_maxOnHookVoltage != value)
                {
                    _maxOnHookVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MinOffHookVoltage
        {
            get { return _minOffHookVoltage; }
            set
            {
                if (_minOffHookVoltage != value)
                {
                    _minOffHookVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxOffHookVoltage
        {
            get { return _maxOffHookVoltage; }
            set
            {
                if (_maxOffHookVoltage != value)
                {
                    _maxOffHookVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MinNoDialToneVoltage
        {
            get { return _minNoDialToneVoltage; }
            set
            {
                if (_minNoDialToneVoltage != value)
                {
                    _minNoDialToneVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double MaxNoDialToneVoltage
        {
            get { return _maxNoDialToneVoltage; }
            set
            {
                if (_maxNoDialToneVoltage != value)
                {
                    _maxNoDialToneVoltage = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
