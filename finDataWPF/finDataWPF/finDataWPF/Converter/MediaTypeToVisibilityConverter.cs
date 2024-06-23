using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace finDataWPF.Converters
{
    public class MediaTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mediaType = value as string;
            var param = parameter as string;

            if (mediaType == param)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


