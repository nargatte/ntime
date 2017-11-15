using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminView.Validation
{
    public class IsPositiveDoubleRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double minNumber = 0, maxNumber = Double.MaxValue;
            if (value == null || !int.TryParse(value.ToString(), out int number) || number <= minNumber || number > maxNumber)
            {
                return new ValidationResult(false, $"Tylko liczby z zakresu {minNumber}-{maxNumber}");
            }
            else { return ValidationResult.ValidResult; }
        }
    }
}
