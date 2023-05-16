using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Loarang.ViewModels
{
  public class ProgressForegroundConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double progress = (double)value;
      SolidColorBrush foreground = new SolidColorBrush(Colors.Green);

      if (progress == 100)
      {
        foreground = new SolidColorBrush(Colors.Orange);
      }
      else if (89 < progress && progress < 100)
      {
        foreground = new SolidColorBrush(Colors.Indigo);
      }
      else if (69 < progress && progress < 90)
      {
        foreground = new SolidColorBrush(Colors.DeepSkyBlue);
      }
      else if (29 < progress && progress < 70)
      {
        foreground = new SolidColorBrush(Colors.LawnGreen);
      }
      else if (10 < progress && progress < 30)
      {
        foreground = new SolidColorBrush(Colors.Gold);
      }
      else
      {
        foreground = new SolidColorBrush(Colors.Red);
      }

      return foreground;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
