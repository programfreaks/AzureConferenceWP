using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AzureConferenceLib.Helper
{
    public class CountVisibilityConverter : IValueConverter
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
                        if ((int)value == 0)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                    else
                    {
                        if ((int)value == 0)
                            return Visibility.Collapsed;
                        else
                            return Visibility.Visible;
                    }
                }
                else
                {
                    if ((int)value == 0)
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
