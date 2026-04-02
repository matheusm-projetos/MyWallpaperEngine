using System;
using System.Globalization;
using System.Web;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyWallpaperEngine.Converters
{
    public class ImagePathConverter : System.Windows.Data.IValueConverter
    {
        public object? Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.DecodePixelWidth = 250;
                    bitmap.UriSource = new Uri(path, UriKind.Absolute);
                    bitmap.EndInit();

                    bitmap.Freeze();

                    return bitmap;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
