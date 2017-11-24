using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BaseCore.DataBase;

namespace ViewCore.XamlConverters
{
    public class IsMaleToSexEnumConverterClass : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMale)
            {
                if (isMale)
                    return SexEnum.Male;
                else
                    return SexEnum.Female;
            }
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SexEnum sex)
            {
                if (sex == SexEnum.Female)
                    return false;
                else
                    return true;
            }
            else return value;
        }
    }
}
