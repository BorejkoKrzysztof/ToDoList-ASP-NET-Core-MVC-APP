using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Enums;

namespace ToDoListInfrastructure.Models
{
    public class ProgressStatusFormatingService
    {
        public string GetTextFormattedStatus(ProgressStatus progress)
        {
            return progress.ToString().Replace('_', ' ');
        }
    }
}
