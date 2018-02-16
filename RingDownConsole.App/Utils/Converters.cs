using System;
using System.Windows;
using System.Windows.Data;
using RingDownConsole.App.ViewModels;

namespace RingDownConsole.App.Utils
{
    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool) value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(TrueValue) ?? false;
        }
    }

    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }

    public class EnumToStringConverter<T> : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            return ((Enum) value).ToString();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.Parse(typeof(T), value.ToString());
        }
    }

    public class PhoneStatusToStringConverter : EnumToStringConverter<PhoneStatus> {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var smushed = (string) base.Convert(value, targetType, parameter, culture);
            return MainViewModel.FromCamelCase(smushed);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var separated = (string) base.Convert(value, targetType, parameter, culture);
            return MainViewModel.ToCamelCase(separated);
        }
    }
}
