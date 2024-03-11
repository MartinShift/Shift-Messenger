using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Client_Wpf.Converters
{
    public class PasswordStrengthToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var passwordStrength = (int)value;
            if (passwordStrength == 0)
            {
                return Brushes.Transparent;
            }
            if (passwordStrength == 20)
            {
                return Brushes.Red;
            }
            else if (passwordStrength == 40)
            {
                return Brushes.OrangeRed;
            }
            else if (passwordStrength == 60)
            {
                return Brushes.Orange;
            }
            else if (passwordStrength == 80)
            {
                return Brushes.Yellow;
            }
            else
            {
                return Brushes.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
