using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using BaseCore.DataBase;

namespace ViewCore.XamlConverters
{
    public class TimeReadTypeToColorXamlConverterClass : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeReadTypeEnum timeReadType)
            {
                Color color =  TimeReadTypeToColorConverterClass.Convert(timeReadType);
                return new SolidColorBrush(color);
            }
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


    public static class TimeReadTypeToColorConverterClass
    {
        public static Color Convert(TimeReadTypeEnum timeReadType)
        {
            switch (timeReadType)
            {
                case TimeReadTypeEnum.Unprocessed:
                    return Colors.Gray;
                case TimeReadTypeEnum.Significant:
                    return Colors.Green;
                case TimeReadTypeEnum.NonsignificantBefore:
                    return Colors.Orange;
                case TimeReadTypeEnum.NonsignificantAfter:
                    return Colors.Brown;
                case TimeReadTypeEnum.Repeated:
                    return Colors.Blue;
                case TimeReadTypeEnum.Skipped:
                    return Colors.Red;
                case TimeReadTypeEnum.Void:
                    return Colors.Yellow;
                default:
                    return Colors.LightGray;
            }
        }

    }
}
