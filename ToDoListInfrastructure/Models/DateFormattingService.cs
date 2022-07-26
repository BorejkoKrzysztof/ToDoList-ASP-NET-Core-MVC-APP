using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Models
{
    public class DateFormattingService
    {
        public string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy HH:mm");
        }
    }
}
