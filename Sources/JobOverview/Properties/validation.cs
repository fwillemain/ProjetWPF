using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SaisieTache
{
    public class RegleChampObligatoire : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "La description de la tache doit être renseignée.");

            return ValidationResult.ValidResult;
        }
    }
}