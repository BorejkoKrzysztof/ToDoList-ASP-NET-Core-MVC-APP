using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Models.ViewModels
{
    public class PagingInfo
    {
        // Total amount of products.
        public int TotalItems { get; set; }

        // Amount of products on one page.
        public int ItemsPerPage { get; set; }

        // Current page info.
        public int CurrentPage { get; set; }

        // Calculated amount of pages
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
