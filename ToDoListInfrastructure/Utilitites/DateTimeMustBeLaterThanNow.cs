using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Utilitites
{
    internal class DateTimeMustBeLaterThanNowAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            int validationResult = DateTime.Compare(Convert.ToDateTime(value), DateTime.Now);

            return validationResult > 0;
        }
    }
}
