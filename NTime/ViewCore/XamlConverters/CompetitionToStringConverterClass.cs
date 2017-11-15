using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdminView.XamlConverters
{
    public class CompetitionToStringConverterClass : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Entities.EditableCompetition competition && value != null)
            {

                string result = "Zawody \t" + competition.EventDate.ToString("dd-MM-yyyy");
                if (competition.City != null)
                    result += $" {competition.City}";
                return result;
                //return competition.ToString("dd-MM-yyyy");
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
