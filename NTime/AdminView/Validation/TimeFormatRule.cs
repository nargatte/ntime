using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BaseCore.TimesProcess;

namespace AdminView.Validation
{
    class TimeFormatRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var timeString = value as string;
            if (!timeString.TryConvertToDateTime(out DateTime dateTime))
                return new ValidationResult(false, "Zmienna tekstowa jest nieprawidłowa");
            return ValidationResult.ValidResult;
        }
    }
}
