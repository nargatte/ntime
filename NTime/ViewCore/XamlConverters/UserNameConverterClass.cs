using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewCore.XamlConverters
{
    public class UserNameConverterClass : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string beginning = "Zalogowany jako ";
            if (value != null && value is string userName && !string.IsNullOrWhiteSpace(userName))
            {
                return beginning + userName;
            }
            else
            {
                return "Użytkownik niezalogowany";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
