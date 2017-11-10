using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminView.Validation
{
    class IsNotEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (String.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "To pole nie może być muste");
            }
            else
                return ValidationResult.ValidResult;
        }
    }
}
