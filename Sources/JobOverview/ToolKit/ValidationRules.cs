using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace JobOverview.ToolKit
{


    public class UniqueDateTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime orderDate = (DateTime)value;
            if (ViewModel.VMMain.CurrentEmployee.ListTask.Where(c => c.Id== ViewModel.VMTaskConsultation.CurrentTask.Id)
                                                         .Select(c => c.ListWorkTime).First()
                                                         .Select(c => c.WorkingDate.ToShortDateString()).Contains(orderDate.ToShortDateString()))
                return new ValidationResult(false, "Il existe déja une date pour cette tâche.");

            return ValidationResult.ValidResult;
        }
    }

    public class MaxHourPerDayValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            if (ViewModel.VMMain.CurrentEmployee.ListTask.Select(c => c.ListWorkTime).First().Where(c=>c.WorkingDate==ViewModel.VMTaskConsultation.CurrentWorkTime.WorkingDate).Sum(c=>c.Hours)>8)
                return new ValidationResult(false, "Le maximum d'heures par jour est 8.");

            return ValidationResult.ValidResult;
        }
    }

}
