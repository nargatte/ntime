﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminView.Validation
{
    public class IsNumber20Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int minNumber = 0, maxNumber = 20; 
            if (value == null || !int.TryParse(value.ToString(), out int number) || number < minNumber || number > maxNumber)
            {
                return new ValidationResult(false, $"Tylko liczby z zakresu {minNumber}-{maxNumber}");
            }
            else { return ValidationResult.ValidResult; }
        }
    }
}
