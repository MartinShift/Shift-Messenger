using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client_Wpf.Converters
{
    public class PasswordStrengthToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var passwordStrength = (int)value;
            if (passwordStrength == 0)
            {
                return "";
            }
            if (passwordStrength == 20)
            {
                return "Very Weak";
            }
            else if (passwordStrength == 40)
            {
                return "Weak";
            }
            else if (passwordStrength == 60)
            {
                return "Moderate";
            }
            else if (passwordStrength == 80)
            {
                return "Good";
            }
            else
            {
                return "Perfect";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
