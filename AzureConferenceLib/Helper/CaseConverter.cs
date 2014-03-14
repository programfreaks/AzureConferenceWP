using System;
using System.Globalization;
using System.Windows.Data;

namespace AzureConferenceLib.Helper
{
    public class CaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertedValue = string.Empty;

            if (value != null)
            {
                convertedValue = (string)value;

                if (parameter.ToString().ToLower() == "u")
                    convertedValue = convertedValue.ToUpper();
                else
                    convertedValue = convertedValue.ToLower();

                return convertedValue;
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
