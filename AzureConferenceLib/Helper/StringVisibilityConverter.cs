using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AzureConferenceLib.Helper
{
    public class StringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter != null)
                {
                    //i = invert.
                    if (parameter.ToString() == "i")
                    {
                        if ((string)value == string.Empty)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                    else
                    {
                        if ((string)value == string.Empty)
                            return Visibility.Collapsed;
                        else
                            return Visibility.Visible;
                    }
                }
                else
                {
                    if ((string)value == string.Empty)
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
