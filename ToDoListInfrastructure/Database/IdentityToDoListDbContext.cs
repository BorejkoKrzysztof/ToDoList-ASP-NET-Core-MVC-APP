using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Database
{
    public class IdentityToDoListDbContext : IdentityDbContext<IdentityUser>
    {
        public IdentityToDoListDbContext(DbContextOptions<IdentityToDoListDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
