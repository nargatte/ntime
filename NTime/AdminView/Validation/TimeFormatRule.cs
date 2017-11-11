using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminView.Validation
{
    class TimeFormatRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var timeString = value as string;
            if (timeString is null)
                return new ValidationResult(false, "Zmienna tekstowa jest nieprawidłowa");
            string[] dividedString = timeString.Split(new char[] { ':' });
            if (dividedString.Length != 3)
                return new ValidationResult(false, "Nieprawidłowy format godziny");
            for (int i = 0; i < dividedString.Length; i++)
            {
                if (!int.TryParse(dividedString[i], out int number))
                    return new ValidationResult(false, "Znaki między dwukropkami, to nie liczby");
                int maxValue = 59;
                if (i == 0)
                    maxValue = 23;
                if (number < 0 || number > maxValue)
                    return new ValidationResult(false, "Wprowadzone liczby nie odpowiadają poprawnemu formatowi godzinowemu");
            }
            return ValidationResult.ValidResult;
        }
    }
}
