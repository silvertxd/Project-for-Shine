using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.MVVM.View
{
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;
            string parameterString = parameter as string;

            return stringValue == parameterString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            string parameterString = parameter as string;

            return isChecked ? parameterString : Binding.DoNothing;
        }
    }
}
